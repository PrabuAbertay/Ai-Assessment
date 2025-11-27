using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTree : MonoBehaviour
    {
        Node rootNode;
        public Node.State treeState;
    
        public List<Node> nodes = new List<Node>();
        public BlackBoard blackBoard;
        
        [SerializeField] AiAgent_BT aiAgentBT;
        [SerializeField] FoodSpawner foodSpawner;

        private void OnValidate()
        {
            if(aiAgentBT == null) aiAgentBT = GetComponent<AiAgent_BT>();
        }

        private void Awake()
        {
            blackBoard = new BlackBoard()
            {
                agent_BT = aiAgentBT,
                foodSpawner = foodSpawner
            };
        }

        private void Start()
        {
            var log = ScriptableObject.CreateInstance<DebugNode>();
            log.Message = "Hello World! - 1";
            
            var log2 = ScriptableObject.CreateInstance<DebugNode>();
            log2.Message = "Hello World! - 2";
            
            var delay = ScriptableObject.CreateInstance<DelayNode>();
            var delay2 = ScriptableObject.CreateInstance<DelayNode>();
            
            
            var sequence = ScriptableObject.CreateInstance<SequenceNode>();
            sequence.children.Add(log);
            sequence.children.Add(delay);
            sequence.children.Add(log2);
            sequence.children.Add(delay2);
            
            
            var loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.child = sequence;
            
            rootNode = loop;
        }

        private void Update()
        {
            UpdateTree();
        }

        public Node.State UpdateTree()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }
        public List<Node> GetChildren(Node parent)
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
        public void Bind(BlackBoard blackBoard)
        {
            this.blackBoard = blackBoard;
            TraverseTree(rootNode, node =>
            {
                node.blackBoard = blackBoard;
            });
        }
    }
}