using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GOAP
{
    public class WanderStrategy : IActionStrategy
    {
        public bool complete => agent.remainingDistance <= 2f && !agent.pathPending;
        public bool canPerform => !complete;
        
        NavMeshAgent agent;
        float wanderRadius;
        private Vector3 wanderAroundPos;
        public WanderStrategy(NavMeshAgent agent, float wanderRadius, Vector3 hideOutPosition)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
            wanderAroundPos = hideOutPosition;
        }

        public void Start()
        {
            Debug.Log("Wander strategy start");
            for (int i = 0; i < 5; i++)
            {
                Vector3 randDirection = Random.insideUnitSphere * wanderRadius;
                randDirection.y = 0;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(wanderAroundPos + randDirection, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    Debug.Log($"Rand position: {hit.position}");
                    return;
                }
            }
        }
    }
}