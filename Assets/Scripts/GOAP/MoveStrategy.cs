using System;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public class MoveStrategy : IActionStrategy
    {
        public bool complete => agent.remainingDistance <= 0.5f && !agent.pathPending;
        public bool canPerform => !complete;
        
        NavMeshAgent agent;
        Func<Vector3> destination;
        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination)
        {
            this.agent = agent;
            this.destination = destination;
        }

        public void Start()
        {
            Debug.DrawLine(agent.transform.position, destination(), Color.red);
            agent.SetDestination(destination());
        }

        public void Stop() => agent.ResetPath();
    }
}