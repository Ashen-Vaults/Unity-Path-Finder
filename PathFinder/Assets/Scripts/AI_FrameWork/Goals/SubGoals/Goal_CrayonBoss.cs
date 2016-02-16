using UnityEngine;
using System.Collections;

public class Goal_CrayonBoss : GoalList
{
    public override void Activate()
    {
        myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
        AddSubGoal(this.GetComponent<Goal_SpawnMinion>());
        this.myProperties.myOwner.myProperties.invinsible = true;
        this.myProperties.myOwner.myProperties.myMovement.WanderOn();
    }



    /// <summary>
    /// This re-activates this Goal after X seconds. In the activate it 
    /// re-adds in the FollowPath goal
    /// </summary>
    /// <returns></returns>
    IEnumerator ResumeFollow()
    {
        yield return new WaitForSeconds(5f);
        //print("RESUME FOLLOW");
        this.Activate();
    }

    public override GoalProps.goalStatus Process()
    {
        this.ActivateIfInactive();
        myProperties.myStatus = this.ProcessSubGoals();

   

        if (!this.GetComponent<Goal_SpawnMinion>().MinionsLeft())
        {
            this.Activate();
        }
        else
        {
          
        }

        if (this.GetComponent<Goal_SpawnMinion>().myProperties.myStatus == GoalProps.goalStatus.ACTIVE)
        {
            //If the player is nearby, then the boss stops moving and goes
            //into defense mode
            if (this.myProperties.myOwner.myProperties.myTargetting.myCurrentTarget != null)
            {
                this.myProperties.myOwner.myProperties.myMovement.myProperties.myAgent.Stop();
            }
            else
            {
                this.myProperties.myOwner.myProperties.myMovement.myProperties.myAgent.Resume();
            }
        }else
            if (this.GetComponent<Goal_SpawnMinion>().myProperties.myStatus == GoalProps.goalStatus.COMPLETED)
            {
                print("Phase 2");
                this.AddSubGoal(this.GetComponent<Goal_WaitForTarget>());
                //this.myProperties.myOwner.myProperties.myMovement.myProperties.myAgent.Stop();
                this.myProperties.myOwner.myProperties.invinsible = false;
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
