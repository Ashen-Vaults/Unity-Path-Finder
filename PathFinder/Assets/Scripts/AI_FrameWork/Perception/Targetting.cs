using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Targetting class is used by the AI to determine who
/// their target is. This class uses the AI's perception
/// by gaining access to the Dictionary of Targets and MemoryProperties
/// called myMemory. 
/// </summary>

[RequireComponent(typeof(Perception))]
public class Targetting : MonoBehaviour
{

   
    public AI myOwner;
    public Transform myTransform;
    public bool showTarget, testAI;
    public int targetCap;
    public GameObject myCurrentTarget;
    public GameObject MyCurrentTarget
    {
        get { return myCurrentTarget; }
        set { myCurrentTarget = value; }
    }

    void Awake()
    {
      //  this.myTransform = this.transform.parent.parent.GetComponent<Living>().transform;
        if (this.myTransform == null) Debug.LogError("Trargetting needs the AI parent transform");
        this.myOwner = this.myTransform.GetComponent<AI>();
    }

    private void OnDrawGizmosSelected()
    {
        if (showTarget)
        {

            if (this.myCurrentTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(this.myCurrentTarget.transform.position, new Vector3(1, 1, 1));

                Gizmos.color = Color.black;
                Gizmos.DrawLine(this.myCurrentTarget.transform.position, this.myTransform.position);

                Gizmos.color = Color.blue;
                Gizmos.DrawCube(this.myTransform.position, new Vector3(1, 1, 1));
            }

        }

    }

    public List<GameObject> percievedTargets;
    public List<GameObject> attackTargets;
    /// <summary>
    /// We check through the AI's Memory and get a list
    /// of all the targets that were in it. The one 
    /// who is closest to this AI will become the new target.
    /// </summary>
    public void UpdateTarget()
    {
        //TODO: Finalize Attacking Multiple People
        this.attackTargets.Clear();

        float closestTarget = float.MaxValue; //Needs tweeking, initial value doesnt matter
        //myCurrentTarget = null;

        //  List<Transform> percievedTargets = new List<Transform>();
        if (myOwner.GetMyPerception().GetRecentPercievedTargets() != null)
        {
            percievedTargets = myOwner.GetMyPerception().GetRecentPercievedTargets();

            if (percievedTargets.Count == 0)
            {
                this.myCurrentTarget = null;
            }
            if (percievedTargets.Count == 1)
            {
                this.myCurrentTarget = percievedTargets[0];
            }
            else
            {
                for (int i = 0; i <= percievedTargets.Count - 1; i++)
                {
                    if (percievedTargets[i] != this.myTransform)
                    {
                        //Getting the distnace between the target and the AI
                        float distance = Vector3.Distance(percievedTargets[i].transform.position, myTransform.position);

                        //If the distance between the two is below the previously closest target
                        //then set that as the newest target
                        if (distance < closestTarget)
                        {
                            closestTarget = distance;
                            myCurrentTarget = percievedTargets[i];

                            //TODO: Finalize Attacking Multiple People
                            if (attackTargets.Count - 1 <= targetCap && !attackTargets.Contains(percievedTargets[i].gameObject))
                                attackTargets.Add(percievedTargets[i].gameObject);
                        }
                    }
                }
            }
        }
    }

    public bool TargetInFOV()
    {
        return myCurrentTarget != null;
    }

    #region Wrapper Functions for gaining access to the AI's perception
    public bool TargetAttackable()
    {
        if (this.TargetInFOV())
        {
            return myOwner.GetMyPerception().IsAttackable(myCurrentTarget);
        }
        return false;
    }

    public Vector3 GetTargetLastPosition()
    {
        return myOwner.GetMyPerception().GetLastVisibleTargetPos(myCurrentTarget);
    }

    public float GetTargetVisibleTime()
    {
        return myOwner.GetMyPerception().GetTargetVisibleTime(myCurrentTarget);
    }

    public float GetWhenTargetLastVisible()
    {
        return myOwner.GetMyPerception().GetWhenTargetLastVisible(myCurrentTarget);
    }
    #endregion
}
