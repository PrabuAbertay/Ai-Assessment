using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] AiAgent_BT agent;
        [SerializeField] FoodSpawner foodSpawner;

        private void Awake()
        {
            agent.Init(foodSpawner);
        }
    }
}