using UnityEngine;
using System.Collections;

/// <summary>
/// Classes that use me can specify how they are supposed to move
/// This can handle anything from projectiles, AI, players, effects,etc...
/// We just need to create concrete implemenations that derive from this class
/// for it to be a type of movement
/// </summary>
public abstract class Movement : MonoBehaviour{
   // protected InputHandler inputHandler;
    

    public virtual void MoveGameObj(GameObject g) { }
    public virtual void MoveMe() { } //used for "possesion" in example if we want a "dead" player to be able to posses an AI
    public virtual void Move() { }// used for regular moving
    public abstract Vector3 CalculateForce();
    public abstract Vector3 CalculateForce(Vector3 target);
    //public abstract void CalculateForce();

}


