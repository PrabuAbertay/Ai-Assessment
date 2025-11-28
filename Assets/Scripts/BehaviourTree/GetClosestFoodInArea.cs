using Gameplay;
using UnityEngine;

namespace BehaviourTree
{
    public class GetClosestFoodInArea : ActionNode
    {
        private Food food;

        protected override void OnStart()
        {
            Debug.Log("[GetClosestFoodInArea] OnStart ");
            food = blackBoard.agent_BT.CurrentFoodSpawnArea.GetClosestFood(blackBoard.agent_BT.GetPosition());
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (food == null)
            {
                Debug.LogError("Cant get food");
                return State.Failure;
            }
            blackBoard.agent_BT.SetClosestFood(food);
            blackBoard.agent_BT.CurrentMoveToTarget = (food.transform);
            return State.Success;
        }
    }
}