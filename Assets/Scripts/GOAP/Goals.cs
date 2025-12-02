using System.Collections.Generic;

namespace GOAP
{
    public class Goals
    {
        public string name { get; }
        public int priority { get; set; }

        HashSet<AgentBelief> desiredEffects = new HashSet<AgentBelief>();

        Goals(string name)
        {
            this.name = name;
        }

        public class Builder
        {
            Goals goals;

            public Builder(string name)
            {
                goals= new Goals(name);
            }

            public Builder WithPriority(int priority)
            {
                goals.priority = priority;      
                return this;
            }

            public Builder WithDesiredEffects(AgentBelief desiredEffect)
            {
                goals.desiredEffects.Add(desiredEffect);
                return this;    
            }

            public Goals Build()
            {
                return goals;
            }
        }
    }
}