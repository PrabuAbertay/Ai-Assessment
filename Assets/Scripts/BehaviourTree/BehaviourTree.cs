using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class BehaviourTree : MonoBehaviour
    {
        Node rootNode;
        public Node.State treeState;
    
        public List<Node> nodes = new List<Node>();
        public BlackBoard blackBoard = new BlackBoard();

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
    }
}