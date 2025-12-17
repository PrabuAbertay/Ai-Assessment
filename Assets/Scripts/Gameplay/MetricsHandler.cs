using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class MetricsHandler : MonoBehaviour
    {
        List<float> psDecisionTimer = new List<float>();
        List<float> decisionTimerAvg = new List<float>();

        private float simulationDuratoin = 60;
        private float currentSimulationTime = 0;
        private float currentTime = 0;
        private bool recieveData;
        private bool stopSimulation;

        private void Start()
        {
            recieveData = true;
        }

        private void Update()
        {
            if(stopSimulation) return;
            if (currentSimulationTime < simulationDuratoin)
            {
                currentSimulationTime += Time.deltaTime;
                currentTime += Time.deltaTime;
                if (currentTime >= 1)
                {
                    CalculatePSMetrics();
                    currentTime = 0;
                }
            }
            else
            {
                CalculatePerSimulation();
                recieveData = false;  
                stopSimulation = true;
            }
        }

        public void SetDecisionTime(float decisionTime)
        {
            if(!recieveData)return;
            
            psDecisionTimer.Add(decisionTime);
        }

        void CalculatePSMetrics()
        {
            float total = 0;
            foreach (var f in psDecisionTimer)
            {
                if(float.IsNaN(f)) continue;
                total += f;
            }
            
            var avgPerSec = total / psDecisionTimer.Count;
            psDecisionTimer.Clear();
            Debug.Log($"Average Per Second: {avgPerSec}");
            if (!float.IsNaN(avgPerSec))
            {
                decisionTimerAvg.Add(avgPerSec);
            }
            else
            {
                simulationDuratoin++;
            }
        }

        void CalculatePerSimulation()
        {
            float total = 0;
            foreach (var f in decisionTimerAvg)
            {
                total += f;
            }
            
            var avg = total / decisionTimerAvg.Count;
            Debug.Log($"Average Per Simulation(60 sec): {avg}");
            
        }


        public void SetDecisionTimeGoap(float realtimeSinceStartup)
        {
            throw new NotImplementedException();
        }
    }
}