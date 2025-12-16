using UnityEngine;

namespace Gameplay
{
    public class Food : MonoBehaviour
    {
        public int HealthValue { get; } = 60;
        public bool consumed { get; set; } = false;

        public void Consumed()
        {
            UnityEngine.Debug.Log("Food Consumed");
            gameObject.SetActive(false);
            consumed = true;    
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}