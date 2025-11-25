using System;

namespace BehaviourTree
{
    public class SelectorNode : CompositeNode
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
                    return State.Success;   
                    break;
                case State.Failure:
                    currentChildId++;
                    break;
                case State.Running:
                    return State.Running;   
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return currentChildId < children.Count ? State.Running : State.Failure;       
        }
    }
}