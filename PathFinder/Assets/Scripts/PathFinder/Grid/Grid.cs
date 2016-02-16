using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace PathFinder
{
    public class Grid : MonoBehaviour
    {
        public List<Tile> myTiles;
        public Tile[,] grid;
        public LayerMask myLayer;
        public float tileSizes;
        public Vector2 size;

        float tileDiameter;
        int sizeX, sizeY;
        public int Size { get { return sizeX * sizeY; } }

        public bool showGrid;


        // Use this for initialization
        void Awake()
        {
            tileDiameter = tileSizes * 2;
            sizeX = Mathf.RoundToInt(size.x / tileDiameter);
            sizeY = Mathf.RoundToInt(size.y / tileDiameter);
            CreateGrid();    
        }

        /// <summary>
        /// Creates the grid that AI will use for pathfinding
        /// </summary>
        [ContextMenu("Bake Grid")]
        void CreateGrid()
        {
            grid = new Tile[sizeX, sizeY];
            Vector3 bottomLeftCorner = transform.position - Vector3.right * size.x / 2 - Vector3.forward * size.y / 2;


            for(int x=0; x < sizeX; x++ )
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Vector3 worldPoint = bottomLeftCorner + Vector3.right * (x * tileDiameter + tileSizes) + Vector3.forward * (y * tileDiameter + tileSizes);
                    bool walkable = !(Physics.CheckSphere(worldPoint,tileSizes,myLayer));
                    grid[x, y] = new Tile(walkable, worldPoint, x, y);
                    //myTiles.Add(grid[x, y]);
                }
            }
        }

        /// <summary>
        /// Returns the tile at a specified
        /// position
        /// </summary>
        /// <param name="_pos">the position of the tile</param>
        /// <returns></returns>
        public Tile GetTile(Vector3 _pos)
        {
            float x = (_pos.x + size.x / 2) / size.x;
            float y = (_pos.z + size.y / 2) / size.y;
            x = Mathf.Clamp01(x);
            y = Mathf.Clamp01(y);

            int posX = Mathf.RoundToInt((sizeX - 1) * x);
            int posY = Mathf.RoundToInt((sizeY - 1) * y);

            return grid[posX, posY];
        }


        /// <summary>
        /// Gets the adjacent tiles
        /// for a given tile
        /// </summary>
        /// <param name="_t"></param>
        /// <returns></returns>
        public List<Tile> GetAdjacents(Tile _t)
        {
            List<Tile> adjacents = new List<Tile>();
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = _t.gridX + x;
                    int checkY = _t.gridY + y;

                    if(checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY)
                    {
                        adjacents.Add(grid[checkX, checkY]);
                    }
                }
            }
            return adjacents;
        }


        public void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3 (size.x,1,size.y));
            if (grid != null && showGrid)
            {
                foreach (Tile t in grid)
                {
                    Gizmos.color = (t.walkable) ? Color.white : Color.red;
                    Gizmos.DrawWireCube(t.myWorldPosition, Vector3.one * (tileDiameter-0.1f));
                }
            }      
        }
    }
}