using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class FoodSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("foodSpawnArea")] [SerializeField] private List<FoodSpawnArea> foodSpawnAreas;
        [SerializeField] Food foodPrefab;
        private int maxFoodCountByArea = 5;

        private void Start()
        {
            foreach (var spawnArea in foodSpawnAreas)
            {
                spawnArea.SpawnFood(maxFoodCountByArea,foodPrefab);
            }
        }

        public bool IsInFoodAvailableArea(Vector3 position, out FoodSpawnArea foodSpawnArea)
        {
            foreach (var spawnArea in foodSpawnAreas.Where(spawnArea => spawnArea.Contains(position)))
            {
                foodSpawnArea = spawnArea;
                return true;
            }
            foodSpawnArea = null;
            return false;
        }

        public FoodSpawnArea GetClosestFoodSpawnArea(Vector3 position)
        {
            float closestDistance = float.MaxValue; 
            FoodSpawnArea closestArea = null;   
            foreach (var foodSpawnArea in foodSpawnAreas)
            {
                var distance = Vector3.Distance(position, foodSpawnArea.transform.position);
                if (!(distance < closestDistance) || !foodSpawnArea.IsFoodAvailable()) continue;
                closestDistance = distance; 
                closestArea = foodSpawnArea;
            }

            return closestArea;
        }

        public Food GetClosestFood(Vector3 position, FoodSpawnArea foodSpawnArea)
        {
            return foodSpawnArea.GetClosestFood(position);
        }

        public void OnFoodConsumed(Food food, FoodSpawnArea foodSpawnArea)
        {
            foodSpawnArea.OnFoodConsumed(food);
        }
    }
}