using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class BeliefFactory
    {
        private GoapAgent agent;
        Dictionary<string, AgentBelief> beliefs;
        
        public BeliefFactory(GoapAgent agent, Dictionary<string, AgentBelief> beliefs)
        {
            this.agent = agent;
            this.beliefs = beliefs;
        }

        public void AddBelief(string key, Func<bool> condition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key).WithCondition(condition).Build());
        }

        public void AddLocationBelief(string key, float distance, Transform transform)
        {
            AddLocationBelief(key,distance,transform.position);
        }
        
        public void AddLocationBelief(string key, float distance,Vector3 location)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => IsInRange(location,distance))
                .WithLocation(()=>location )
                .Build());
        }
        
        public void AddSensorBelief(string key, Sensor sensor)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(sensor.IsTargetInRange)
                .WithLocation(sensor.TargetPosition )
                .Build());
        }
        bool IsInRange(Vector3 position, float range) => Vector3.Distance(position, agent.GetPosition()) < range;
        
    }
}