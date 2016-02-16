using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace PathFinder
{
    public class Node : IQueueable
    {
        public bool walkable;
        public Vector2 myPosition2D;
        public Vector3 myWorldPosition;

        #region GridProperties
        public int gridX, gridY; //Positions in the grid
        public int gScore = 0; // Cost it took to get to this tile from the start
        public int hScore = 0; //is a heuristic estimate of the cost to get from n to any goal node.                
        public int fScore  // Estimated total cost from start to goal through; determines which tile gets put into the closed list.
        {
            get { return gScore + hScore; }
        }

        public Node myParent;
        #endregion GridProperties;


        #region IQueueable Properties

        int priority;
        public int Priority
        {
            get { return fScore; }
        }

        int queueIndex;
        public int QueueIndex
        {
            get { return queueIndex;  }
            set { queueIndex = value; }
        }

        int insertionIndex;
        public int InsertionIndex
        {
            get { return insertionIndex; }
            set { insertionIndex = value; }
        }


        public int CompareTo(Node _t)
        {
            int value = fScore.CompareTo(_t.fScore);
            if (value == 0) value = hScore.CompareTo(_t.hScore);
            return -value;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Node)obj);    
        }
        #endregion IQueueable Properties


        public Node(bool _walkable, Vector3 _pos)
        {
            this.walkable = _walkable;
            this.myWorldPosition = _pos;
        }

        /// <summary>
        /// The main Constructor which is called
        /// </summary>
        /// <param name="_walkable">Whether or not it will be a considered tile when a path is created</param>
        /// <param name="_pos">the world position</param>
        /// <param name="_x">the x position in the grid</param>
        /// <param name="_y">the y position in the grid</param>
        public Node(bool _walkable, Vector3 _pos, int _x, int _y)
        {
            this.walkable = _walkable;
            this.myWorldPosition = _pos;
            gridX = _x;
            gridY = _y;

        }

    }
}

