using UnityEngine;
using System.Collections;

/// <summary>
/// This is just being treated as an object type for now.
/// The reason being is because Living objects (enemy/players) and AI's (which are not expliticly living)
/// need an object type interface to be used by the perception class.
/// </summary>
public interface IPerceivable
{
    Perception GetMyPerception();
}
