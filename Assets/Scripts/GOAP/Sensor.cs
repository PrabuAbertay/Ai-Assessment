using UnityEngine;

namespace GOAP
{
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour
    {
        [SerializeField] float detectionRadius;
        [SerializeField] float timeInterval;
        float timeCounter;  
        
        SphereCollider sensorCollider;
        
        GameObject target;
        Vector3 lastKnownPosition;  
        
        public Vector3 TargetPosition()=> target?.transform.position ?? Vector3.zero; 
        public bool IsTargetInRange() => TargetPosition()!=Vector3.zero;

        private void OnValidate()
        {
            if(sensorCollider == null) sensorCollider = GetComponent<SphereCollider>(); 
        }

        private void Awake()
        {
            sensorCollider.radius = detectionRadius;    
            sensorCollider.isTrigger = true;        
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange() ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        private void Start()
        {
            timeCounter = Time.time;
        }

        private void Update()
        {
            if (!(Time.time - timeCounter >= timeInterval)) return;
            timeCounter = Time.time;
            UpdateTargetPosition(target);
        }

        void UpdateTargetPosition(GameObject target = null)
        {
            if(target == null) return;
            this.target = target;
            if (IsTargetInRange() &&
                (TargetPosition() != lastKnownPosition || lastKnownPosition != Vector3.zero))
            {
                lastKnownPosition = target.transform.position;      
                UpdateTargetPosition();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("AiAgent"))return;
            UpdateTargetPosition(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("AiAgent"))return;
            UpdateTargetPosition();
            
        }
    }
}