using UnityEngine;
using System.Collections;

public class AIMananger : MonoBehaviour 
{
    #region Events
    public delegate void AIAction();
    public static event AIAction OnSeekPlayer;
    public static event AIAction OnKillAll;
    #endregion Events


    [ContextMenu("Seek Player")]
    public void AllSeekPlayer()
    {
        if (OnSeekPlayer != null)
            OnSeekPlayer();
    }

    [ContextMenu("KIll All Enemies")]
    public void AllDie()
    {
        if (OnKillAll != null)
            OnKillAll();
    }
}
