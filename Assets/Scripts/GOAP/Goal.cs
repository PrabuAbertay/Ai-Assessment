using System.Collections.Generic;

namespace GOAP
{
    public class Goal
    {
        public string name { get; }
        public float priority { get; set; }

        public HashSet<AgentBelief> desiredEffects = new HashSet<AgentBelief>();

        Goal(string name)
        {
            this.name = name;
        }

        public class Builder
        {
            Goal goal;

            public Builder(string name)
            {
                goal= new Goal(name);
            }

            public Builder WithPriority(int priority)
            {
                goal.priority = priority;      
                return this;
            }

            public Builder WithDesiredEffects(AgentBelief desiredEffect)
            {
                goal.desiredEffects.Add(desiredEffect);
                return this;    
            }

            public Goal Build()
            {
                return goal;
            }
        }
    }
}