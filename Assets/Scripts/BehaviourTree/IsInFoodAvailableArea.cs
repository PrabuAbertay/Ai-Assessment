using Gameplay;
using UnityEngine;

namespace BehaviourTree
{
    public class IsInFoodAvailableArea : ActionNode
    {
        FoodSpawnArea foodSpawnArea;     
        protected override void OnStart()
        {
            Debug.Log("[IsInFoodAvailableArea] OnStart ");
            foodSpawnArea = null;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (blackBoard.foodSpawner.IsInFoodAvailableArea(blackBoard.agent_BT, out foodSpawnArea))
            {
                blackBoard.agent_BT.SetCurrentFoodSpawnArea(foodSpawnArea);
                return State.Success;
            }

            return State.Failure;
        }
    }
}