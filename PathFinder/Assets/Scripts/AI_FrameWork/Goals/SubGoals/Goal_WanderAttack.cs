using UnityEngine;
using System.Collections;
using System;

public class Goal_WanderAttack : GoalList
{

    public override void Activate()
    {

        // this.defendProps.resumeFollow = new UpdateUtility(this.defendProps.resumeFollowTime);

        myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
      //  print("ACTIVATE WANDER");
        AddSubGoal(this.GetComponent<Goal_Wander>());



    }


    public override GoalProps.goalStatus Process()
    {
        this.ActivateIfInactive();
        myProperties.myStatus = this.ProcessSubGoals();

        if (this.myProperties.myOwner.myProperties.myTargetting.myCurrentTarget != null)
        {
          //  print("FOLLOW");
            AddSubGoal(this.GetComponent<Goal_MoveToPosition>());   //Follow target
        }

        else
        {
            //Is the AI currently seeking but has no target?
            if (this.myProperties.myOwner.IsLost())
            {
                print("LOST");
                this.Activate();
            }
        }

        ReactivateIfFailed();
        return myProperties.myStatus;
    }

    public override void Terminate()
    {
        this.ClearSubGoals();
        myProperties.myStatus = GoalProps.goalStatus.COMPLETED;
    }


    public override float CalculateUtility()
    {
        this.myProperties.myUtility = 0f;

        this.myProperties.myUtility += this.myProperties.myOwner.myProperties.myBrain.myPersonality.defendBias;


        return this.myProperties.myUtility;
    }
}
