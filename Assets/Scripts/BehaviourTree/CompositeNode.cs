using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector]public List<Node> children = new List<Node>();
    }
}