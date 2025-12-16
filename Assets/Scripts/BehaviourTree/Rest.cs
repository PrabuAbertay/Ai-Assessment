using UnityEngine;
using UnityEngine.Serialization;

namespace BehaviourTree
{
    public class Rest : ActionNode
    {
        private float duration = 1;
        private float startTime;
        private int healthGain;
        private int staminaGain;

        public void SetData(float duration, int healthGain, int staminaGain)
        {
            this.duration = duration;
            this.healthGain = healthGain;
            this.staminaGain = staminaGain;
        }
        protected override void OnStart()
        {
            startTime = Time.time;
            Debug.Log($"[Rest] Data,duration: {duration},healthGain: {healthGain},staminaGain: {staminaGain}");
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                blackBoard.agent_BT.AddHealth(healthGain);
                blackBoard.agent_BT.AddStamina(staminaGain);
                return State.Success;
            }
            return State.Running;
        }
    }
}