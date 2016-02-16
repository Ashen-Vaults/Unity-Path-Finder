using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace PathFinder
{
    public class AStar : PathAlgorithm
    {
        //  public List<Tile> open;   //Set of tiles that need to be evaluated
        //  public List<Tile> closed; //Set of tiles that have been evaluated

    //    public Transform seeker, target;

        public int tentativeGScore;
        public PathMananger myMananger;
        public Stopwatch sw;
        public bool distort;
        public AnimationCurve distortion;

        Grid grid;

        void Awake()
        {
            grid = GetComponent<Grid>();
        }

        void Update()
        {
          //  if (Input.GetButtonDown("Jump")) 
        //    FindPath(seeker.position, target.position);
        }

        public override void StartFindPath(Vector3 startPos, Vector3 targetPos, DistanceHeuristic distanceType, bool simplified)
        {
            StartCoroutine(FindPath(startPos, targetPos, distanceType, simplified));    
        }

        public override IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, DistanceHeuristic distanceType, bool simplified)
        {

            sw = new Stopwatch();
            sw.Start();

            #region Init
            Tile startTile = grid.GetTile(startPos);
            Tile targetTile = grid.GetTile(targetPos);

            Vector3[] wayPoints = new Vector3[0];
            bool success = false;

            //List<Tile> open = new List<Tile>();

            PriorityQueue<Tile> open = new PriorityQueue<Tile>(this.myProperties.myGrid.Size);

            HashSet<Tile> close = new HashSet<Tile>();
            open.Enqueue(startTile);  //Add the starting tile to be processed
            #endregion Init

            //if (startTile.walkable) {

                while (open.Count > 0)
                {
                    Tile currentTile = open.Dequeue(); //Set the currentTile to the next elem in open

                    //If we got to the target, the create the path to it
                    //and exit the loop
                    if (currentTile == targetTile)
                    {
                        sw.Stop();
                        //print(sw.ElapsedMilliseconds + " ms");
                        success = true;
                        break;
                    }


                    close.Add(currentTile);

                    //
                    foreach (Tile adjacent in grid.GetAdjacents(currentTile))
                    {

                        //Ignore the adjacent neightbor which is already evaluated
                        if (!adjacent.walkable || close.Contains(adjacent)) continue;

                        //Length of this path
                        tentativeGScore = currentTile.gScore + this.GetDistance(currentTile, adjacent, distanceType);

                        //Find new tiles
                        if (tentativeGScore < adjacent.gScore || !open.Contains(adjacent))
                        {
                            adjacent.gScore = tentativeGScore;
                            adjacent.hScore = this.GetDistance(adjacent, targetTile, distanceType);
                            adjacent.myParent = currentTile;

                            if (!open.Contains(adjacent))
                                open.Enqueue(adjacent);
                            else
                                open.UpdateElement(adjacent);
                        }
                    }
                }
                yield return new WaitForSeconds(0.0000001f);
                if (success)
                {
                    wayPoints = CreatePath(startTile, targetTile, simplified);
                }
                myMananger.DoneProcessing(wayPoints, success);
            //}
        }

        /// <summary>
        /// Creates the final path after it is found
        /// </summary>
        /// <param name="startTile"></param>
        /// <param name="endTile"></param>
        public override Vector3[] CreatePath(Tile startTile, Tile endTile, bool simplified)
        {
            List<Tile> path = new List<Tile>();
            Tile currentTile = endTile;

            //Constructs the path by starting at the target position
            //and getting the parent of each tile until it gets to the start
            while (currentTile != startTile)
            {
                path.Add(currentTile);
                currentTile = currentTile.myParent;
            }
            path.Add(startTile);
            Vector3[] simplifiedPath = Simplify(path, simplified, distort);
            Array.Reverse(simplifiedPath);
            return simplifiedPath;
        }

        Vector3[] Simplify(List<Tile> _path, bool _simplifyPath, bool _distort)
        {
            List<Vector3> wayPoints = new List<Vector3>();
                Vector2 direction = Vector2.zero;

                for (int i = 1; i < _path.Count; i++)
                {
                    if (_simplifyPath)
                    {
                        Vector2 newDirection = new Vector2(_path[i - 1].gridX - _path[i].gridX, _path[i - 1].gridY - _path[i].gridY);
                        if (newDirection != direction) wayPoints.Add(_path[i - 1].myWorldPosition);
                        direction = newDirection;
                    }

                    else if (_distort)
                    {
                           
                    }

                    else
                    {
                        wayPoints.Add(_path[i].myWorldPosition);
                    }

                }
            return wayPoints.ToArray();
        }


        Vector3[] Distort(List<Tile> _path)
        {
            List<Vector3> wayPoints = new List<Vector3>();

            for (int i = 0; i <= wayPoints.Count - 1; i++)
            {

            }

            return wayPoints.ToArray();
        }

        void SmoothPath(List<Tile> _path)
        {
            int start = 0;
            int next = 1;
            Tile _start = _path[start];
            Tile _end = _path[next];

            while (_end != _path[_path.Count - 1])
            {
                if (_start.walkable)
                {
                    _start = _end;
                    _path.Remove(_end);
                }
                else
                {
                    _start = _end;
                    _end = _path[next++];
                }
            }
        }

        /// <summary>
        /// Gets the distance between two tiles
        /// Uses Diagonal Distance for calculating 
        /// the distance
        /// </summary>
        /// <param name="_currentTile"></param>
        /// <param name="_endTile"></param>
        /// <returns></returns>
        public int GetDistance(Tile _currentTile, Tile _endTile, DistanceHeuristic _distanceType)
        {
            int dx = Mathf.Abs(_currentTile.gridX - _endTile.gridX);
            int dy = Mathf.Abs(_currentTile.gridY - _endTile.gridY);


            ///////DEFUALT////////
            //Square Grid that allows 8 directions of movement
            if (_distanceType == DistanceHeuristic.DIAGONAL)
            {
                //3/2 are used for calculating the diagonal distance
                if (dx > dy)
                    return 14 * dy + 10 * (dx - dy);
                return 14 * dx + 10 * (dy - dx);
            }

            else

            //Square Grid that allows 4 directions of movement
            //UP, DOWN, LEFT, RIGHT
            if (_distanceType == DistanceHeuristic.MANHATTAN)
            {
                return 14 * (dx + dy);
            }



            else

            //Square grid that allows any direction of movement
            //NOT restricted to a grid, **SLOWER
            if (_distanceType == DistanceHeuristic.EUCLIDEAN)
            {
                return (int)(14 * Mathf.Sqrt(dx * dx + dy * dy));
            }

            return 0;
        }

        /*
       public List<Tile> ReversePath(List<Tile> _path)
       {
           if (_path == null) return null;
           else
           if (_path[1] == null) return _path;
           else
           {
               List<Tile> nextTile = _path[1];
               _path[1] = null;

               List<Tile> reversedPath = ReversePath(nextTile);

           }
       }
        }    */
    }
}
