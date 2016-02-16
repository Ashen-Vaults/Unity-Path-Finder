using UnityEngine;
using System.Collections;
using System;
using System.Linq;

//Delegate for turning On/Off movement patterns
//public delegate void MovementHandler(bool state);
public delegate void MovementHandler(AIMovement _mov);


public class AIMovement : Movement
{
  //  [Space(20)]
    public AIMoveProps myProperties;


    public static int behaviour;

  //  [Space(20)]
  //  [Header("Debugs: DONT TOUCH")]
    public int flag;

    public enum KnockBackType { TIME = 0, PHYSICS = 1 };
    public KnockBackType knockback;

    void Awake()
    {
        //this.myProperties.myAgent = this.GetComponent<NavMeshAgent>();
        //     this.myProperties.maxSpeed = 4f;
        //      this.myProperties.maxAcceleration = 2f;
        //     this.myProperties.maxAngularSpeed = 45f;
    }


    /// <summary>
    /// Turns the enum to contain flags which effectively make the 
    /// enum have multiple states
    /// Each enum is stored as a binary value as a 
    /// multiple of 2. We then can shift the bits in the enum state to contain
    /// as many different flags are we want.
    /// </summary>
    [Flags]
    //   [HideInInspector]
    public enum MovementType
    {
        //BitShift               //Int       Binary
        none = 0,                 //0        000000
        seek = 1 << 0,            //2        000001
        arrive = 1 << 1,          //4        000010
        flee = 1 << 2,            //6        000100
        wander = 1 << 3,          //8        001000
        followPath = 1 << 4,      //16       010000
        separation = 1 << 5,      //32       100000
        knockBack = 1 << 6        //64       1000000  
    }


    public MovementType movementType;


    /// <summary>
    /// Checks to see if the current movement behaviour is 
    /// currently activated.
    /// </summary>
    /// <param name="_type">The movement behaviour</param>
    /// <returns>The state of the movement type</returns>
    public bool On(MovementType _type)
    {
        return ((flag & (int)_type) == (int)_type);
    }

    public int GetActiveBehaviour()
    {
        int flagBits = Enum.GetValues(typeof(MovementType)).Cast<int>().Sum();
        print(flagBits);
        return flagBits;
    }

    public static MovementType SetFlag(MovementType a, MovementType b)
    {
        return a | b;
    }

