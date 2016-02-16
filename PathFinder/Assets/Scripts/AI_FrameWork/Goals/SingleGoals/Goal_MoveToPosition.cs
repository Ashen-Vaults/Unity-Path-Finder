using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 
/// </summary>
public class Goal_MoveToPosition : Goal
{

    public Goal_MoveToProps moveProps;

    /// <summary>
    /// 
    /// </summary>
    //public Goal_MoveToPosition(AI owner) : base(owner,1){ }

    /// <summary>
    /// 
    /// </summary>
    public override void Activate()
    {
        //this.ClearSubGoals();
        this.myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        this.myProperties.myOwner.myProperties.myMovement.ArriveOn();

        if (this.myProperties.myOwner.myProperties.myMovement.myProperties.myAgent.remainingDistance > this.myProperties.myOwner.myProperties.myMovement.myProperties.myArrive.arriveDistance)
        {
            // this.myProperties.myStatus = GoalProps.goalStatus.COMPLETED;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public override GoalProps.goalStatus Process()
    {
        this.ActivateIfInactive();
        //this.myProperties.myStatus;
        this.ReactivateIfFailed();


        return this.myProperties.myStatus;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Terminate()
    {
        //Debug.LogError("ENDING Arrive " + this.name);
        this.myProperties.myOwner.myProperties.myMovement.ArriveOff();
        this.myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
    }

    public override float CalculateUtility()
    {
        float utility = 1f;

        if (this.myProperties.myOwner.myProperties.myTargetting.TargetInFOV())
        {
            utility += this.myProperties.myOwner.myProperties.myBrain.myPersonality.attackBias;
        }

        return utility;
    }
}

[Serializable]
public class Goal_MoveToProps
{
    public Vector3 ai_Pos;
    public float expectedTime;
    public float startTime;
    public float timeTaken;
    protected bool isStuck()
    {
        timeTaken = Time.time - startTime;
        if (timeTaken > expectedTime) return true;
        return false;
    }

}


