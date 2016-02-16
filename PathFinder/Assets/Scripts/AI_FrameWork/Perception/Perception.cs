using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Perception handles each time an AI's target comes within the FOV of that AI.
/// </summary>
public class Perception : MonoBehaviour
{
    /// <summary>
    /// Each time an AI encounters a new target
    /// A new instance of the Memory class is created and 
    /// added to this Dictionary. Whenever the target is then perceived 
    /// again, the key in MemoryDictionary which matches the target
    /// will be updated and the AI will then act accordingly.
    /// </summary>
    private class Memory : Dictionary<GameObject, MemoryProps> { }

    //The Memory which belongs to this AI
    private Memory myMemory = new Memory();

    //The Living Object which uses this 
    [SerializeField]
   // [Header("\t--Perception Properties--")]
   // [Space(20)]
    // [HideInInspector]
    public AI myOwner;

    //public GameInstance myInstance;
    public LayerMask mask;
    public LayerMask losMask;
    //When an AI wants a list of 
    //of all the "recently" percieved enemies
    //it removes those which fall behind the  
    //memorySpan they have 
    //(0 == out of sight out of mind)
    //(X>0, where X is super high, the better the memory
    [SerializeField]
    private float myMemorySpan = 0f;
    public List<GameObject> targets;

    public float fovRadius, angleSpread, attackDistance;

    //[Space(20)]

    public Collider[] hitColliders = new Collider[0];

    //The Properties of Perception \m/ \m/
    // [Header("\t--Memory--")]
    public MemoryProps myMemoryProperties;

    /// <summary>
    /// Checks to see if we have a memory of the new target
    /// If we do not, then we create one
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    private void MakeNewMemory(GameObject target)
    {
        if (!myMemory.ContainsKey(target))
        {
            targets.Add(target);
            myMemory.Add(target, new MemoryProps());
        }
    }

    void Awake()
    {
        this.myOwner = this.transform.parent.GetComponent<AI>();

    }

    void Start()
    {
        //this.myInstance = this.GetComponentInParent<GameInstance>();     //unCOMMENT FOR SPLITSCREEN 
        // targets.Add(this.myOwner.myProperties.myGameInstance.player.GameObject);
    }

    /// <summary>
    /// Returns a list of all the targets that had their memory
    /// properties updated within the AI's memorySpan.
    /// </summary>
    /// <returns>A list of all targets that the AI has seen recently</returns>
    public virtual List<GameObject> GetRecentPercievedTargets()
    {
        List<GameObject> enemies = new List<GameObject>();
        //   targets 

        foreach (KeyValuePair<GameObject, MemoryProps> entry in myMemory)
        {
            if ((Time.time - entry.Value.targetVisibleTime) <= myMemorySpan && IsInLayerMask(entry.Key.transform.gameObject, this.mask))
            {
                enemies.Add(entry.Key);//Adds those in memory to the targets of the AI
            }
        }
        return enemies;//returns those that within the memorySpan of this AI
    }

    /// <summary>
    /// Returns the time target has been in FOV 
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    /// <returns></returns>
    public virtual float GetTargetVisibleTime(GameObject target)
    {
        MemoryProps tempMemory = myMemory[target];
        if (tempMemory != null) return tempMemory.targetVisibleTime;
        return 0f;
    }

