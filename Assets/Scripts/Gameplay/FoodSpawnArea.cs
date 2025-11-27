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
        private void OnValidate()
        {
            if(collider == null) collider = GetComponent<BoxCollider>();
        }

        public List<(float,float)> GetRandomPoints(int count)
        {
            List<(float,float)> points = new List<(float,float)>();
            for (int i = 0; i < count; i++)
            {
                var xPos = UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x);
                var zPos = UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z);
                Debug.Log($"[GetRandomPoints] xpos : {xPos}, ypos : {zPos}");
                points.Add((xPos, zPos));
            }
            
            return points;
        }

        public bool Contains(Vector3 position)
        {
            return collider.bounds.Contains(position);
        }

        public void SpawnFood(int count, Food food)
        {
            for (int i = 0; i < count; i++)
            {
                var points = GetRandomPoints(5);
                foreach ((float, float) point in points)
                {
                    var c = Instantiate(food);     
                    c.transform.position = new Vector3(point.Item1, c.transform.position.y, point.Item2);
                    foods.Add(c);
                }
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
            foreach (var f in foods.ToList().Where(f => f == food))
            {
                f.Consumed();
                foods.Remove(f);
            }
        }

        public bool IsFoodAvailable()
        {
            return foods.Count > 0;
        }
    }
}