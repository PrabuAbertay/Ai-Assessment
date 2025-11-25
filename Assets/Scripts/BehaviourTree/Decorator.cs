using UnityEngine;

namespace BehaviourTree
{
    public abstract class Decorator : Node
    {
        [HideInInspector]public Node child;
    }
}