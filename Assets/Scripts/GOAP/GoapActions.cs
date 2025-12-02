using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    public class GoapActions
    {
        public string name { get; }
        public int cost { get; set; }

        private HashSet<AgentBelief> conditions = new HashSet<AgentBelief>();
        private HashSet<AgentBelief> effects = new HashSet<AgentBelief>();
        
        IActionStrategy actionStrategy;

        GoapActions(string name)
        {
            this.name = name;
        }

        public bool ActionCompleted() => actionStrategy.complete;
        
        public void Start() => actionStrategy.Start();
        public void Stop() => actionStrategy.Stop();

        public void Update(float deltaTime)
        {
            if (actionStrategy.canPerform)
            {
                actionStrategy.Update(deltaTime);
            }
            
            if(!actionStrategy.complete) return;
            
            foreach (var effect in effects)
            {
                effect.Evaluate();
            }
        }

        public class Builder
        {
            GoapActions goapActions;

            public Builder(string name)
            {
                goapActions = new GoapActions(name)
                {
                    cost = 1
                };
            }

            public Builder WithCost(int cost)
            {
                goapActions.cost = cost;
                return this;
            }

            public Builder WithActionStrategy(IActionStrategy actionStrategy)
            {
                goapActions.actionStrategy = actionStrategy;
                return this;    
            }

            public Builder AddEffects(AgentBelief effects)
            {
                goapActions.effects.Add(effects);
                return this;
            }

            public Builder AddConditions(AgentBelief conditions)
            {
                goapActions.conditions.Add(conditions);
                return this;    
            }

            public GoapActions Build()
            {
                return goapActions; 
            }
        }

    }
}