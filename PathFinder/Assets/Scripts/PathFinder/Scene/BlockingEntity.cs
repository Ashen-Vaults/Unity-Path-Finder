using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathFinder;

public class BlockingEntity : MonoBehaviour
{
    public Graph grid;
    public List<Node> nodes;

    void OnEnable()
    {
        grid = GameObject.Find("PathFinder").GetComponent<Graph>();

        Node node = grid.GetNode(this.transform.position);
        nodes  = grid.GetAdjacents(node);
        nodes.Add(node);

        foreach(Node n in nodes)
        {
            print(n.myWorldPosition);
            n.walkable = false;
        }
        
    }
}
