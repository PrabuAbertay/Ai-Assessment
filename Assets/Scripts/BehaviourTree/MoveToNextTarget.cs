using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    public class MoveToNextTarget : ActionNode
    {
        private Transform target;
        private NavMeshAgent navMeshAgent;

        protected override void OnStart()
        {
            navMeshAgent = blackBoard.agent_BT.NavMeshAgent;
            target = blackBoard.agent_BT.CurrentMoveToTarget.transform;
            Debug.Log($"[MoveToNextFood] OnStart , target : {target.name}, stopping distance : {navMeshAgent.stoppingDistance}");
            Debug.DrawLine(blackBoard.agent_BT.transform.position, target.transform.position, Color.green,1000);
                
            navMeshAgent.isStopped = false; 
            navMeshAgent.destination = target.position;     
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (target == null)
            {
                Debug.LogError("Next Food cant be found ");
                return State.Failure;
            }
            var distance = Vector2.Distance(blackBoard.agent_BT.transform.position, target.position);   
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