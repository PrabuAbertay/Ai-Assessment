namespace GOAP
{
    public interface IActionStrategy
    {
        bool canPerform { get; }
        bool complete { get; }

        public void Start()
        {
            
        }
        public void Stop()
        {
            
        }

        public void Update(float dt)
        {
            
        }
    }
}