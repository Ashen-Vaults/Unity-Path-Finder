using UnityEngine;
using System.Collections;

/// <summary>
/// The base class for defining different Attacks
/// Living objects aggregate their own AttackSystem,
/// which in turn uses an array of concrete implementations
/// of this class, ie. Punch, Dash, Ground Pound.
/// </summary>
public class AttackSystem : MonoBehaviour {
    public AttackSystemProps myProperties;
   

    /// <summary>
    /// Gets the current Attack Type being used and 
    /// calls the Attack 
    /// </summary>
    public virtual void Attack(){}
}

/// <summary>
/// The Properties associated with the Attack System
/// </summary>
[System.Serializable]
public class AttackSystemProps
{
    public IPerceivable myOwner;
    public Attack[] myAttacks;
    public Attack myCurrentAttack;
    public float mySpeed; //How fast the attack I make is
    public float myReactionTime; //How fast I take to begin my attack
    public float myAccuracy; 
    public float myPersitance; //How often I'll try to attack a target after failing 
}