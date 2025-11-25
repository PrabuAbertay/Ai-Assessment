using System;

namespace BehaviourTree
{
    public class SequenceNode : CompositeNode
    {
        int currentChildId;   
        protected override void OnStart()
        {
            currentChildId = 0; 
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var child = children[currentChildId];
            switch (child.Update())
            {
                case State.Success:
                    currentChildId++;
                    break;
                case State.Failure:
                    return State.Failure;
                case State.Running:
                    return State.Running;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return currentChildId < children.Count ? State.Running : State.Success;       
        }
    }
}