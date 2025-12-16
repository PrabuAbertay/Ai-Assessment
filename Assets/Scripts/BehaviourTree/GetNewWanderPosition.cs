using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class GetNewWanderPosition : ActionNode
    {
        
        protected override void OnStart()
        {
            Debug.Log("[GetNewWanderPosition] OnStart ");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 randDirection = Random.insideUnitSphere * blackBoard.agent_BT.WanderRadius;
                randDirection.y = 0;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(blackBoard.agent_BT.GetPosition() + randDirection, out hit, blackBoard.agent_BT.WanderRadius, NavMesh.AllAreas))
                {
                    blackBoard.agent_BT.CurrentMoveToPosition = hit.position;
                    Debug.Log($"Rand position: {hit.position}");
                    return State.Success;
                }
            }
            return State.Failure;
        }
    }
}