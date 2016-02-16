using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The AI utilizes a hierarchical set of Goals for 
/// accomplishing whatever that AI was supposed to do.
/// Goals can either be a single instance, or can be composed
/// of several other sub-goals. 
/// 
/// Single Instance  : Goal_Wander (Just randomly moves around the map)
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
/// </summary>
public abstract class Goal : MonoBehaviour {

    //The properties which define the outline for a goal
    public GoalProps myProperties;

    void Awake()
    {
        myProperties.myOwner = this.transform.parent.parent.GetComponent<AI>();
    }

    /// <summary>
    /// Empty Constructor 
    /// </summary>
    public Goal() { }


    /// <summary>
    /// This constructor takes in the Living thing
    /// that is currently trying to satisfy this goal
    /// and the type of goal tht it is
    /// </summary>
    /// <param name="_AI"></param>
    /// <param name="_type"></param>
    public Goal(AI _AI, int _type)
    {
        myProperties.myOwner = _AI;
     //   myProperties.myType = _type;
        myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
    }


    /// <summary>
    /// 
    /// </summary>
    protected void ActivateIfInactive()
    {
        if (isInactive()) Activate();
        //Debug.LogError("ACTIVATE IN ACTIVE");
    }

    /// <summary>
    /// 
    /// </summary>
    protected void ReactivateIfFailed()
    {
        if (hasFailed()) myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
        //Debug.LogError("RECTIVATE IN FAIL");
    }

    /// <summary>
    /// Initializes the goal with the logic
    /// specific to the type of goal, What the goal is doing.
    /// ex(Goal_Wandering) might contain logic for checking to see
    /// how long the AI wandered.
    /// 
    /// It plans out how it will accomplish that goal based off 
    /// the logic. 
    /// What is nice about this is we can call activate
    /// as many times as we want, therefore we can have an AI
    /// replan its current routine, while mid routine.
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Tidy ups the goal before the goal
    /// is "destroyed" after the goal is
    /// exited
    /// </summary>
    public abstract void Terminate();

    /// <summary>
    /// Executed often enough to check the status
    /// of the goal.
    /// 
    /// Returns one of the const in GoalProps
    /// </summary>
    /// <returns>Int value indicating the status of the goal</returns>
    public abstract GoalProps.goalStatus Process();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool HandleMessage()
    {
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_goal"></param>
    public virtual void AddSubGoal(Goal _goal)
    {

    }

    /// <summary>
    /// Wrapper methods for changing the status of the goal
    /// </summary>
    /// <returns></returns>
#region Utility functions for setting the status of the Goal   
    public bool isActive()
    {
        return myProperties.myStatus == GoalProps.goalStatus.ACTIVE;
    }

    public bool isInactive()
    {
        return myProperties.myStatus == GoalProps.goalStatus.INACTIVE;
    }

    public bool isCompleted()
    {
        return myProperties.myStatus == GoalProps.goalStatus.COMPLETED;
    }

    public bool hasFailed()
    {
        return myProperties.myStatus == GoalProps.goalStatus.FAILED;
    }
    #endregion

    public abstract float CalculateUtility();
}


/// <summary>
/// The Properties for a goal. 
/// </summary>
[Serializable]
public class GoalProps
{
  //  [HideInInspector]
    public AI myOwner; //The person living thing using the goal
   // public int myStatus;   //
   // public int myType;

    public float myUtility;

    public enum goalStatus { INACTIVE=0, ACTIVE=1, COMPLETED=2, FAILED=3 };
    public goalStatus myStatus;
    /// <summary>
    /// active    : The goal has been activated and will be processed in the coming update steps
    /// inactive  : The goal is waiting to be activated
    /// completed : The goal has completed and will be removed on the next update
    /// failed    : The goal has failed and will either readjust or be removed next time it's updated 
    /// </summary>



}