using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefab;
    public Unit badboy;

    public static void SpawnAtPos(GameObject _obj, Vector3 _pos)
    {
        Instantiate(_obj, _pos, Quaternion.identity);
    }


    [ContextMenu("SPAWN")]
    public void SpawnTest()
    {
        Vector3 blockade = Vector3.zero;
        for(int i=0;i<=badboy.path.Length-1;i++)
        {
            if(badboy.path[i] == badboy.currentWaypoint)
            {
                if(badboy.path.Length <= i + 1)
                {
                    blockade =  badboy.path[i+1];
                }
            }
        }
        SpawnAtPos(prefab,  blockade);
    }

}
