using UnityEngine;
using System.Collections;
using System;

public class Goal_FollowPath : Goal
{
    public Goal_FollowPath(AI owner) : base(owner, 1) { }

    public override void Activate()
    {
        //Debug.LogError("STARTING  FOLLOW");
        this.myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        this.myProperties.myOwner.myProperties.myMovement.FollowPathOn();
    }

    public override void Terminate()
    {
        //Debug.LogError("ENDING FOllow" + this.name);
        this.myProperties.myOwner.myProperties.myMovement.FollowPathOff();
        this.myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
    }

    public override GoalProps.goalStatus Process()
    {
        //     Debug.LogError("Process " + this.name);
        //if status is inactive, call Activate()
        ActivateIfInactive();
        return this.myProperties.myStatus;
    }

    public override float CalculateUtility()
    {
        float utility = 1f;

        if (!this.myProperties.myOwner.myProperties.myTargetting.TargetInFOV())
        {
            utility += this.myProperties.myOwner.myProperties.myBrain.myPersonality.defendBias;
        }

        return utility;
    }
}
