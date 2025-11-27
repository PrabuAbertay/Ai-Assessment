using Gameplay;

namespace BehaviourTree
{
    public class IsInFoodAvailableArea : ActionNode
    {
        FoodSpawnArea foodSpawnArea;     
        protected override void OnStart()
        {
            foodSpawnArea = null;
        }

        protected override void OnStop()
        {
            throw new System.NotImplementedException();
        }

        protected override State OnUpdate()
        {
            if (blackBoard.foodSpawner.IsInFoodAvailableArea(blackBoard.agent_BT.GetPosition(), out foodSpawnArea))
            {
                blackBoard.agent_BT.SetCurrentFoodSpawnArea(foodSpawnArea);
                return State.Success;
            }

            return State.Failure;
        }
    }
}