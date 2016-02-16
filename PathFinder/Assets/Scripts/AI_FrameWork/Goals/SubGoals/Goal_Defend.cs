using UnityEngine;
using System.Collections;
using System;

public class Goal_Defend : GoalList
{

    public DefendProps defendProps;
    public override void Activate()
    {
        myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        if (this.myProperties.myOwner.myProperties.myMovement.myProperties.myFollowPath.wayPoints.Length >= 2)
        {
            AddSubGoal(this.GetComponent<Goal_FollowPath>());
        }
        else
        {
            AddSubGoal(this.GetComponent<Goal_WaitForTarget>());
            //print("I have no waypoints");
            //this.AddSubGoal(this.GetComponent<Goal_Wander>()); //have AI wander around
        }
    }



    /// <summary>
    /// This re-activates this Goal after X seconds. In the activate it 
    /// re-adds in the FollowPath goal
    /// </summary>
    /// <returns></returns>
    IEnumerator ResumeFollow()
    {
        yield return new WaitForSeconds(this.defendProps.resumeFollowTime);
        //print("RESUME FOLLOW");
        this.Activate();
    }

    public override GoalProps.goalStatus Process()
    {
        this.ActivateIfInactive();
        myProperties.myStatus = this.ProcessSubGoals();

        if (this.myProperties.myOwner.myProperties.myTargetting.myCurrentTarget != null)
        {
            AddSubGoal(this.GetComponent<Goal_MoveToPosition>());   //Follow target
        }

        else
        {
            //Is the AI currently seeking but has no target?
            if (this.myProperties.myOwner.IsLost())
            {
                this.AddSubGoal(this.GetComponent<Goal_Wander>()); //have AI wander around
                // print("IM LOST");
                StartCoroutine(ResumeFollow());   //If no new target is seen, followpath again
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

[Serializable]
public class DefendProps
{
    public UpdateUtility resumeFollow;
    public float resumeFollowTime;
}
