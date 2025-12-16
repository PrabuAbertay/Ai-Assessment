using UnityEngine;

namespace BehaviourTree
{
    public class UpdateStats : ActionNode
    {
        private float updateDureation = 1;
        float startTime = 0;
        protected override void OnStart()
        {
            // startTime = blackBoard.agent_BT.LastStatUpdateTime;
            Debug.Log($"[UpdateStats] OnStart, Health: {blackBoard.agent_BT.Health}, Stamina: {blackBoard.agent_BT.Stamina} ");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            // if (Time.time - startTime > updateDureation)
            // {
            //     blackBoard.agent_BT.UpdateStats(Time.time);
            // }
            return State.Success;
        }
    }
}