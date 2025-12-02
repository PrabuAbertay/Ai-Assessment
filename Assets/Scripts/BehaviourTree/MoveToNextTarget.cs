using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class MoveToNextTarget : ActionNode
    {
        private Vector3 target;
        private NavMeshAgent navMeshAgent;

        protected override void OnStart()
        {
            navMeshAgent = blackBoard.agent_BT.NavMeshAgent;
            target = blackBoard.agent_BT.CurrentMoveToPosition;
            Debug.Log($"[MoveToNextFood] OnStart , stopping distance : {navMeshAgent.stoppingDistance}");
            Debug.DrawLine(blackBoard.agent_BT.transform.position, target, Color.green,1000);
                
            navMeshAgent.isStopped = false; 
            navMeshAgent.destination = target;     
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var distance = Vector3.Distance(blackBoard.agent_BT.GetPosition(), target);   
            if (distance > navMeshAgent.stoppingDistance)
            {
                Debug.Log($"Target Distance : {distance}");
                return State.Running;
            }
            navMeshAgent.isStopped = true; 
            return State.Success;   
        }
    }
}