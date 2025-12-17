using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

namespace BehaviourTree
{
    public class BehaviourTree : MonoBehaviour
    {
        Node rootNode;
        public Node.State treeState = Node.State.Running;
    
        public List<Node> nodes = new List<Node>();
        public BlackBoard blackBoard;
        
        FoodSpawner foodSpawner;
        AiAgent_BT aiAgentBT;
        
        [SerializeField] MetricsHandler metrics;


        public void Init(FoodSpawner foodSpawner1, AiAgent_BT agent)
        {
            aiAgentBT = agent;  
            foodSpawner = foodSpawner1;
            blackBoard = new BlackBoard()
            {
                agent_BT = aiAgentBT,
                foodSpawner = foodSpawner
            };
        }

        private void Start()
        {
            var updateStats = ScriptableObject.CreateInstance<UpdateStats>();
            
            var isHungry = ScriptableObject.CreateInstance<IsHungry>();
            var isInFoodAvailableArea = ScriptableObject.CreateInstance<IsInFoodAvailableArea>();   
            var isFoodAvailableInArea = ScriptableObject.CreateInstance<IsFoodAvailableInArea>();
            var getClosestFood = ScriptableObject.CreateInstance<GetClosestFoodInArea>();
            var moveToFood = ScriptableObject.CreateInstance<MoveToNextTarget>();
            var consumeFood = ScriptableObject.CreateInstance<ConsumeFood>();
            consumeFood.duration = 2;
            
            var getClosestFoodArea = ScriptableObject.CreateInstance<GetClosestFoodArea>();
            var moveToFoodArea = ScriptableObject.CreateInstance<MoveToNextTarget>();
            
            var insideFoodAreaSeq = ScriptableObject.CreateInstance<SequenceNode>();
            
            insideFoodAreaSeq.children.Add(isInFoodAvailableArea);
            insideFoodAreaSeq.children.Add(isFoodAvailableInArea);
            insideFoodAreaSeq.children.Add(getClosestFood);
            insideFoodAreaSeq.children.Add(moveToFood);
            insideFoodAreaSeq.children.Add(consumeFood);
            
            var log = ScriptableObject.CreateInstance<DebugNode>();
            log.Message = "Is not in food area--";
            
            var outsideFoodAreaSeq = ScriptableObject.CreateInstance<SequenceNode>();
            outsideFoodAreaSeq.children.Add(log);
            outsideFoodAreaSeq.children.Add(getClosestFoodArea);
            outsideFoodAreaSeq.children.Add(moveToFoodArea);
            
            var hungrySelector = ScriptableObject.CreateInstance<SelectorNode>();
            hungrySelector.children.Add(insideFoodAreaSeq);   
            hungrySelector.children.Add(outsideFoodAreaSeq);
            
            var hungrySeq = ScriptableObject.CreateInstance<SequenceNode>();
            hungrySeq.children.Add(isHungry);    
            hungrySeq.children.Add(hungrySelector); 
            
            var isTired = ScriptableObject.CreateInstance<IsTired>();
            var moveToHideOut = ScriptableObject.CreateInstance<MoveToHideout>(); 
            var rest = ScriptableObject.CreateInstance<Rest>();
            rest.SetData(5,50,100);
            
            var tiresSeq = ScriptableObject.CreateInstance<SequenceNode>();
            tiresSeq.children.Add(isTired);
            tiresSeq.children.Add(moveToHideOut);
            tiresSeq.children.Add(rest);
            
            
            var getWanderPosition = ScriptableObject.CreateInstance<GetNewWanderPosition>();
            var moveToWanderPosition = ScriptableObject.CreateInstance<MoveToNextTarget>();
            var restAtWanderPosition = ScriptableObject.CreateInstance<Rest>();
            float duration = Random.Range(1, 2);
            restAtWanderPosition.SetData(duration,0,10);

            var wanderSequence = ScriptableObject.CreateInstance<SequenceNode>();
            wanderSequence.children.Add(getWanderPosition); 
            wanderSequence.children.Add(moveToWanderPosition); 
            wanderSequence.children.Add(restAtWanderPosition); 

            
            var mainSelector = ScriptableObject.CreateInstance<SelectorNode>();      
            mainSelector.children.Add(hungrySeq);
            mainSelector.children.Add(tiresSeq);
            mainSelector.children.Add(wanderSequence);
            
            var repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
            repeatNode.child = mainSelector;
            
            rootNode = repeatNode;
            Bind(blackBoard);
        }

        private void Update()
        {
            UpdateTree();
        }

        private Node.State UpdateTree()
        {
            if (rootNode.state == Node.State.Running)
            {
                float start = Time.realtimeSinceStartup;
                treeState = rootNode.Update();
            }
            return treeState;
        }

        private List<Node> GetChildren(Node parent)
        {
            var children = new List<Node>();
            var decorator = parent as Decorator;
            if (decorator != null && decorator.child != null)  
            {
                children.Add(decorator.child);
            }
            var root = parent as RootNode;
            if (root != null && root.child != null)  
            {
                children.Add(root.child);
            }
            var composite = parent as CompositeNode;
            return composite != null ? composite.children : children;
        }
        void TraverseTree(Node node , Action<Node> action)
        {
            if(node == null)return;
            action.Invoke(node);
            var children = GetChildren(node);
            children.ForEach(child => TraverseTree(child, action));
        }

        private void Bind(BlackBoard blackBoard)
        {
            this.blackBoard = blackBoard;
            TraverseTree(rootNode, node =>
            {
                node.blackBoard = blackBoard;
            });
        }
    }
}