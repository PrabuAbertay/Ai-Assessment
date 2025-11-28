using UnityEngine;

namespace BehaviourTree
{
    public class MoveToNextFoodArea : ActionNode
    {
        private Transform target;

        protected override void OnStart()
        {
            target = blackBoard.agent_BT.CurrentMoveToTarget;
            blackBoard.agent_BT.CurrentMoveToTarget = target;
            blackBoard.agent_BT.MoveToTarget();
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (target == null)
            {
                Debug.LogError("Next Food cant be found ");
                return State.Failure;
            }
            if (blackBoard.agent_BT.IsMovingToTarget())
            {
                return State.Running;
            }
            blackBoard.agent_BT.StopMovingToTarget();
            return State.Success;
        }
    }
}