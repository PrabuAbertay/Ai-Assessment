using Gameplay;
using UnityEngine;

namespace BehaviourTree
{
    public class IsHungry : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log($"[IsHungry] OnStart, Health: {blackBoard.agent_BT.Health}");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return blackBoard.agent_BT.Health < 50 ? State.Success : State.Failure;
        }
    }
}