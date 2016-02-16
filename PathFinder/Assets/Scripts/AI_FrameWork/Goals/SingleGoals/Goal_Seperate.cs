using UnityEngine;
using System.Collections;
using System;

public class Goal_Seperate : Goal {

    public Goal_Seperate(AI owner) : base(owner,1) { }

    public override void Activate()
    {
        this.myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        this.myProperties.myOwner.myProperties.myMovement.SeparationOn();
       
    }

    public override void Terminate()
    {
        this.myProperties.myOwner.myProperties.myMovement.SeparationOff();
        //   this.myProperties.myOwner.myProperties.myMovement.SeekOn();
    }

    public override GoalProps.goalStatus Process()
    {
        //if status is inactive, call Activate()
        ActivateIfInactive();
        return this.myProperties.myStatus;
    }

    public override float CalculateUtility()
    {
        float utility = 0f;

        if (this.myProperties.myOwner.myProperties.myTargetting.TargetInFOV())
        {
            utility *= this.myProperties.myOwner.myProperties.myBrain.myPersonality.attackBias;
        }

        return utility;
    }
}
