using Unity.VisualScripting;

namespace GOAP
{
    public interface IActionStrategy
    {
        public bool canPerform { get; }
        public bool complete { get;}

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