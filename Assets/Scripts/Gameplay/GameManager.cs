using System;
using GOAP;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public enum AgentType
    {
        BtAgent,
        GoapAgent,
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] AiAgent_BT btAgent;
        [SerializeField] private Transform btHideout;
        [SerializeField] GoapAgent goapAgent;
        [SerializeField] private Transform goapHideout;
        [SerializeField] FoodSpawner foodSpawner;

        public AgentType agentType { get; set; } = AgentType.GoapAgent;

        private void Awake()
        {
            btAgent.gameObject.SetActive(false);
            btHideout.gameObject.SetActive(false);
            goapAgent.gameObject.SetActive(false);
            goapHideout.gameObject.SetActive(false);
            btAgent.Init(foodSpawner);
        }

        public void StartGame()
        {
            switch (agentType)
            {
                case AgentType.BtAgent:
                    btAgent.gameObject.SetActive(true);
                    btHideout.gameObject.SetActive(true);
                    break;
                case AgentType.GoapAgent:
                    goapAgent.gameObject.SetActive(true);
                    goapHideout.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetAgentType(AgentType agentType)
        {
            this.agentType = agentType; 
        }
    }
}