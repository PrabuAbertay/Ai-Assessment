using System;
using System.Data;
using Gameplay;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiAgent_BT : MonoBehaviour
{
    FoodSpawner foodSpawner;
    FoodSpawnArea currentFoodSpawnArea;
    private Food closestFood;
    private Transform currentMoveToTarget;
    
    [SerializeField] NavMeshAgent navMeshAgent;

    public Transform CurrentMoveToTarget
    {
        set => currentMoveToTarget = value;
    }
    
    public FoodSpawnArea CurrentFoodSpawnArea => currentFoodSpawnArea;
    public Food ClosestFood => closestFood;

    private void OnValidate()
    {
        if(navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(FoodSpawner foodSpawner)
    {
        this.foodSpawner = foodSpawner;
    }

    public bool IsInFoodAvailableArea()
    {
        return (foodSpawner.IsInFoodAvailableArea(transform.position, out currentFoodSpawnArea));
    }

    public void SetCurrentFoodSpawnArea(FoodSpawnArea newArea)
    {
        currentFoodSpawnArea = newArea;
    }

    public bool IsFoodAvailableInArea()
    {
        return currentFoodSpawnArea.IsFoodAvailable();
    }

    public void SetClosestFood(Food food)
    {
        closestFood = food;
    }
    

    public void GetClosestFood()
    {
        if (currentFoodSpawnArea != null)
        {
            closestFood = currentFoodSpawnArea.GetClosestFood(transform.position);
            return;
        }

        Debug.LogError("[AiAgent_BT] currentFoodSpawnArea == null");
    }

    public void MoveToTarget()
    {
        if (currentMoveToTarget == null)
        {
            Debug.LogError("[AiAgent_BT] currentMoveToTarget == null");
            return;
        }
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(currentMoveToTarget.position);
        navMeshAgent.isStopped = true;
    }

    public bool IsMovingToTarget()
    {
        return navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
    }

    public void StopMovingToTarget()
    {
        navMeshAgent.isStopped = true;
        currentMoveToTarget = null; 
    }

    public void ConsumeFood()
    {
        if (closestFood == null || currentFoodSpawnArea == null)
        {
            Debug.LogError($"[AiAgent_BT] closestFood == null : {closestFood == null} , currentFoodSpawnArea == null : {currentFoodSpawnArea == null}");
            return;
        }
        foodSpawner.OnFoodConsumed(closestFood, currentFoodSpawnArea);
    }

    public void GetClosestFoodArea()
    {
        currentFoodSpawnArea = foodSpawner.GetClosestFoodSpawnArea(transform.position);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}