    /// <summary>
    /// Compares the layers of the gameobjects that it can percieve
    /// and the layer mask that you set the AI to be able to detect
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }

    /// <summary>
    /// Returns the time target was last seen
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    /// <returns></returns>
    public virtual float GetWhenTargetBecameVisible(GameObject target)
    {
        MemoryProps tempMemory = myMemory[target];
        if (tempMemory != null) return tempMemory.whenTargetLastVisible;
        return 0f;
    }

    /// <summary>
    /// Returns the amount of time the target has
    /// not been in the AI's FOV
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    /// <returns></returns>
    public virtual float GetWhenTargetLastVisible(GameObject target)
    {
        MemoryProps tempMemory = myMemory[target];
        if (tempMemory != null)
        {
            return Time.time - tempMemory.whenTargetLastVisible;
        }
        return 0f; //needs to be some high number we need to figure out
    }

    /// <summary>
    /// Returns true if the target is 
    /// with in the Field of View of the AI
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    /// <returns></returns>
    public virtual bool IsTargetInFOV(GameObject target)
    {
        MemoryProps tempMemory = myMemory[target];
        if (tempMemory != null) return tempMemory.targetInFOV;
        Debug.Log("Error-Attempting to get FOV info of unregistered target");
        return false;
    }

    /// <summary>
    /// Returns true if the target is 
    /// within range of the AI to be
    /// attacked
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    /// <returns></returns>
    public virtual bool IsAttackable(GameObject target)
    {
        MemoryProps tempMemory = myMemory[target];
        if (tempMemory != null) return tempMemory.targetAttackable;
        Debug.Log("Error-Attempting to get Attack info of unregistered target");
        return false;
    }

    /// <summary>
    /// Returns the target's position 
    /// from when they are last in FOV
    /// </summary>
    /// <param name="target">The Object the AI is seeking</param>
    /// <returns></returns>
    public Vector3 GetLastVisibleTargetPos(GameObject target)
    {
        MemoryProps tempMemory = myMemory[target];
        if (tempMemory != null) return tempMemory.lastVisibleTargetPos;
        Debug.Log("Error-Attempting to get Pos of unregistered target");
        return new Vector3(0f, 0f, 0f); //TODO: need to put better error-handling
    }

    /// <summary>
    /// We get all of the potential targets in this Game Instance
    /// Then update the memory Property information of each of those that
    /// exist within this AI's FOV
    /// </summary>
    public virtual void UpdateFOV()
    {

        //For each target in this AI's game instance
        //find those who are currently visible to the AI
        // List<GameObject> targets = new List<GameObject>(); //TODO: needs to get all the targets in the world
        // List<GameObject> targets = new List<GameObject>();
        // targets = this.myInstance.targets;

        if (targets != null)
        {
            hitColliders = Physics.OverlapSphere(transform.position, fovRadius, mask);
            //  print("Not Scanning");
            foreach (GameObject t in targets)
            {

                if (myOwner != t.GetComponent<AI>())//We dont want to this AI
                {
                    //Make a new memory if it doesn't already exist
                    MakeNewMemory(t);

                    //Get the memory info for this target
                    MemoryProps properties = myMemory[t];





                        //Gets all GameObjects that are within the radius around the AI
                        //Collider[]


                        for (int i = 0; i < hitColliders.Length; i++)
                        {
                            //check if the object is within the angle bounds
                            if (Vector3.Angle(transform.forward, (hitColliders[i].transform.position - transform.position)) < angleSpread)
                            {
                                //TODO: Check to see if something is between target and AI that would obsturct them
         
                                    if (Vector3.Distance(hitColliders[i].transform.position, transform.position) <= this.attackDistance)
                                    {
                                        properties.targetAttackable = true;
                                    }
                                    else
                                    {
                                        properties.targetAttackable = false;
                                    }


                                    // we need to check some how if its in the FOV
                                    properties.targetVisibleTime = Time.time;
                                    properties.lastVisibleTargetPos = t.transform.position;
                                    properties.whenTargetLastVisible = Time.time;
                                    properties.myLayer = t.gameObject.layer;
                                    /*If the new target was not originially in FOV
                                     *Then we set their FOV to true
                                     *And mark when we saw them
                                     */
                                    if (properties.targetInFOV == false)
                                    {
                                        properties.targetInFOV = true;
                                        properties.whenTargetBecameVisible = properties.targetVisibleTime;
                                    }
                                
                            }
                        }
                    }
                }
        }

        //  print("Scanning");
        ScanArea();

    }

    public virtual void ScanArea()
    {
        //Gets all GameObjects that are within the radius around the AI
        //Collider[]
        hitColliders = Physics.OverlapSphere(transform.position, fovRadius, mask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            //check if the object is within the angle bounds
            if (Vector3.Angle(transform.forward, (hitColliders[i].transform.position - transform.position)) < angleSpread)
            {
                if (hitColliders[i].GetComponent<Player>() != null)
                    MakeNewMemory(hitColliders[i].gameObject);

            }
        }
    }


    public virtual bool InLOS(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = target.transform.position-this.transform.position;
        if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit))
        {
            if (target.GetComponent<Collider>() == hit.collider)
                return true;
        }
        return false;
    }

    /// <summary>
    /// For Debugging Purposes, shows the GameObjects that are currently in view of the enemys
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, fovRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hitColliders[i].gameObject.transform.position, transform.position);
            //check if the object is within the angle bounds
            if (Vector3.Angle(transform.forward, (hitColliders[i].transform.position - transform.position)) < angleSpread)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hitColliders[i].transform.position - transform.position, transform.position);
            }
        }
    }
}

/// <summary>
/// Perception Mananges the Memory of 
/// what the AI has "experienced" 
/// Everytime an AI percieves one of their Targets
/// Memory is updated and then their AI Goals are 
/// changed whether or not they remember that the player was 
/// in their face a second ago or not.
/// </summary>
[Serializable]
public class MemoryProps
{

    //The time when the target was last percieved. 
    //We use this when determining whether or not an
    //AI can "REMEMBER" when a target was percieved, or 
    //if they are stupid and forget immediately  when
    //the target is out of sight (out of sight, out of mind)
   // [Tooltip("The Time the target was last sensed")]
    [HideInInspector]
    public float targetVisibleTime;

    //How long the target has been visible to this object
    //We set this to the Time.time when the target enters
    //this objects FOV then we get how long they have been visible
    // Time.time - whenTargetBecameVisible
    //[Tooltip("The Time the target has been visible to this object")]
    [HideInInspector]
    public float whenTargetBecameVisible;

    //Helpr variable - might be useful
    [HideInInspector]
    public float whenTargetLastVisible;

    //The position the targer was last percieved at. This can
    //be used to help target player if they go out of view
    [HideInInspector]
    public Vector3 lastVisibleTargetPos;

    //Set true if the target is within sight
    [HideInInspector]
    public bool targetInFOV;

    //Set true if there is nothing that
    //would obstuct an attack between
    //this AI and their target
    //    [HideInInspector]
    public bool targetAttackable;

    public LayerMask myLayer;

}