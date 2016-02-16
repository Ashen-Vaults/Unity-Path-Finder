using UnityEngine;
using System.Collections;

public abstract class GoalEvaluator
{
    public GoalEvaluator(float bias)
    {

    }

    /**
 * when the desirability score for a goal has been evaluated it is
 * multiplied by this value. It can be used to create bots with preferences
 * based upon their personality
 */



    /**
     * returns a score between 0 and 1 representing the desirability of the
     * strategy the concrete subclass represents
     */
    public abstract float CalculateDesirability(AI _ai);

    /**
     * adds the appropriate goal to the given bot's brain
     */
    public abstract void SetGoal(AI _ai);

    /**
     * used to provide debugging/tweaking support
     */
    public abstract void RenderInfo(Vector3 _pos, AI _ai);


}
