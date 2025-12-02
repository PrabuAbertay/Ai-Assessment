using UnityEngine;

namespace Gameplay
{
    public class Food : MonoBehaviour
    {
        public void Consumed()
        {
            UnityEngine.Debug.Log("Food Consumed");
            gameObject.SetActive(false);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}