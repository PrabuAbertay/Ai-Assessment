using UnityEngine;

namespace BehaviourTree
{
    public class ConsumeFood : ActionNode
    {
        public float duration = 1;
        private float startTime;
        protected override void OnStart()
        {
            startTime = Time.time;
            Debug.Log("[ConsumeFood] OnStart ");
            
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                blackBoard.foodSpawner.OnFoodConsumed(blackBoard.agent_BT.ClosestFood, blackBoard.agent_BT.CurrentFoodSpawnArea);
                blackBoard.agent_BT.AddHealth(blackBoard.agent_BT.ClosestFood.HealthValue);
                blackBoard.agent_BT.AddStamina(2);
                return State.Success;
            }
            return State.Running;   
        }
    }
}