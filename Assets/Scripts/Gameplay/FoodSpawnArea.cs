using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class FoodSpawnArea : MonoBehaviour
    {
        [SerializeField] BoxCollider collider;

        List<Food> foods = new List<Food>();
        [SerializeField]private AiAgent_BT aiAgent;

        private void OnValidate()
        {
            if(collider == null) collider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            UnityEngine.Debug.Log($" Name: {gameObject.name}, extends --------- {collider.bounds.extents.x}, {collider.bounds.extents.y}, {collider.bounds.extents.z}");
        }

        public List<(float,float)> GetRandomPoints(int count)
        {
            List<(float,float)> points = new List<(float,float)>();
            for (int i = 0; i < count; i++)
            {
                var xPos = UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x);
                var zPos = UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z);
                // Debug.Log($"[GetRandomPoints] xpos : {xPos}, ypos : {zPos}");
                points.Add((xPos, zPos));
            }
            
            return points;
        }

        public bool Contains(AiAgent_BT agent)
        {
            // Debug.Log($"aiAgent == null : {aiAgent == null}, agent name : {aiAgent.name}");
            if(aiAgent == null) return false;
            return aiAgent == agent;
        }

        public void SpawnFood(int count, Food food)
        {
            var points = GetRandomPoints(count);
            foreach ((float, float) point in points)
            {
                var c = Instantiate(food);
                c.name = $"Food {point}";
                c.transform.position = new Vector3(point.Item1, c.transform.position.y, point.Item2);
                foods.Add(c);
            }
        }

        public Food GetClosestFood(Vector3 position)
        {
            var closestDistance = float.MaxValue; 
            Food closestFood = null;    
            foreach (var food in foods)
            {
                var dist = Vector3.Distance(position, food.transform.position);
                if (!(dist < closestDistance)) continue;
                closestDistance = dist; 
                closestFood = food;
            }
            return closestFood;
        }

        public void OnFoodConsumed(Food food)
        {
            Food foodToConsume = null;
            foreach (var f in foods.ToList().Where(f => f == food))
            {
                foodToConsume = f;
            }
            if(foodToConsume == null) return;
            foodToConsume.Consumed();
            foods.Remove(foodToConsume);
        }

        public bool IsFoodAvailable()
        {
            return foods.Count > 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AiAgent"))
            {
                aiAgent = other.GetComponent<AiAgent_BT>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("AiAgent"))
            {
                aiAgent = null;
            }
        }

        public Vector3 GetClosestPointInArea(Vector3 position)
        {
            return collider.ClosestPoint(position);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public bool IsInBounds(Vector3 position)
        {
            return collider.bounds.Contains(position);
        }
    }
}