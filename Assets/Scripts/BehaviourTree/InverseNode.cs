using System;

namespace BehaviourTree
{
    public class InverseNode : Decorator
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return child.Update() switch
            {
                State.Success => State.Failure,
                State.Failure => State.Success,
                State.Running => State.Running,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}