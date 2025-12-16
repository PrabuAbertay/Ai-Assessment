using System;
using UnityEngine;

namespace GOAP
{
    public class AgentBelief
    {
        public string name { get; }

        private Func<bool> condition;
        private Func<Vector3> observedLocation;
    
        public Vector3 GetObservedLocation => observedLocation();
        public bool Evaluate() => condition();

        private AgentBelief(string name)
        {
            this.name = name;
        }
        
        public class Builder
        {
            AgentBelief agentBelief;

            public Builder(string name)
            {
                agentBelief = new AgentBelief(name);
            }

            public Builder WithCondition(Func<bool> condition)
            {
                agentBelief.condition = condition;
                return this;
            }
        
            public Builder WithLocation(Func<Vector3> location)
            {
                agentBelief.observedLocation = location;
                return this;
            }

            public AgentBelief Build()
            {
                return agentBelief;
            }
        }
    }
}
