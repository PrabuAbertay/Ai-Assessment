namespace GOAP
{
    public class IdleStrategy : IActionStrategy
    {
        public bool canPerform => true;
        public bool complete { get; set; }

        
        float currentTime = 0;
        float idleDuration = 0;
        public IdleStrategy(int duration)
        {
            idleDuration = duration;
        }

        public void Start()
        {
            currentTime = 0;
        }
        public void Update(float dt)
        {
            if (currentTime >= idleDuration)
            {
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