using System;

namespace GOAP
{
    public class InteractStrategy : IActionStrategy
    {
        public bool canPerform => true;
        public bool complete { get; set; }

        
        float currentTime = 0;
        float idleDuration = 0;
        event Action OnInteract;
        public InteractStrategy(int duration, Action callback)
        {
            idleDuration = duration;
            OnInteract = callback;  
        }

        public void Start()
        {
            currentTime = 0;
        }
        public void Update(float dt)
        {
            if (currentTime >= idleDuration)
            {
                OnInteract?.Invoke();
                complete = true;
            }
            else
            {
                complete = false;
                currentTime += dt;
            }
        }
    }
}