    public static MovementType UnsetFlag(MovementType a, MovementType b)
    {
        return a & (~b);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool HasFlag(MovementType a, MovementType b)
    {
        return (a & b) == b;
    }

    public static MovementType ToogleFlag(MovementType a, MovementType b)
    {
        return a ^ b;
    }

    /*
    public static MovementType GetTypeFromBit(int _i)
    {
        
    }
    */
    ////////////////////////////////////////////
    ///          TURN ON Behaviours          ///
    ////////////////////////////////////////////

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the seeking behaviour.
    /// </summary>
    public void SeekOn()
    {
        flag |= (int)MovementType.seek;
        //    movementType = float;
    }


    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the arrive behaviour.
    /// </summary>
    public void ArriveOn()
    {
        flag |= (int)MovementType.arrive;
    }


    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the flee behaviour.
    /// </summary>
    public void FleeOn()
    {
        flag |= (int)MovementType.flee;
    }


    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the wander behaviour.
    /// </summary>
    public void WanderOn()
    {
        flag |= (int)MovementType.wander;
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the seperate behaviour.
    /// </summary>
    public void SeparationOn()
    {
        flag |= (int)MovementType.separation;
    }


    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the follow path behaviour.
    /// </summary>
    public void FollowPathOn()
    {
        flag |= (int)MovementType.followPath;
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// contain the follow path behaviour.
    /// </summary>
    public void KnockBackOn()
    {
        flag |= (int)MovementType.knockBack;
    }

    ////////////////////////////////////////////
    ///          TURN OFF Behaviours         ///
    ////////////////////////////////////////////
    #region Turn Off Movement Behaviours

    [ContextMenu("Clear Flags")]
    public void ClearAllFlags()
    {
        flag = 0;   //sets the flag to bn
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the seek behaviour.
    /// </summary>
    public void SeekOff()
    {
        if (On(MovementType.seek))
        {
            flag ^= (int)MovementType.seek;
        }
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the wander behaviour.
    /// </summary>
    public void WanderOff()
    {
        if (On(MovementType.wander))
        {
            flag ^= (int)MovementType.wander;
        }
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the arrive behaviour.
    /// </summary>
    public void ArriveOff()
    {
        if (On(MovementType.arrive))
        {

            flag ^= (int)MovementType.arrive;
            this.SeekOff();
        }
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the flee behaviour.
    /// </summary>
    public void FleeOff()
    {
        if (On(MovementType.flee))
        {
            flag ^= (int)MovementType.flee;
        }
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the follow path behaviour.
    /// </summary>
    public void FollowPathOff()
    {
        if (On(MovementType.followPath))
        {
            flag ^= (int)MovementType.followPath;
        }
    }

    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the follow path behaviour.
    /// </summary>
    public void KnockBackOff()
    {
        if (On(MovementType.knockBack))
        {
            flag ^= (int)MovementType.knockBack;
        }
    }


    /// <summary>
    /// Shifts the bits in the MovementTypes enum to 
    /// remove the seperate path behaviour.
    /// </summary>
    public void SeparationOff()
    {
        if (On(MovementType.flee))
        {
            flag ^= (int)MovementType.separation;
        }
    }
    #endregion Turn Off Movement Behaviours



    #region Behaviour Functionality

    /// <summary>
    /// The Basic Steering behaviour for seeking
    /// The AI will move to the given target
    /// </summary>
    public void Seek()
    {
        // 
        this.myProperties.myAgent.speed = this.myProperties.mySeek.speed;
        //   this.myProperties.myAgent.angularSpeed = this.myProperties.mySeek.angularSpeed;
        //   this.myProperties.myAgent.acceleration = this.myProperties.mySeek.acceleration; 
        this.myProperties.myAgent.destination = this.myProperties.myTarget.transform.position;
        // this.myProperties.myAgent.Move(new Vector3(this.myProperties.arriveCurve.Evaluate(this.myProperties.curveTime), 0f, this.myProperties.arriveCurve.Evaluate(this.myProperties.curveTime)));
        // transform.LookAt(new Vector3(this.myProperties.arriveCurve.Evaluate(this.myProperties.curveTime), 0f, this.myProperties.arriveCurve.Evaluate(this.myProperties.curveTime)));

        if (this.myProperties.mySeek.curveStatus == true)
        {
            this.myProperties.myAgent.velocity = new Vector3(this.myProperties.mySeek.curve.Evaluate(this.myProperties.mySeek.curveTime), 0f, this.myProperties.mySeek.curve.Evaluate(this.myProperties.mySeek.curveTime));
            transform.LookAt(new Vector3(this.myProperties.mySeek.curve.Evaluate(this.myProperties.mySeek.curveTime), 0f, this.myProperties.mySeek.curve.Evaluate(this.myProperties.mySeek.curveTime)));
        }

    }

    /// <summary>
    /// Runs away from the target
    /// </summary>
    public void Flee()
    {
        if (this.myProperties.myAgent.remainingDistance < 15f)
            this.myProperties.myAgent.destination = this.transform.position - this.myProperties.myTarget.transform.position;
    }

    /// <summary>
    /// Will Slow Down when the AI comes close to a target;
    /// </summary>
    public void Arrive()
    {
        this.myProperties.myAgent.stoppingDistance = this.myProperties.myArrive.arriveDistance;
        this.Seek();
        if (this.myProperties.myAgent.remainingDistance <= this.myProperties.myArrive.arriveDistance)
        {
            transform.LookAt(this.myProperties.myTarget.transform.position);  //Looks at the target when they arrive, to make sure they are rotated at  the right direction
        }
    }

    /// <summary>
    /// Creates a random movement pattern for an AI by finding a random point on a circle 
    /// in fron of the AI, then moving to that location. 
    /// </summary>
    public void Wander()
    {
        this.myProperties.myAgent.speed = this.myProperties.myWander.speed;
        //   this.myProperties.myAgent.angularSpeed = this.myProperties.myWander.angularSpeed;
        //    this.myProperties.myAgent.acceleration = this.myProperties.myWander.acceleration;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * myProperties.myWander.wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, UnityEngine.Random.Range(0f, myProperties.myWander.wanderRadius), 1);
        if (this.myProperties.myWander.curveStatus == true)
        {
            this.myProperties.myAgent.Move(new Vector3(0f, 0f, this.myProperties.myWander.curve.Evaluate(this.myProperties.myWander.curveTime)));
            transform.LookAt(new Vector3(this.myProperties.myWander.curve.Evaluate(this.myProperties.myWander.curveTime), 0f, this.myProperties.myWander.curve.Evaluate(this.myProperties.myWander.curveTime)));
        }
        myProperties.myAgent.destination = hit.position;
    }

    /// <summary>
    /// Follows a fixed waypoint path.
    /// If ordered is true then it will follow it sequentially in the list.
    /// Otherwise it will randomize where it goes.
    /// </summary>
    public void FollowPath()
    {

        this.myProperties.myAgent.speed = this.myProperties.myFollowPath.speed;
        // this.myProperties.myAgent.angularSpeed = this.myProperties.myFollowPath.angularSpeed;
        // this.myProperties.myAgent.acceleration = this.myProperties.myFollowPath.acceleration;


        // Returns if no points have been set up
        if (myProperties.myFollowPath.wayPoints.Length == 0 || myProperties.myFollowPath.wayPoints.Length == 1)
        {
            this.myProperties.myAgent.Stop();
        }
        else
        {


            // Set the agent to go to the currently selected destination.
            this.myProperties.myAgent.destination = myProperties.myFollowPath.wayPoints[myProperties.myFollowPath.destPoint].position;

            if (this.myProperties.myFollowPath.curveStatus == true)
            {
                this.myProperties.myAgent.Move(new Vector3(0f, 0f, this.myProperties.myFollowPath.curve.Evaluate(this.myProperties.myFollowPath.curveTime)));
                transform.LookAt(new Vector3(0f, 0f, this.myProperties.myFollowPath.curve.Evaluate(this.myProperties.myFollowPath.curveTime)));
            }
            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            if (myProperties.myFollowPath.ordered) myProperties.myFollowPath.destPoint = (myProperties.myFollowPath.destPoint + 1) % myProperties.myFollowPath.wayPoints.Length;
            else myProperties.myFollowPath.destPoint = (UnityEngine.Random.Range(0, myProperties.myFollowPath.wayPoints.Length));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Separation()
    {
        Collider[] aroundMe = this.GetComponent<AI>().myProperties.myPerception.hitColliders;

        for (int i = 0; i <= aroundMe.Length - 1; i++)
        {

        }

    }

    /// <summary>
    /// stops nav mesh movement and enables rigidbody physics before adding a force to an enemy
    /// triggers a coroutine to control returning to nav mesh movement
    /// </summary>
    /// <param name="knockBackForce"> the force to be applied to the enemy </param>
    public void KnockBack(float knockBackForce)
    {

        this.ClearAllFlags(); //Turn off any currently active movements
        //is the object currently being knocked back
        //disable the nav mesh movement and then add force to the enemy
        KnockBackOn();

        this.myProperties.myAgent.Stop();
        this.myProperties.myAgent.updatePosition = false;
        this.myProperties.myAgent.updateRotation = false;
        this.myProperties.myAgent.enabled = false;
        this.myProperties.myRigidbody.isKinematic = false;
        this.myProperties.myRigidbody.AddForce(-this.transform.forward * knockBackForce, ForceMode.VelocityChange);
        StartCoroutine(KnockBackEnd());
    }

    /// <summary>
    /// stops nav mesh movement and enables rigidbody physics before adding a force to an enemy
    /// triggers a coroutine to control returning to nav mesh movement
    /// </summary>
    /// <param name="dir"> the direction to move in (x,z) </param>
    /// <param name="knockBackForce"> the force to be applied to the enemy </param>
    public void KnockBack(Vector2 dir, float knockBackForce)
    {

        this.ClearAllFlags(); //Turn off any currently active movements
        //disable the nav mesh movement and then add force to the enemy
        KnockBackOn();

        //TEMP: Will use method below:
        //this.ArriveOff();

        //TODO: TURN OFF ACTIVE AIMOVEMENT
        //this.TurnOffActive();

        //this.myProperties.myAgent.Stop();
        this.myProperties.myAgent.updatePosition = false;
        this.myProperties.myAgent.updateRotation = false;
        this.myProperties.myAgent.enabled = false;
        this.myProperties.myRigidbody.isKinematic = false;
        this.myProperties.myRigidbody.AddForce(new Vector3(dir.x, 0, dir.y) * knockBackForce, ForceMode.VelocityChange);
        StartCoroutine(KnockBackEnd());
    }

    /// <summary>
    /// checks to see if an enemy's speed is below a certain value, then returns to nav mesh movement once this is true
    /// has a delay between reaching the speed threshold and restarting nav mesh physics
    /// </summary>
    /// <returns></returns>
    public IEnumerator KnockBackEnd()
    {
        //Delay to wait for the velocity change
        yield return new WaitForSeconds(.01f);

        //loop until the speed is low enough
        while (Mathf.Abs(this.myProperties.myRigidbody.velocity.magnitude) >= 2f)
        {
            yield return new WaitForSeconds(.1f);
        }

        //stop movement and wait for a set amount of time
        this.myProperties.myRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(myProperties.myKnockBack.getUpTime);

        //disable rigidbody physics and renebale the nav mesh features
        KnockBackOff();

        this.myProperties.myRigidbody.isKinematic = true;
        this.myProperties.myAgent.enabled = true;
        this.myProperties.myAgent.ResetPath();

        this.myProperties.myAgent.updatePosition = true;
        this.myProperties.myAgent.updateRotation = true;
        this.ArriveOn();
    }


    /// <summary>
    /// Knocks an enemy back after they are attacked
    /// by setting their current velocity to move backwards
    /// </summary>
    /// <param name="knockBackPos">The position for the desired knockback</param>
    public void KnockBackNOTPHYSICS(float knockBackForce)
    {
        this.KnockBackOn();
        //Store the velocity before knockback is applied
        this.myProperties.myKnockBack.beforeKnockbackVelocity = this.myProperties.myAgent.velocity;

        //knock them back
        if (this.knockback == KnockBackType.TIME)
        {
            this.myProperties.myAgent.velocity = new Vector3
            (
                (-this.transform.forward.x * ((knockBackForce))),
                0.5f,
                (-this.transform.forward.z * ((knockBackForce)))
            );
        }

        if (this.knockback == KnockBackType.PHYSICS)
        {
            //this.myProperties.myAgent.velocity -= Vector3.ClampMagnitude(this.myProperties.myAgent.velocity, 1) * this.myProperties.knockbackFriction * Time.deltaTime;

            //Vector3 acceleration = (((new Vector3(1, 0, 1) * 2) - ((this.myProperties.myAgent.velocity)*2) ) / 2 * this.myProperties.resetVelocityTime);
            //this.myProperties.myAgent.velocity = (acceleration);
        }

        // reset velocity
        this.myProperties.myKnockBack.duringknockbackVelocity = this.myProperties.myAgent.velocity;

        if (this.knockback == KnockBackType.PHYSICS) StartCoroutine(knockBackPhysics());
        else
            if (this.knockback == KnockBackType.TIME) StartCoroutine(knockBackDuration());
    }

    /// <summary>
    /// Makes the enemy retreat. 
    /// by setting their current velocity to move backwards
    /// This is different from Knockback because it is not a force that is 
    /// directed onto an enemy, rather a force the enemy makes.
    /// </summary>
    /// <param name="retreatForce">The position for the desired retreat</param>
    public void Retreat(float retreatForce)
    {
        //Store the velocity before knockback is applied
        this.myProperties.myKnockBack.beforeKnockbackVelocity = this.myProperties.myAgent.velocity;

        //knock them back
        this.myProperties.myAgent.velocity = new Vector3
             (
                 (-this.transform.forward.x * retreatForce),
                 0.5f,
                 (-this.transform.forward.z * retreatForce)
             );
        this.myProperties.myKnockBack.duringknockbackVelocity = this.myProperties.myAgent.velocity;

        //reset velocity
        if (this.knockback == KnockBackType.PHYSICS) StartCoroutine(knockBackPhysics());
        else
            if (this.knockback == KnockBackType.TIME) StartCoroutine(knockBackDuration());
    }

    /// <summary>
    /// The amount of time before we reset the velocity of the enemy after it is knock backed
    /// </summary>
    /// <returns></returns>
    IEnumerator knockBackDuration()
    {
        yield return new WaitForSeconds(this.myProperties.myKnockBack.resetVelocityTime);
        this.myProperties.myAgent.velocity = this.myProperties.myKnockBack.beforeKnockbackVelocity;
        this.KnockBackOff();
    }



    /// <summary> TODO: Need to actually implement
    /// The amount of time before we reset the velocity of the enemy after it is knock backed
    /// </summary>
    /// <returns></returns>
    IEnumerator knockBackPhysics()
    {
        //print("VEL " + this.myProperties.myAgent.velocity);
        // Vector3 acceleration = (((new Vector3(1, 0, 1) * 2) - ((this.myProperties.myAgent.velocity) * 2)) / 2 * this.myProperties.resetVelocityTime);
        //  for (int i = 0; i <= this.myProperties.resetVelocityTime; i++)
        // {
        //   print(   "  "+ this.myProperties.myAgent.velocity);
        yield return new WaitForSeconds(1);
        this.myProperties.myAgent.velocity *= -this.myProperties.myKnockBack.knockbackFriction;

        if (this.myProperties.myAgent.acceleration < this.myProperties.myKnockBack.minVelocity)
        {
            this.myProperties.myAgent.velocity = Vector3.zero;
            this.KnockBackOff();
        }
        //}
        //  this.KnockBackOff();
    }


    #endregion Behaviour Functionality

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            this.myProperties.myPath = newPath;
            StopCoroutine("FollowWayPoints");
            StartCoroutine("FollowWayPoints");
        }
    }

    IEnumerator FollowWayPoints()
    {
        Vector3 currentWaypoint = this.myProperties.myPath[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                this.myProperties.targetIndex++;
                if (this.myProperties.targetIndex >= this.myProperties.myPath.Length)
                {
                    yield break;
                }
                currentWaypoint = this.myProperties.myPath[this.myProperties.targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, this.myProperties.maxSpeed * Time.deltaTime);
            yield return null;

        }
    }

    /// <summary>
    /// Moves the AI according to the movement behaviour that 
    /// is currently On.
    /// </summary>
    public void CalculateMovement()
    {
        if (this.myProperties.myAgent.speed != this.myProperties.maxSpeed)
        {
            this.myProperties.myAgent.speed = this.myProperties.maxSpeed;
        }

        if (this.myProperties.myAgent.acceleration != this.myProperties.maxAcceleration)
        {
            this.myProperties.myAgent.acceleration = this.myProperties.maxAcceleration;
        }

        if (this.myProperties.myAgent.angularSpeed != this.myProperties.maxAngularSpeed)
        {
            this.myProperties.myAgent.angularSpeed = this.myProperties.maxAngularSpeed;
        }

        //curveTime += Time.deltaTime;


        if (On(MovementType.seek))
            this.Seek();

        if (On(MovementType.wander))
        {
            if (this.myProperties.myAgent.remainingDistance <= this.myProperties.myWander.wanderTargetDistance)
                this.Wander();
        }

        if (On(MovementType.arrive))
            this.Arrive();

        if (On(MovementType.flee))
            this.Flee();


        if (On(MovementType.followPath))
        {
            if (this.myProperties.myAgent.remainingDistance < 5f)
            {
                this.FollowPath();
            }
        }

        if (On(MovementType.separation))
        {
            Separation();
        }

        //Will fill out later for optimization
        if (On(MovementType.knockBack))
        {
            // KnockBack();

        }

    }


    /// <summary>
    /// Finds a random point on a circle and then sends a ray towards it. 
    /// If the ray hits the point then we return the random position that it
    /// hit. 
    /// </summary>
    /// <param name="origin">The starting location of the ray.</param>
    /// <param name="dist">How far the ray goes.</param>
    /// <param name="layermask">Which layer this ray should be casted on.</param>
    /// <returns></returns>
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    public static double Length(Vector3 v1)
    {
        return Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z);
    }

    public static Vector3 Normalize(Vector3 v1)
    {
        double length = Length(v1);
        if (length > 0)
        {
            return new Vector3((v1.x / (float)Length(v1)), 0, (v1.z / (float)Length(v1)));
        }
        else
        {
            return new Vector3((v1.x / 1), 0, (v1.z / 1));
        }
    }


    public static Vector3 Div(Vector3 v1, Double _x)
    {
        v1.x /= (float)_x;
        v1.z /= (float)_x;

        return v1;
    }

    public override Vector3 CalculateForce()
    {
        throw new NotImplementedException();
    }

    public override Vector3 CalculateForce(Vector3 target)
    {
        throw new NotImplementedException();
    }



    #region DEBUG

    public void OnDrawGizmos()
    {
        if (this.myProperties.myPath != null)
        {
            for (int i = this.myProperties.targetIndex; i < this.myProperties.myPath.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(this.myProperties.myPath[i], Vector3.one);

                if (i == this.myProperties.targetIndex)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, this.myProperties.myPath[i]);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(this.myProperties.myPath[i - 1], this.myProperties.myPath[i]);
                }
            }
        }
    }
    /* private void OnDrawGizmosSelected()
     {
         for (int i = 0; i < this.myProperties.myFollowPath.wayPoints.Length - 1; i++)
         {
             Gizmos.color = Color.green;
             //Gizmos.DrawLine(this.myProperties.wayPoints[i].gameObject.transform.position, this.myProperties.wayPoints[(myProperties.destPoint + 1) % myProperties.wayPoints.Length].transform.position);
             //check if the object is within the angle bounds
             if (i != this.myProperties.myFollowPath.destPoint)
             {
                 Gizmos.color = Color.red;
                 //Gizmos.DrawLine(this.myProperties.myFollowPath.destPoint[(myProperties.myFollowPath.destPoint + 1) % myProperties.myFollowPath.wayPoints.Length].transform.position,this.myProperties.myFollowPath.wayPoints[i].gameObject.transform.position);
                 //  Gizmos.DrawLine(this.myProperties.wayPoints[(myProperties.destPoint + 1) % myProperties.wayPoints.Length].transform.position, this.myProperties.wayPoints[this.myProperties.destPoint].transform.position);
             }
             else
             {
                 Gizmos.color = Color.green;
                 Gizmos.DrawLine(this.myProperties.myFollowPath.wayPoints[i].gameObject.transform.position, this.myProperties.myFollowPath.wayPoints[(myProperties.myFollowPath.destPoint + 1) % myProperties.myFollowPath.wayPoints.Length].transform.position);
             }
         }
     }    */

    [ContextMenu("Test Knockback")]
    public void TestKnockBack()
    {
        this.KnockBack(15f);
    }

    #endregion DEBUG

}


[Serializable]
public class AIMoveProps
{
    #region General Properties
  //  [HideInInspector]
    public NavMeshAgent myAgent;
    public Rigidbody myRigidbody;

    public GameObject myTarget;
    public float radius;

    public Vector3[] myPath;
    public int targetIndex;

    [Range(0, 10)]
   // [Tooltip("How fast the AI will go")]
    public float maxSpeed;

    [Range(1, 50)]
  //  [Tooltip("How fast the AI will Accelerate")]
    public float maxAcceleration;

    [Range(0, 360)]
   // [Tooltip("How fast the AI will turn. 0=never turns")]
    public float maxAngularSpeed;
    #endregion General Properties
   // [Space(20)]
   // [Header("~MovementTypes----------------------------------------------------------")]
   // [Space(20)]
    public SeekProps mySeek;
    public ArriveProps myArrive;
    public FleeProps myFlee;
    public WanderProps myWander;
    public SeparationProps mySeparation;
    public FollowPathProps myFollowPath;
    public KnockBackProps myKnockBack;
}

[Serializable]
public abstract class AIMoveGenerals
{
    public DistanceHeuristic distanceType;
    public AnimationCurve curve;
    public float curveTime;
    public bool curveStatus;
    public float speed;
    public float angularSpeed;
    public float acceleration;
}



#region MovementType Properties & Attributes
[Serializable]
public class SeekProps : AIMoveGenerals { }

[Serializable]
public class FleeProps : AIMoveGenerals
{
   // [Tooltip("How close I can be to my target before I start to Flee.")]
    public float fleeSpeed;
    public float fleeDistance;
}

[Serializable]
public class ArriveProps : AIMoveGenerals
{
  //  [Tooltip("Defines when I should start to slow down based off how close I am to my current target")]
    public float arriveDistance;
    public float minVelocity;
}

[Serializable]
public class WanderProps : AIMoveGenerals
{
   // [Tooltip("How big the area is for me to wander")]
    public float wanderRadius;
  //  [Tooltip("How far I can wander in the wander radius")]
    public float wanderTargetDistance;
}

[Serializable]
public class SeparationProps : AIMoveGenerals
{
  //  [Tooltip("The acceleration I can separate at")]
    public float separateMaxAcceleration;
  //  [Tooltip("How close another AI can get to me, before I try to separate")]
    public float closenessThreshold;
}

[Serializable]
public class FollowPathProps : AIMoveGenerals
{
   // [Tooltip("TRUE = Moves the AI to each waypoint sequently\nFALSE= The next waypoint is randomly chosen from the array")]
    public bool ordered = true;
   // [Tooltip("The tether is")]
    public Vector2 tether;
    public bool flee;
   // [Tooltip("The nodes that the AI will move to")]
    public Transform[] wayPoints;
   // [Tooltip("The Waypoint the AI is currently moving towards")]
    public int destPoint = 0;
}

[Serializable]
public class KnockBackProps : AIMoveGenerals
{
    [HideInInspector]
    public Vector3 beforeKnockbackVelocity;
    public Vector3 duringknockbackVelocity;
    [Range(.01f, 15f)]
    public float resetVelocityTime;
    public float friction = 0.0546f;
    public float knockbackFriction;
    public float getUpTime = 1f;
    public float minKnockbackVelocity = 2f;
    public float minVelocity;
}
#endregion MovementType Properties & Attributes

