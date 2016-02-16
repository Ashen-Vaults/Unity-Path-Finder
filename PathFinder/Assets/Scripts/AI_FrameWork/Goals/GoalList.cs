using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GoalList contains several high-level goals
/// that are nested together. They are composed of two or more 
/// atomic goals, which are goals that have 1 general task. IE: Goal_SeekPosition, or Goal_ChaseTarget
/// 
/// Composed Instance: Goal_RetreatAndCover
///     -> Goal_SeekPosition (Finds a suitible place to move)
///     -> Goal_MoveToTarget (Moves to said place)
///     -> Goal_Cover        (Ducks for cover)
/// 
/// Each time an AI "thinks" it is:
///      1) examining the Game State,
///      2) Selecting the "best" goal that it has access to
///      3) Attempt to accomplish said goal
///         i) It breaks down an composed goals into its sub-goals
///        ii) Attempts to solve each sub-goal
///      4) Repeat until the status of the goal is completed or failed
///
/// Every time the decision-making BRAIN is updated, each 
/// goal will be evaluated for its suitability for the given task by 
/// the current state of the AI.
/// 
/// The one with the highest utility, "SCORE", is made the current Goal.
/// </summary>
public abstract class GoalList : Goal {

    /// <summary>
    /// A list of the subgoals in this GoalList
    /// </summary>
    public List<Goal> subGoals;

    public GoalList() { }
    
    /// <summary>
    /// This will go through the subgoals and will remove
    /// the goals which are completed at the end of the list
    /// and will then process the next goal
    /// </summary>
    /// <returns>The status of the current goal</returns>
    protected GoalProps.goalStatus ProcessSubGoals()
    {
        //Removes completed and failed goals from the front
        while((subGoals.Count > 0) && (subGoals[0].isCompleted() || subGoals[0].hasFailed()))
        {
            subGoals[0].Terminate();
            subGoals.RemoveAt(0);
        }

        //Process first goal at list
        if (subGoals.Count > 0)
        {
           // print(subGoals[0]);
            GoalProps.goalStatus status = subGoals[0].Process();

            if(status == GoalProps.goalStatus.COMPLETED && subGoals.Count-1 > 1)
            {
                return GoalProps.goalStatus.ACTIVE;
            }

            return status;
        }
        else //no more goals, return completed
        {
            return GoalProps.goalStatus.COMPLETED;
        }

    }

    public bool HasGoal(Goal _g)
    {
        if (this.subGoals.Count > 0)
        {
            return subGoals[0] != _g;
        }
        return true;
    }

    /// <summary>
    /// Adds a new goal at the head of the list
    /// </summary>
    /// <param name="_g">The goal to add.</param>
    public override void AddSubGoal(Goal _g)
    {
        if (HasGoal(_g) == true)
        {
            ClearSubGoals();
            subGoals.Insert(0, _g);
        }
        else
        {
//add component
        }
        
    }

    /// <summary>
    /// Calls the terminate in each goal
    /// then removes it from subgoals
    /// </summary>
    public void ClearSubGoals()
    {
        for(int i = 0; i <= subGoals.Count-1; i++)
        {
            subGoals[i].Terminate();
        }
        subGoals.Clear();
    }


}
