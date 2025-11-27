using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "Create Node", fileName = "Node", order = 0)]
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Success,
            Failure,    
            Running
        }
        
        public State state = State.Running;
        private bool isStarted;
        public BlackBoard blackBoard;   

        public State Update()
        {
            if (!isStarted)
            {
                OnStart();
                isStarted = true;
            }
        
            state = OnUpdate();

            if (state == State.Success || state == State.Failure)
            {
                OnStop();
                isStarted = false;
            }
            return state;
        }
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}
