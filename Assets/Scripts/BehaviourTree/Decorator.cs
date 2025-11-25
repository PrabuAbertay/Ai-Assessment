using UnityEngine;

namespace BehaviourTree
{
    public abstract class Decorator : Node
    {
        [HideInInspector]protected Node child;
    }
}