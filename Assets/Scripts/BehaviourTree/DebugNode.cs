using UnityEngine;

namespace BehaviourTree
{
    public class DebugNode : ActionNode
    {
        private string message;

        public string Message
        {
            set => message = value;
        }

        protected override void OnStart()
        {
            Debug.Log("Debug Node (message) [OnStart]: " + message);
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            Debug.Log("Debug Node (message) [Update]: " + message);
            return State.Success;
        }
    }
}