using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// The base class for defining different Attacks
/// Living objects aggregate their own AttackSystem,
/// which in turn uses an array of concrete implementations
/// of this class, ie. Punch, Dash, Ground Pound.
/// </summary>
public abstract class Attack : MonoBehaviour
{
    public AttackProps myProperties;

    /// <summary>
    /// Various methods used by an AttackSystem to fire this Attack
    /// 
    /// (This is the implementation for what the Attack does.)
    /// </summary>
    public virtual void AttackTarget()
    {
        this.UpdateLastAttackTime();
    }

    public virtual void AttackTarget(Vector2 dir)
    {
        this.UpdateLastAttackTime();
    }

    public virtual void AttackTarget(GameObject target)
    {
        this.UpdateLastAttackTime();
    }

    public virtual void AttackTarget(GameObject target, Vector2 dir)
    {
        this.UpdateLastAttackTime();
    }

    public virtual void AttackTarget(GameObject target, float force)
    {
        this.UpdateLastAttackTime();
    }

    public virtual void AttackTarget(GameObject target, Vector2 dir, float force)
    {
        this.UpdateLastAttackTime();
    }

    public virtual void AttackTargets(List<GameObject> targets)
    {
        this.UpdateLastAttackTime();
    }

    /// <summary>
    /// Checks to see if the attack is ready by
    /// comparing the last time it was used verses the cool down time
    /// </summary>
    /// <returns></returns>
    protected virtual bool AttackReady()
    {
        if (Time.time > myProperties.coolDown + myProperties.timeSinceAttack)
        {
            this.myProperties.canAttack = true;
            return true;
        }
        this.myProperties.canAttack = false;
        return false;
    }

    // sets last attack time to Time.time
    protected void UpdateLastAttackTime()
    {
        this.myProperties.timeSinceAttack = Time.time;
    }
}

/// <summary>
/// The properties associated with Attacks
/// </summary>
[Serializable]
public class AttackProps
{
    //[HideInInspector]
   // public Living myOwner;


  //  [Tooltip("How fast I can attack")]
    public float attackRate;

  // [Tooltip("How long before I can attack again")]
    public float coolDown;

    [HideInInspector]
    public float timeSinceAttack = 0;

  //  [Tooltip("The area my attack effects.")]
    public Vector3 areaOfAttack;

  //  [Tooltip("How much damage the attack does.")]
    public float damage;

    public bool canAttack;

}