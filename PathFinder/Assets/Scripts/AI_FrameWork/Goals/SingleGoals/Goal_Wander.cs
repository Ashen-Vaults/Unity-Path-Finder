using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is the Brain of the AI. It will manage all the decisions about
/// which goals it persues and the order it does them in. It will be able
/// to put all of the goals that it aggregates in a queue and pop them out 
/// and checks the utility of each of them until it gets to a high rated one, then
/// pursues that.
/// </summary>
public class Goal_Wander : Goal
{

    public Goal_Wander(AI owner) : base(owner, 1) { }

    public override void Activate()
    {
        //print("WANDERRING");
        this.myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        this.myProperties.myOwner.myProperties.myMovement.WanderOn();
    }

    public override void Terminate()
    {
        //Debug.Log("ENDING Wander " + this.name);
        this.myProperties.myOwner.myProperties.myMovement.WanderOff();
        this.myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
    }

    public override GoalProps.goalStatus Process()
    {
        //if status is inactive, call Activate()
        this.ActivateIfInactive();
        this.ReactivateIfFailed();
        return this.myProperties.myStatus;
    }

    public override float CalculateUtility()
    {
        float utility = 1f;

        if (!this.myProperties.myOwner.myProperties.myTargetting.TargetInFOV())
        {
            utility *= this.myProperties.myOwner.myProperties.myBrain.myPersonality.exploreBias;
        }

        return utility;
    }
}
