using Gameplay;
using UnityEngine;

namespace BehaviourTree
{
    public class GetClosestFoodArea : ActionNode
    {
        private FoodSpawnArea foodSpawnArea;
        protected override void OnStart()
        {
            foodSpawnArea = blackBoard.foodSpawner.GetClosestFoodSpawnArea(blackBoard.agent_BT.GetPosition());
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (foodSpawnArea != null)
            {
                blackBoard.agent_BT.SetCurrentFoodSpawnArea(foodSpawnArea);
                blackBoard.agent_BT.CurrentMoveToTarget = (foodSpawnArea.transform);
                Debug.Log($"Got next food area : {blackBoard.agent_BT.CurrentMoveToTarget.name}");
                return State.Success;
            }

            Debug.LogError("Cant get next food area");
            return State.Failure;
        }
    }
}