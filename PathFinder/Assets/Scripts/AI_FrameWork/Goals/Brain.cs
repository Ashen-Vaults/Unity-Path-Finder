using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 
/// </summary>
public class Brain : GoalList
{
    public Goal myGoal;
    public GoalEvaluator evaluator;
    public Personality myPersonality;

    public bool activateGoal;

    public override void Activate()
    {
        if (subGoals.Count > 0)
        {
            this.SelectGoal();
            myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
            //       this.myGoal.Activate();
        }
    }

    /// <summary>
    /// Goes thru the subgoals and process' them.
    /// </summary>
    /// <returns>The status of the goal we are processing.</returns>
    public override GoalProps.goalStatus Process()
    {
        ActivateIfInactive();
        GoalProps.goalStatus SubgoalStatus = ProcessSubGoals();

        if (SubgoalStatus == GoalProps.goalStatus.COMPLETED || SubgoalStatus == GoalProps.goalStatus.FAILED)
        {
            myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
        }

        return myProperties.myStatus;
    }

    public override void Terminate()
    {
        this.ClearSubGoals();
    }

    /// <summary>
    /// Gets the usefulness of each subgoal that the Brain
    /// currently has and chooses the one with the greatest
    /// utility. The utility of a goal is calculated periodically
    /// by the Brains update in AI. 
    /// </summary>
    public void SelectGoal()
    {
        float best = 0f;
        Goal topChoice = null;

        for (int i = 0; i <= this.subGoals.Count - 1; i++)
        {
            //print("GOALS " + i + " " + subGoals[i]);
            float utility = subGoals[i].CalculateUtility();

            if (utility >= best && utility > 0)
            {
                best = utility;
                topChoice = subGoals[i];
                if (!topChoice.Equals(this.myGoal) && this.myGoal != null)
                {
                    this.myGoal.myProperties.myStatus = GoalProps.goalStatus.FAILED;
                    //  this.myGoal.Terminate();
                }
            }
        }
        this.myGoal = topChoice;
        this.AddSubGoal(this.myGoal);
        //this.myGoal.Activate();
        /*
      if (this.myGoal != null)
      {


          if (this.myGoal != topChoice)
          {
              this.myGoal.Terminate();
              this.myGoal = topChoice;
              this.myGoal.Activate();
          }

      }
      else
      {
          if (subGoals.Count > 0)
          {
              this.myGoal = topChoice;
              this.myGoal.Activate();
          }
          else
          {
              this.subGoals.Add(this.GetComponent<Goal_Wander>());   
          }



      }
          */
    }


    public override float CalculateUtility()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class Personality
{
   // [Tooltip("The desirability to attack their target. The higher their attackBias, the less they care about other goals. They just want to KILL")]
    public float attackBias;

   // [Tooltip("The desirability to explore the world. The higher their exploreBias, the less they care about other goals.")]
    public float exploreBias;

   // [Tooltip("The desirability to defend their target. The higher their defendBias, the less they care about other goals.")]
    public float defendBias;
}
