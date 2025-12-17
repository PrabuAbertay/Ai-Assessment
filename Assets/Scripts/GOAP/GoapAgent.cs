using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BehaviourTree;
using Gameplay;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GOAP
{
    [RequireComponent(typeof(NavMeshAgent)) , RequireComponent(typeof(Rigidbody))]
    public class GoapAgent : MonoBehaviour
    {
        [SerializeField] FoodSpawner foodSpawner;
        [SerializeField] NavMeshAgent navAgent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform hideOut;
        [SerializeField]private int health = 100;
        [SerializeField]private int stamina = 100;
        private float statsUpdateTimer = 2f;
        float currentTime = 0f; 

        private GameObject target;
        private Vector3 destination;
        
        

        public Goal currentGoal;
        public Goal lastGoal;
        public GoapAction currentAction;
        public ActionPlan actionPlan;
        public IGoapPlanner planner;
        
        public Dictionary<string, AgentBelief> beliefs = new Dictionary<string, AgentBelief>();
        public HashSet<GoapAction> actions = new HashSet<GoapAction>();
        public HashSet<Goal> goals = new HashSet<Goal>();
        [SerializeField]private bool isInFoodArea;
        [SerializeField]private MetricsHandler metrics;
        private Food availableFood;
        [SerializeField]private UiManager uiManager;

        private void OnValidate()
        {
            if(navAgent == null) navAgent = GetComponent<NavMeshAgent>();
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                rb.freezeRotation = true;   
            }
        }

        private void Start()
        {
            SetUpBeliefs();
            SetUpActions();
            SetUpGoals();
            
            planner = new GoapPlanner();
        }

        private void Update()
        {
            UpdateUi();
            if (currentTime < statsUpdateTimer)
            {
                currentTime += Time.deltaTime;      
            }
            else
            {
                currentTime = 0f;
                UpdateStats();
            }

            if (currentAction == null)
            {
                CalculatePlan();

                if (actionPlan != null && actionPlan.actions.Count > 0)
                {
                    navAgent.ResetPath();
                    currentGoal = actionPlan.Goal;
                    currentAction = actionPlan.actions.Pop();
                    // currentAction.Start();
                    Debug.Log($"<color=red>Current goal: {currentGoal.name}, Current action: {currentAction.name}</color>");
                    
                    if (currentAction.preConditions.All(b => b.Evaluate())) {
                        currentAction.Start();
                    } else {
                        Debug.Log("Preconditions not met, clearing current action and goal");
                        currentAction = null;
                        currentGoal = null;
                    }
                }
            }
            
            //Update current action
            if (currentAction != null && actionPlan != null)
            {
                currentAction.Update(Time.deltaTime);

                if (currentAction.ActionCompleted())
                {
                    Debug.Log($"Action completed: {currentAction.name}");
                    currentAction.Stop();
                    currentAction = null;

                    if (actionPlan.actions.Count <= 0)
                    {
                        Debug.Log($"Plan executed");
                        lastGoal = currentGoal;
                        currentGoal = null;
                    }
                }
            }
        }

        private void UpdateUi()
        {
            uiManager.UpdateHealthText(health);
            uiManager.UpdateStaminaText(stamina);
        }

        void CalculatePlan()
        {
            var priorityLevel = currentGoal?.priority ?? 0;

            var goalsToCheck = goals;

            if (currentGoal != null)
            {
                // Debug.Log("Current goal != null, checking goals with higher priority");
                goalsToCheck = new HashSet<Goal>(goals.Where(g => g.priority > priorityLevel));
            }
            
            if(goalsToCheck.Count == 0) return;     
            float start = Time.realtimeSinceStartup;
            var newPlan = planner.Plan(this, goalsToCheck, lastGoal);
            if (newPlan != null)
            {
                metrics.SetDecisionTime((Time.realtimeSinceStartup - start) * 1000f); // ms
                actionPlan = newPlan;
            }
        }

        private void UpdateStats()
        {
            AddHealth(-3);
            AddStamina(-3);
        }

        bool IsInRange(Vector3 position, float range)
        {
            return Vector3.Distance(transform.position, position) < range;
        }

        void AddHealth(int value)
        {
            health += value;
            health = math.clamp(health, 0, 100);
        }
        void AddStamina(int value)
        {
            stamina += value;
            stamina = math.clamp(stamina, 0, 100);
        }

        private void SetUpGoals()
        {
            goals = new HashSet<Goal>();

            goals.Add(new Goal.Builder(GoapStrings.Chill)
                .WithPriority(1)
                .WithDesiredEffects(beliefs[GoapStrings.Nothing]).Build());
            
            goals.Add(new Goal.Builder(GoapStrings.Wander)
                .WithPriority(1)
                .WithDesiredEffects(beliefs[GoapStrings.Moving]).Build());
            
            goals.Add(new Goal.Builder(GoapStrings.StayHealthy)
                .WithPriority(2)
                .WithDesiredEffects(beliefs[GoapStrings.AgentHealthy]).Build());
            
            goals.Add(new Goal.Builder(GoapStrings.StayActive)
                .WithPriority(2)
                .WithDesiredEffects(beliefs[GoapStrings.AgentRested]).Build());
        }

        private void SetUpActions()
        {
            actions = new HashSet<GoapAction>();

            float duration = Random.Range(1, 2);
            actions.Add(new GoapAction.Builder(GoapStrings.Relax)
                .WithActionStrategy(new IdleStrategy(duration,() => AddStamina(5)))
                .AddEffects(beliefs[GoapStrings.Nothing]).Build());
            
            actions.Add(new GoapAction.Builder(GoapStrings.WanderAround)
                .WithActionStrategy(new WanderStrategy(navAgent, 20, transform.position))
                .AddEffects(beliefs[GoapStrings.Moving]).Build());
            
            actions.Add(new GoapAction.Builder(GoapStrings.MoveToFoodArea)
                .WithActionStrategy(new MoveStrategy(navAgent, GetClosestFoodSpawnArea))
                .AddEffects(beliefs[GoapStrings.AgentAtFoodArea]).Build());
            
            actions.Add(new GoapAction.Builder(GoapStrings.MoveToFood)
                .WithActionStrategy(new MoveStrategy(navAgent, GetClosestFood))
                .AddPreConditions(beliefs[GoapStrings.AgentAtFoodArea])
                .AddEffects(beliefs[GoapStrings.AgentAtFood]).Build());
            
            actions.Add(new GoapAction.Builder(GoapStrings.Eat)
                .WithActionStrategy(new InteractStrategy(2, OnEatCallback))
                .AddPreConditions(beliefs[GoapStrings.AgentAtFood])
                .AddEffects(beliefs[GoapStrings.AgentHealthy]).Build());
            
            actions.Add(new GoapAction.Builder(GoapStrings.MoveToHideout)
                .WithActionStrategy(new MoveStrategy(navAgent,() => hideOut.position))
                .AddEffects(beliefs[GoapStrings.AgentAtHideOut]).Build());
            
            actions.Add(new GoapAction.Builder(GoapStrings.Rest)
                .WithActionStrategy(new IdleStrategy(5,() => AddStamina(80)))
                .AddPreConditions(beliefs[GoapStrings.AgentAtHideOut])
                .AddEffects(beliefs[GoapStrings.AgentRested]).Build());
            
        }

        

        private void SetUpBeliefs()
        {
            beliefs = new Dictionary<string, AgentBelief>();
            
            BeliefFactory factory = new BeliefFactory(this, beliefs);
            factory.AddBelief(GoapStrings.Nothing, ()=> false);
            factory.AddBelief(GoapStrings.Idle, ()=> !navAgent.hasPath);
            factory.AddBelief(GoapStrings.Moving, ()=> navAgent.hasPath);
            factory.AddBelief(GoapStrings.AgentHealthy, ()=> health >= 50);
            factory.AddBelief(GoapStrings.AgentUnHealthy, ()=> health < 10);
            factory.AddBelief(GoapStrings.AgentRested, ()=> stamina >= 50);
            factory.AddBelief(GoapStrings.AgentTired, ()=> stamina < 10);
            factory.AddBelief(GoapStrings.AgentAtFoodArea, () =>isInFoodArea);
            factory.AddBelief(GoapStrings.AgentAtFood, () => (availableFood != null));
            
            factory.AddLocationBelief(GoapStrings.AgentAtHideOut, 3f, hideOut);
            
            
            
        }
        private void OnEatCallback()
        {
            if (availableFood == null)
            {
                Debug.LogError("Food Unavailable");
                return;
            }
            
            availableFood.Consumed();
            AddHealth(availableFood.HealthValue);
            AddStamina(2);
            availableFood = null;   
        }
        Vector3 GetClosestFoodSpawnArea()
        {
            var spawner = foodSpawner.GetClosestFoodSpawnArea(transform.position);
            var pos = spawner.GetPosition();
            var closestPoint =    spawner.GetClosestPointInArea(transform.position);
            var newPos = Vector3.Lerp(pos, closestPoint, .9f);  
            newPos.y = 0;
            return newPos;
        }
        Vector3 GetClosestFood()
        {
            var spawnArea = foodSpawner.GetClosestFoodSpawnArea(transform.position);    
            return foodSpawner.GetClosestFood(transform.position, spawnArea).GetPosition();  
        }
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Food"))
            {
                Debug.Log("Got food");
                availableFood = other.GetComponent<Food>();  
            }
            if (other.CompareTag("FoodArea"))
            {
                isInFoodArea = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("FoodArea"))
            {
                isInFoodArea = false;
            }
        }

        public void SetHideout(Hideout hideout)
        {
            hideOut = hideout.GetTransform();
        }
    }

    public static class GoapStrings
    {
        public const string Nothing = "Nothing";    
        public const string Idle = "Idle";  
        public const string Moving = "Moving";
        public const string AgentHealthy = "AgentHealthy";  
        public const string AgentUnHealthy = "AgentUnHealthy";  
        public const string AgentRested = "AgentRested";            
        public const string AgentTired = "AgentTired";  
        public const string AgentAtFoodArea = "AgentAtFoodArea";
        public const string AgentAtFood = "AgentAtFood";
        public const string AgentAtHideOut = "AgentAtHideOut";
        public const string Rest = "Rest";
        public const string Relax = "Relax";
        public const string WanderAround = "WanderAround";
        public const string Wander = "Wander";
        public const string Chill = "Chill";
        public const string MoveToFoodArea = "MoveToFoodArea";
        public const string MoveToHideout = "MoveToHideout";
        public const string MoveToFood = "MoveToFood";
        public const string StayHealthy = "StayHealthy";
        public const string StayActive = "StayActive";
        public const string Eat = "Eat";
        
    }
}