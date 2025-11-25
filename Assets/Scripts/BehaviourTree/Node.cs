namespace BehaviourTree
{
    public abstract class Node
    {
        public enum State
        {
            Success,
            Failure,    
            Running
        }
        
        protected State state;
        private bool isStarted;

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
