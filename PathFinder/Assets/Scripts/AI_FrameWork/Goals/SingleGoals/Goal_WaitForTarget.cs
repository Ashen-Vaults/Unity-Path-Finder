using UnityEngine;
using System.Collections;

public class Goal_WaitForTarget : Goal
{
    public override void Activate()
    {
        //Debug.LogError("STARTING  FOLLOW");
        this.myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        this.myProperties.myOwner.myProperties.myMovement.myProperties.myAgent.Stop();
    }

    public override void Terminate()
    {
        //Debug.LogError("ENDING FOllow" + this.name);
        this.myProperties.myOwner.myProperties.myMovement.myProperties.myAgent.Resume();
        this.myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
    }

    public override GoalProps.goalStatus Process()
    {
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