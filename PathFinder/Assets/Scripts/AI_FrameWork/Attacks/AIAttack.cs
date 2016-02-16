using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The Attack system used specifically by AI.
/// </summary>
public class AIAttack : AttackSystem
{
    [HideInInspector]
    public AI myOwner;

    /// <summary>
    /// Gets the current Attack Type being used and 
    /// calls the Attack from that if a target is within range.
    /// </summary>
    public override void Attack()
    {
        // if (myOwner.myProperties.myTargetting.TargetAttackable())
        // {
        //print("ATTACK");
        this.myProperties.myCurrentAttack.AttackTarget(this.myOwner.myProperties.myTarget.gameObject);
        // }
    }

    /// <summary>
    /// Chooses the attack type an AI would want to use based  
    /// off the start
    /// </summary>
    public void ChooseAttack()
    {
        if (this.myOwner.myProperties.myTargetting.TargetAttackable())
        {

            this.myProperties.myCurrentAttack = this.myProperties.myAttacks[0];
           // print("Choosing " + this.myProperties.myCurrentAttack);
            Attack();
        }
    }
}
