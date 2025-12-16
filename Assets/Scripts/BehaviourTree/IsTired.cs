using UnityEngine;

namespace BehaviourTree
{
    public class IsTired : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log($"[IsTired] OnStart, stamina: {blackBoard.agent_BT.Stamina}");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return blackBoard.agent_BT.Stamina < 30 ? State.Success : State.Failure;
        }
    }
}