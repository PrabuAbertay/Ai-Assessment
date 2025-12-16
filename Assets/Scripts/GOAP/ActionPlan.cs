using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public interface IGoapPlanner
    {
        ActionPlan Plan(GoapAgent agent, HashSet<Goal> goals, Goal mostRecentGoal = null);
    }

    public class GoapPlanner : IGoapPlanner
    {
        public ActionPlan Plan(GoapAgent agent, HashSet<Goal> goals, Goal mostRecentGoal = null)
        {
            // var orderedGoals = goals.Where(g => g.desiredEffects.Any(g => !g.Evaluate())).ToList();
            
            var orderedGoals = goals.Where(g => g.desiredEffects.Any(g => !g.Evaluate()))
            .OrderByDescending(g => g == mostRecentGoal ? g.priority -= 0.01f : g.priority).ToList();
            
            Debug.Log($"orderedGoals count: {orderedGoals.Count}, goals count: {goals.Count}");
            foreach (var goal in orderedGoals)
            {
                var goalNode = new GoapNode(null, null, goal.desiredEffects, 0);

                if (FindPath(goalNode, agent.actions))
                {
                    // Debug.Log($"goal node is leaf: {goalNode.IsLeaf}");
                    if(goalNode.IsLeaf) continue;

                    // Debug.Log($"goal node child count: {goalNode.children.Count}");
                    var actionStack = new Stack<GoapAction>();
                    while (goalNode.children.Count > 0)
                    {
                        var cheapestNode = goalNode.children.OrderBy(n => n.cost).First();
                        goalNode = cheapestNode;
                        actionStack.Push(cheapestNode.action);
                    }
                    
                    return new ActionPlan(goal, actionStack,goalNode.cost);   
                }
            }
            
            UnityEngine.Debug.LogError("Cant find plan--");
            return null;    
        }

        bool FindPath(GoapNode parent, HashSet<GoapAction> actions)
        {
            var orderedActions = actions.OrderBy(a => a.cost);
            // Debug.Log($"orderedActions count: {orderedActions.Count()}");
            foreach (var action in orderedActions)
            {
                var requiredEffects = parent.requiredEffects;
                // Debug.Log($"requiredEffects count: {requiredEffects.Count()}");

                requiredEffects.RemoveWhere(e => e.Evaluate());
                // Debug.Log($"requiredEffects count after remove: {requiredEffects.Count()}");
                
                if(requiredEffects.Count == 0) return true;

                if (action.effects.Any(requiredEffects.Contains))
                {
                    var newRequiredEffects = new HashSet<AgentBelief>(requiredEffects); 
                    newRequiredEffects.ExceptWith(action.effects);
                    newRequiredEffects.UnionWith(action.preConditions);
                    // Debug.Log($"newRequiredEffects count: {newRequiredEffects.Count()}");

                    var newAvailableAction = new HashSet<GoapAction>(actions);
                    newAvailableAction.Remove(action); // remove used action from a plan
                    // Debug.Log($"newAvailableAction count: {newAvailableAction.Count()}");
                    
                    var newNode = new GoapNode(parent, action, newRequiredEffects, parent.cost + action.cost);

                    if (FindPath(newNode, newAvailableAction)) 
                    {
                        parent.children.Add(newNode);
                        newRequiredEffects.ExceptWith(newNode.action.preConditions);
                    }
                    
                    // Debug.Log($"newRequiredEffects count after recurse: {newAvailableAction.Count()}");
                    if(newRequiredEffects.Count == 0) return true;
                }
            }
            return parent.children.Count > 0;
        }
    }

    public class GoapNode
    {
        public GoapNode parent { get; }
        public GoapAction action { get; }
        public HashSet<AgentBelief> requiredEffects { get; }
        public List<GoapNode> children { get; }
        public int cost { get; }    
        
        public bool IsLeaf => children.Count == 0 && action == null;  
        
        public GoapNode(GoapNode parent,GoapAction action, HashSet<AgentBelief> beliefs,  int cost)
        {
            this.parent = parent;
            this.action = action;
            this.requiredEffects = new HashSet<AgentBelief>(beliefs);
            this.cost = cost;
            children = new List<GoapNode>();
        }
    }
    public class ActionPlan
    {
        public Goal Goal { get; }
        public Stack<GoapAction> actions{ get; }
        private int totalCost{ get; }

        public ActionPlan(Goal goal, Stack<GoapAction> actions, int totalCost)
        {
            this.Goal = goal;
            this.actions = actions; 
            this.totalCost = totalCost; 
        }
    }
}