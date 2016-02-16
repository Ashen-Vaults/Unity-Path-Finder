using UnityEngine;
using System.Collections;

public class ChangeAIUpdate : MonoBehaviour
{
    public float updateTime;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + " AI ENTER " + other.name);
        AI _ai;
        if(other.GetComponent<AI>() != null)
        {
           _ai = other.GetComponent<AI>();
           _ai.updater.visionUpdate.SetUp(updateTime);
           _ai.updater.targetUpdate.SetUp(updateTime);
           _ai.updater.selectGoalUpdate.SetUp(updateTime);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log(this.name + " AI EXIT " + other.name);
        AI _ai;
        if (other.GetComponent<AI>() != null)
        {
            _ai = other.GetComponent<AI>();
            _ai.updater.visionUpdate.SetUp(updateTime);
            _ai.updater.targetUpdate.SetUp(updateTime);
            _ai.updater.selectGoalUpdate.SetUp(updateTime);
        }
    }
}
