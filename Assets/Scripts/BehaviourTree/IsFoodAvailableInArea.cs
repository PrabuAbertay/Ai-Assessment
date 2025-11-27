namespace BehaviourTree
{
    public class IsFoodAvailableInArea : ActionNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return (blackBoard.agent_BT.CurrentFoodSpawnArea.IsFoodAvailable()) ? State.Success : State.Failure;
        }
    }
}