using UnityEngine;

namespace BehaviourTree
{
    public class DelayNode : ActionNode
    {
        public float delay = 1;
        private float startTime;
        protected override void OnStart()
        {
            startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return Time.time - startTime > delay ? State.Success : State.Running;
        }
    }
}