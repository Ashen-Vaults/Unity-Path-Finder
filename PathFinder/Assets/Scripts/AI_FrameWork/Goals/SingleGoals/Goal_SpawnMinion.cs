using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Goal_SpawnMinion : Goal
{
    public SpawnProps spawnProps;

    public override void Activate()
    {
        this.myProperties.myStatus = GoalProps.goalStatus.ACTIVE;
    }

    public override void Terminate()
    {
        this.myProperties.myStatus = GoalProps.goalStatus.INACTIVE;
    }

    public override GoalProps.goalStatus Process()
    {
        this.ActivateIfInactive();
        this.ReactivateIfFailed();


        if (this.spawnProps.currentWave < this.spawnProps.numWaves)
        {
            //Spawn more after X seconds
            if (this.spawnProps.settings == SpawnProps.SpawnSetting.AFTER_TIME)
            {
                StartCoroutine(SpawnAfterTime());
            }

            //Spawn after all enemies in wave are dead
            else if (this.spawnProps.settings == SpawnProps.SpawnSetting.AFTER_REMAINING_LEFT)
            {
                if (this.MinionsLeft() == false)
                {
                    this.spawnProps.activeMinions.Clear();
                    StartCoroutine(Spawn());
                }
            }
        }
        else
        {
            this.myProperties.myStatus = GoalProps.goalStatus.COMPLETED;
        }

        return this.myProperties.myStatus;
    }

    /// <summary>
    /// Checks to see if any of the spawned
    /// minions are still alive
    /// </summary>
    /// <returns></returns>
    public bool MinionsLeft()
    {
        bool minionsLeft = false;
        for (int i = 0; i <= this.spawnProps.activeMinions.Count - 1; i++)
        {
            if (this.spawnProps.activeMinions[i].activeSelf)
            {
                minionsLeft = true;
            }
        }
        return minionsLeft;
    }

    IEnumerator Spawn()
    {
        //TEMP
        //TODO: Add in Dannys spawning stuff to replace this below
        for (int i = 1; i <= this.spawnProps.waveAmount; i++)
        {
            GameObject newMinion = Instantiate(this.spawnProps.minion, new Vector3(this.transform.position.x + UnityEngine.Random.Range(-5, 5), this.transform.position.y, this.transform.position.z + UnityEngine.Random.Range(-5, 5)), Quaternion.identity) as GameObject;
            newMinion.transform.parent = GameObject.FindGameObjectWithTag("EnemyContainer").transform;
            this.spawnProps.activeMinions.Add(newMinion);
            yield return new WaitForSeconds(this.spawnProps.timeBetweenSpawns);
        }
        this.spawnProps.currentWave += 1;
        
    }

    IEnumerator SpawnAfterTime()
    {
        StartCoroutine(Spawn());
        yield return new WaitForSeconds(this.spawnProps.timeBetweenWaves);
    }

    public override float CalculateUtility()
    {
        float utility = 1f;

        if (!this.myProperties.myOwner.myProperties.myTargetting.TargetInFOV())
        {
            utility *= this.myProperties.myOwner.myProperties.myBrain.myPersonality.defendBias;
        }

        return utility;
    }    
}

[Serializable]
public class SpawnProps
{
    public int numWaves;
    public int waveAmount;
    public int currentWave = 0;
    public float timeBetweenWaves;
    public float timeBetweenSpawns;
    public GameObject minion;
    public List<GameObject> activeMinions;
    public enum SpawnSetting {AFTER_TIME, AFTER_REMAINING_LEFT}
    public SpawnSetting settings;
}
