using System;
using System.Data;
using Gameplay;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiAgent_BT : MonoBehaviour
{
    FoodSpawner foodSpawner;
    FoodSpawnArea currentFoodSpawnArea;
    private Food closestFood;
    private Transform currentMoveToTarget;
    private Vector3 currentMoveToPosition;
    [SerializeField]private int health = 100, stamina = 100;
    [SerializeField]BehaviourTree.BehaviourTree behaviourTree;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] private Transform hideOut;
    [SerializeField] private UiManager uiManager;
    private float statUpdateInterval = 1f;
    private float currentIntervalTime;

    public Transform Hideout => hideOut;
    public int WanderRadius { get; } = 20;

    public Transform CurrentMoveToTarget
    {
        get => currentMoveToTarget;
        set => currentMoveToTarget = value;
    }
    
    public Vector3 CurrentMoveToPosition
    {
        get => currentMoveToPosition;
        set => currentMoveToPosition= value;
    }

    public int Health => health;
    public int Stamina => stamina;
    public FoodSpawnArea CurrentFoodSpawnArea => currentFoodSpawnArea;
    public Food ClosestFood => closestFood;
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    private void OnValidate()
    {
        if(navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
        if(behaviourTree == null) behaviourTree = GetComponent<BehaviourTree.BehaviourTree>();
    }

    public void Init(FoodSpawner foodSpawner)
    {
        this.foodSpawner = foodSpawner;
        behaviourTree.Init(foodSpawner, this);
    }

    private void Start()
    {
        currentIntervalTime = Time.time;    
    }

    private void Update()
    {
        UpdateUi();
        if (!(Time.time - currentIntervalTime >= statUpdateInterval)) return;
        UpdateStats();
        currentIntervalTime = Time.time;
    }

    private void UpdateUi()
    {
        uiManager.UpdateHealthText(health);
        uiManager.UpdateStaminaText(stamina);
    }

    public void SetCurrentFoodSpawnArea(FoodSpawnArea newArea)
    {
        currentFoodSpawnArea = newArea;
    }

    public bool IsFoodAvailableInArea()
    {
        return currentFoodSpawnArea.IsFoodAvailable();
    }

    public void SetClosestFood(Food food)
    {
        closestFood = food;
    }
    

    public void GetClosestFood()
    {
        if (currentFoodSpawnArea != null)
        {
            closestFood = currentFoodSpawnArea.GetClosestFood(transform.position);
            return;
        }

        Debug.LogError("[AiAgent_BT] currentFoodSpawnArea == null");
    }

    public void MoveToTarget()
    {
        if (currentMoveToTarget == null)
        {
            Debug.LogError("[AiAgent_BT] currentMoveToTarget == null");
            return;
        }
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = (currentMoveToTarget.position);
    }

    public bool IsMovingToTarget()
    {
        return navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
    }

    public void StopMovingToTarget()
    {
        navMeshAgent.isStopped = true;
        currentMoveToTarget = null; 
    }

    public void ConsumeFood()
    {
        if (closestFood == null || currentFoodSpawnArea == null)
        {
            Debug.LogError($"[AiAgent_BT] closestFood == null : {closestFood == null} , currentFoodSpawnArea == null : {currentFoodSpawnArea == null}");
            return;
        }
        foodSpawner.OnFoodConsumed(closestFood, currentFoodSpawnArea);
    }

    public void GetClosestFoodArea()
    {
        currentFoodSpawnArea = foodSpawner.GetClosestFoodSpawnArea(transform.position);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void UpdateStats()
    {
        AddHealth(-3);
        AddStamina(-3);
    }
    
    public void AddHealth(int value)
    {
        health += value;
        health = math.clamp(health, 0, 100);
    }
    public void AddStamina(int value)
    {
        stamina += value;
        stamina = math.clamp(stamina, 0, 100);
    }

    public void SetHideout(Hideout hideout)
    {
        this.hideOut = hideout.GetTransform(); 
    }
}