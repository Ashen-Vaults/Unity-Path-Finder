using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{

    public delegate void ReachPath();
    public event ReachPath OnReachedPath;

    public Transform target;
    public float speed = 20;
    public Vector3[] path;
    public Vector3 origDestination;
    public Vector3 currentWaypoint;
    public int targetIndex; //

    public DistanceHeuristic distanceType;
    public bool simplifyPath, showPath;
    public bool changePath, followingPath;
    public float distanceFromTarget, rerouteDistance, refreshRate;

    void Start()
    {
        origDestination = target.position;
        PathFinder.PathMananger.RequestPath(transform.position, target.position, distanceType, simplifyPath, OnPathFound);
    }                                                                                           

    void Update()
    {
        distanceFromTarget = Vector3.Distance(transform.position, target.position);
        StartCoroutine(RefreshPath());
    }

    public void RaiseReachedPath()
    {
        if (OnReachedPath != null)
        {
            OnReachedPath();
        }
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator RefreshPath()
    {
        if (distanceFromTarget < rerouteDistance)
        {
            PathFinder.PathMananger.RequestPath(transform.position, target.position, distanceType, simplifyPath, OnPathFound);
            origDestination = target.position;
            yield return new WaitForSeconds(refreshRate);
        }
        
    }

    IEnumerator FollowPath()
    {
     
        if (path != null)
        {
            changePath = false;
            followingPath = true;

            if(path[0] != null)
                currentWaypoint = path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = new Vector3[0];
                        followingPath = false;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                if(origDestination != target.position)
                {
                    changePath = true;
                    followingPath = false;
                    break;
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
             //   transform.LookAt(path[targetIndex]);  //Looks at the target when they arrive, to make sure they are rotated at  the right direction
                yield return null;

            }
        }
    }

    public void OnDrawGizmos()
    {
        if (this.path != null && showPath)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], new Vector3(1,10,1));

                if (i == targetIndex)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
