using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

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
            var isInFoodAvailableArea = ScriptableObject.CreateInstance<IsInFoodAvailableArea>();   
            var isFoodAvailableInArea = ScriptableObject.CreateInstance<IsFoodAvailableInArea>();
            var getClosestFood = ScriptableObject.CreateInstance<GetClosestFoodInArea>();
            var moveToFood = ScriptableObject.CreateInstance<MoveToNextTarget>();
            var consumeFood = ScriptableObject.CreateInstance<ConsumeFood>();
            
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
            
            var selector = ScriptableObject.CreateInstance<SelectorNode>();
            selector.children.Add(insideFoodAreaSeq);   
            selector.children.Add(outsideFoodAreaSeq);   
            
            var repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
            repeatNode.child = selector;
            
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