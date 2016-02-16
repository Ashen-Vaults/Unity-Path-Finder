using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Things which will have AI, ie ENEMY, will aggregate an AI subclass
/// The subclass which handle the specific implemenation of the AI
/// The reason this isn't coupled with the Enemy, is because we are able to easily change 
/// the AI type it is using
/// </summary>
public class AI : MonoBehaviour, IPerceivable
{
    public AIProps myProperties;

   // [Tooltip("Set The Times Each Component Should Be Updated, 0 = constant, -1 = never, > 0 = time in miliseconds/frames")]
    public UpdateUtilites updater;

    public GameObject myGeometry;
    public SpriteRenderer myRenderer;


    void Awake()
    {

        AIMananger.OnSeekPlayer += SeekPlayer;

        //if (this.gameObject.GetComponent<Goal_Wander>() == null) this.gameObject.AddComponent<Goal_Wander>();

        if (myProperties.myBrain == null) myProperties.myBrain = this.GetComponent<Brain>();
        if (myProperties.myPerception == null) myProperties.myPerception = this.GetComponent<Perception>();
        if (myProperties.myMovement == null) myProperties.myMovement = this.GetComponent<AIMovement>();
        if (myProperties.myAttackSystem == null) myProperties.myAttackSystem = this.GetComponent<AIAttack>();
        if (myProperties.myMovement == null) myProperties.myMovement = this.GetComponent<AIMovement>();

        this.myProperties.myBrain.myProperties.myOwner = this;
        this.myProperties.myPerception.myOwner = this;
        this.myProperties.myAttackSystem.myOwner = this;
        this.myProperties.myTargetting.myOwner = this;

        #region Update Utilities
        this.updater.selectGoalUpdate = new UpdateUtility(this.updater.selectGoalTime);
        this.updater.targetUpdate = new UpdateUtility(this.updater.targetTime);
        this.updater.visionUpdate = new UpdateUtility(this.updater.visionTime);
        this.updater.attackTypeUpdate = new UpdateUtility(this.updater.attackTypeTime);
        #endregion

       

        //StartListening();
    }


    //Activate the brain to start making descisions about its goals
    void Start()
    {
        this.myProperties.myBrain.Activate();
        this.myProperties.myBrain.myProperties.myOwner = this;
        this.myProperties.myPerception.myOwner = this;
        this.myProperties.myTargetting.myOwner = this;
        this.myProperties.myAttackSystem.myOwner = this;
        this.myProperties.myMovement.myProperties.myAgent = this.transform.parent.GetComponent<NavMeshAgent>();

        if(this.myRenderer==null)
            this.myRenderer = this.myGeometry.transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Processes the current active goal
        this.myProperties.myBrain.Process();
        this.myProperties.myMovement.CalculateMovement();
        this.myProperties.myPerception.UpdateFOV();





        //Update Targetting
        if (this.updater.targetUpdate.IsReady())
        {
            //Update Movement

            this.myProperties.myTargetting.UpdateTarget();
            this.UpdateTarget();

            //TODO: REMOVE, this is temp until the entire AI system is done and each component uses
            //the same target
            if (this.myProperties.myMovement.myProperties.myTarget != this.myProperties.myTarget)
            {
                //    this.myProperties.myMovement.myProperties.myTarget = this.myProperties.myTarget;
            }
        }

        if (this.myProperties.myBrain.activateGoal == true)
        {
            this.myProperties.myBrain.myGoal.Activate();
        }

        //Process the currently active goal
        if (this.updater.selectGoalUpdate.IsReady())
        {
            this.myProperties.myBrain.SelectGoal();
        }

        //Updates the vision of the AI
        if (this.updater.visionUpdate.IsReady()) this.myProperties.myPerception.UpdateFOV();


        //Updates the Attack an ai is using based off of their situation
        if (this.updater.attackTypeUpdate.IsReady())
        {
            this.myProperties.myAttackSystem.ChooseAttack();
        }


        if (myProperties.overrideGoal == true)
        {
            this.myProperties.myBrain.Terminate();
            myProperties.overrideGoal = false;
        }

        if (this.myProperties.myTargetting.TargetInFOV())
        {

            if (this.myProperties.myTargetting.TargetAttackable())
            {
                if (this.myProperties.myAttackSystem.myProperties.myCurrentAttack.myProperties.canAttack)
                {
                    this.myRenderer.color = Color.red;
                }
            }
            else
            {
                this.myRenderer.color = Color.yellow;
            }
        }

        else
        {
            this.myRenderer.color = Color.white;
        }


    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1f);
        this.myGeometry.transform.parent.GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(this.myProperties.myAttackSystem.myProperties.myCurrentAttack.myProperties.coolDown);
    }



    /// <summary>
    /// The Search routine for the AI
    /// </summary>
    public virtual void Search(float _speed)
    {
        Move(_speed);
    }

    /// <summary>
    /// The Move routine for the AI
    /// </summary>
    public virtual void Move(float _Speed)
    {

    }

    void UpdateTarget()
    {

        this.myProperties.myTarget = this.myProperties.myTargetting.myCurrentTarget;
        if (this.myProperties.myTarget != null)
            this.myProperties.myMovement.myProperties.myTarget = this.myProperties.myTarget;
    }



    public Perception GetMyPerception()
    {
        return this.myProperties.myPerception;
    }

    /// <summary>
    /// If the Ai is currently arriving to a target
    /// but has no target for it to arrive to
    /// then the AI is lost
    /// </summary>
    /// <returns></returns>
    public bool IsLost()
    {
        if (this.myProperties.myMovement.On(AIMovement.MovementType.arrive) && !this.myProperties.myTargetting.TargetInFOV())
        {
            return true;
        }
        return false;
    }

    public void SeekPlayer()
    {
        this.myProperties.myPerception.fovRadius *= 10;
        this.myProperties.myPerception.fovRadius = 360f;
    }
}

/// <summary>
/// AIProps acts as a HUB to allow all aggregated classes to communicate 
/// with each other.
/// </summary>
[Serializable]
public class AIProps
{
 //   [Header("\t--Thinking Properties--")]
 //   [Space(20)]
    public Brain myBrain; //Decides which goal the AI will use
    public Perception myPerception; //The sensing and memory of the AI
    public Targetting myTargetting; //How the AI handles targetting
    public AIAttack myAttackSystem;
    public GameObject myTarget;
    public AIMovement myMovement;//How the AI moves
    // public GameInstance myGameInstance;
    void Awake() { myMovement.myProperties.myTarget = myTarget; }

    public bool overrideGoal, isLost, invinsible;
}

/// <summary>
/// Sets how often we update a specific AI component.
/// Use this to optimize the AI so that every aspect of the AI 
/// is not constantly updating.
/// </summary>
[Serializable]
public class UpdateUtilites
{
    public float targetTime = 2f;
    public float visionTime = 4f;
    public float selectGoalTime = 2f;
    public float attackTypeTime = -1f;

    public UpdateUtility targetUpdate;
    public UpdateUtility visionUpdate;
    public UpdateUtility selectGoalUpdate;
    public UpdateUtility attackTypeUpdate;
}
