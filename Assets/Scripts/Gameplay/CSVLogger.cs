using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Gameplay
{
    public static class CSVLogger
    {
        private static string path;
        public static void CSVLogDecisionTimeBt(string fileName, DecisionTimeMetric metric)
        {
            path = Application.persistentDataPath + "/" + fileName;
                
            StringBuilder sb = new StringBuilder();
                
            if (!File.Exists(path)) sb.AppendLine("AgentCount,AverageDecisionTime");
                
            foreach (var data in metric.data)
            {
                sb.AppendLine($"{data.agentCount},{data.avgTime}");
            }
            File.AppendAllText(path, sb.ToString());
            Debug.Log($"CSV exported, path: {path}");
        }
    }
    [Serializable]
    public class DecisionTimeMetric
    {
        public DecisionTimeData[] data;

        public DecisionTimeMetric(DecisionTimeData[] data)
        {
            this.data = data;
        }
    }

    [Serializable]
    public class DecisionTimeData
    {
        public int agentCount;
        public float avgTime;

        public DecisionTimeData(int agentCount, float avgTime)
        {
            this.agentCount = agentCount;
            this.avgTime = avgTime;
        }
    }
}