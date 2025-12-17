using System;

namespace GOAP
{
    public class IdleStrategy : IActionStrategy
    {
        public bool canPerform => true;
        public bool complete { get; set; }

        
        float currentTime = 0;
        float idleDuration = 0;
        event Action OnComplete; 
        public IdleStrategy(float duration, Action onComplete)
        {
            idleDuration = duration;
            OnComplete = onComplete;
        }

        public void Start()
        {
            currentTime = 0;
        }
        public void Update(float dt)
        {
            if (currentTime >= idleDuration)
            {
                OnComplete?.Invoke();   
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