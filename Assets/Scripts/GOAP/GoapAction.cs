using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    public class GoapAction
    {
        public string name { get; }
        public int cost { get; set; }

        public HashSet<AgentBelief> preConditions = new HashSet<AgentBelief>();
        public HashSet<AgentBelief> effects = new HashSet<AgentBelief>();
        
        IActionStrategy actionStrategy;
        GoapAction(string name)
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
            GoapAction goapAction;

            public Builder(string name)
            {
                goapAction = new GoapAction(name)
                {
                    cost = 1
                };
            }

            public Builder WithCost(int cost)
            {
                goapAction.cost = cost;
                return this;
            }

            public Builder WithActionStrategy(IActionStrategy actionStrategy)
            {
                goapAction.actionStrategy = actionStrategy;
                return this;    
            }

            public Builder AddEffects(AgentBelief effects)
            {
                goapAction.effects.Add(effects);
                return this;
            }

            public Builder AddPreConditions(AgentBelief conditions)
            {
                goapAction.preConditions.Add(conditions);
                return this;    
            }

            public GoapAction Build()
            {
                return goapAction; 
            }
        }

    }
}