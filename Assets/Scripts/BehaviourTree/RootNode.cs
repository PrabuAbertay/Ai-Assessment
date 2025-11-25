using UnityEngine;

namespace BehaviourTree
{
    public class RootNode : Node
    {
        [HideInInspector]public Node child;
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return child.Update();
        }
    }
}