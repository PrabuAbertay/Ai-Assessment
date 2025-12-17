using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        [SerializeField] Canvas homeScreen;
        [SerializeField] private Button btBtn;
        [SerializeField] private Button goapBtn;
        [SerializeField] private Button startBtn;
        [SerializeField] private TMP_Text healthTxt;
        [SerializeField] private TMP_Text staminaTxt;

        private void Awake()
        {
            startBtn.interactable = false;
        }

        private void OnEnable()
        {
            btBtn.onClick.AddListener(BtCall);   
            goapBtn.onClick.AddListener(GoapCall);   
            startBtn.onClick.AddListener(StartCall);   
        }
        private void OnDisable()
        {
            btBtn.onClick.RemoveAllListeners();
            goapBtn.onClick.RemoveAllListeners();
            startBtn.onClick.RemoveAllListeners();
        }
        private void StartCall()
        {
            startBtn.interactable = false;
            btBtn.interactable = false;
            goapBtn.interactable = false;
            gameManager.StartGame();
            homeScreen.gameObject.SetActive(false);
        }
        private void GoapCall()
        {
            startBtn.interactable = true;
            btBtn.interactable = false;
            goapBtn.interactable = false;
            gameManager.SetAgentType(AgentType.GoapAgent);
        }


        private void BtCall()
        {
            startBtn.interactable = true;
            btBtn.interactable = false;
            goapBtn.interactable = false;
            gameManager.SetAgentType(AgentType.BtAgent);
        }

        public void UpdateHealthText(int health)
        {
            healthTxt.text = "Health: " + health.ToString();
        }
        public void UpdateStaminaText(int stamina)
        {
            staminaTxt.text = "Stamina: " + stamina.ToString();
        }
    }
}