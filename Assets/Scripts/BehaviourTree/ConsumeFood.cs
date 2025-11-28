using UnityEngine;

namespace BehaviourTree
{
    public class ConsumeFood : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("[ConsumeFood] OnStart ");
            blackBoard.foodSpawner.OnFoodConsumed(blackBoard.agent_BT.ClosestFood, blackBoard.agent_BT.CurrentFoodSpawnArea);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
    
    
}