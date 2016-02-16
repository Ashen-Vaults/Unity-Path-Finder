using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Used for controlling when a specific component is updated.
/// </summary>
public class UpdateUtility 
{
    private double updatePeriod; //How often we update
    private long nextUpdate;     //The next time we update
    private const double offset = 10.0; //Used for offsetting updates by a few random miliseconds
    private static System.Random rand = new System.Random();

    /// <summary>
    /// The constructor takes in how long you want 
    /// to wait before we call the update
    /// </summary>
    /// <param name="updatesPerSecond"></param>
    public UpdateUtility(float updatesPerSecond)
    {
        SetUp(updatesPerSecond);
    }

    public void SetUp(float updatesPerSecond)
    {
        double randDouble = rand.NextDouble();
        nextUpdate = (long)(Time.time + randDouble) * 1000;

        if (updatesPerSecond > 0) updatePeriod = updatesPerSecond;

        else if (Double.Equals(0.0, updatesPerSecond)) updatePeriod = 0.0;

        else if (updatesPerSecond < 0) updatePeriod = -1;
    }

    /// <summary>
    /// Returns whether or not the thing we want to update
    /// is ready to be updated. 
    /// </summary>
    /// <returns>Update status</returns>
    public bool IsReady()
    {
        long currentTime = (long)Time.time;
 
        //Always Update
        if (Double.Equals(0.0,updatePeriod)) return true;

        //Never Update
        if (updatePeriod < 0) return false;

        //Update and set the time to update next
        if(currentTime >= nextUpdate)
        {
            nextUpdate = (long)(currentTime + updatePeriod + RandomDouble(-offset,offset));
            return true;
        }
        return false; //dont update
    }
    

    /// <summary>
    /// Returns a random double between two points
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <returns></returns>
    public double RandomDouble(double _x, double _y)
    {
        double randomDouble = _x + rand.NextDouble() * (_y - _x);
        return randomDouble;
    }
}

