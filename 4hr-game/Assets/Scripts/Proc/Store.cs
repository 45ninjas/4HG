using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc
{
    public class Store : MonoBehaviour
    {
        public const int blockSize = 6;
        public const int blockMoves = 4;

        public GameObject[] prefabs;

        System.Random random;

        Vector2Int[] tiles;
        HashSet<Vector2Int> tileSet;

        public static Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down,
            Vector2Int.right
        };

        [SerializeField]
        public string StartingSeed = "Hello World";

        [HideInInspector]
        public int Seed;

        public void CreateStore(string seed)
        {
            CreateStore(seed.GetHashCode());
        }
        public void CreateStore()
        {
            int seed = new System.Random().Next();
            CreateStore(seed);
        }

        public void ClearStore()
        {
            // Destroy each child of the store.
            foreach (Transform child in transform)
            {
                // The editor is special. Make it have it's own food.
                if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
            }
        }

        public void CreateStore(int seed)
        {
            // Clear the old store.
            ClearStore();

            // Create a new random number generator with the provided seed.
            random = new System.Random(seed);
            Seed = seed;

            // Create a floor plan object.
            var floorPlan = new FloorPlan(blockSize, blockMoves, random);

            // Get all the tiles of this store from the floor plan.
            tiles = floorPlan.GetTiles();
            tileSet = new HashSet<Vector2Int>(tiles);

            CreateStoreWalls();
        }

        public void CreateStoreWalls()
        {
            List<Vector3Int> frontWalls = new List<Vector3Int>();

            // Get the highest (y) value.
            int northMost = int.MinValue;

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].y > northMost)
                    northMost = tiles[i].y;
            }

            Debug.Log(northMost);

            // Now that we know what's the highest value. Only add the ones to the list.

                Vector2Int tile;
            for (int i = 0; i < tiles.Length; i++)
            {
                tile = tiles[i];

                int prefab = 0;

                // If the wall is the north most wall. Make it a window wall.
                if (tile.y >= northMost)
                    prefab = 1;

                if (!tileSet.Contains(tile + directions[0]))
                    SpawnWall(tile, directions[0], prefab);
                if (!tileSet.Contains(tile + directions[1]))
                    SpawnWall(tile, directions[1], prefab);
                if (!tileSet.Contains(tile + directions[2]))
                    SpawnWall(tile, directions[2], prefab);
                if (!tileSet.Contains(tile + directions[3]))
                    SpawnWall(tile, directions[3], prefab);
            }
        }

        public void SpawnWall(Vector2Int pos, Vector2Int direction, int prefabIndex)
        {
            if(prefabIndex > prefabs.Length)
                throw new System.IndexOutOfRangeException("Prefab index is too high. Is there enough prefabs.");

            GameObject wall = Instantiate(prefabs[prefabIndex], transform, true);

            // Get the direction the wall needs to face.
            Vector3 normal = new Vector3(direction.x, 0, direction.y);

            wall.transform.position = new Vector3(pos.x, 0, pos.y) + normal * 0.5f;
            wall.transform.rotation = Quaternion.LookRotation(-normal, Vector3.up);
        }

        private void Start()
        {
            CreateStore(StartingSeed);
        }

        private void OnDrawGizmosSelected()
        {
            if (tiles != null)
            {
                foreach (var tile in tiles)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(new Vector3(tile.x, 0, tile.y), new Vector3(1, 0.01f, 1));
                    Gizmos.color = new Color(0, 1f, 0, 0.2f);
                    Gizmos.DrawCube(new Vector3(tile.x, 0, tile.y), new Vector3(1, 0.01f, 1));
                }
            }
        }

        public class FloorPlan
        {
            private int blockSize = 4;
            private int moves = 3;

            private System.Random random;

            public FloorPlan(int size, int moves, System.Random random)
            {
                blockSize = size;
                this.moves = moves;
                this.random = random;
            }

            public Vector2Int[] GetBlocks()
            {
                Vector2Int[] square = new Vector2Int[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, 0)
                };

                List<Vector2Int> blocks = new List<Vector2Int>(square);

                Vector2Int position = new Vector2Int();
                for (int i = 1; i < moves; i++)
                {
                    Vector2Int direction = new Vector2Int();

                    int fails = 10;

                    while (!SquareExists(position + direction, ref blocks) && fails > 0)
                    {
                        direction = directions[random.Next(0, 4)];
                        fails--;
                    }
                    // Pick a random direction to move the position in.

                    for (int j = 0; j < square.Length; j++)
                    {
                        if (!blocks.Contains(direction + position + square[j]))
                            blocks.Add(direction + position + square[j]);
                    }

                    position += direction;
                }

                return blocks.ToArray();
            }

            private bool SquareExists(Vector2Int position, ref List<Vector2Int> blocks)
            {
                int count = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (!blocks.Contains(position + blocks[j]))
                        count++;
                }
                return count >= 2;
            }

            public Vector2Int[] GetTiles()
            {
                var blocks = GetBlocks();

                Vector2Int[] tiles = new Vector2Int[blocks.Length * blockSize * blockSize];

                int t = 0;
                foreach (var block in blocks)
                {
                    Vector2Int pos = new Vector2Int();
                    for (pos.x = 0; pos.x < blockSize; pos.x++)
                    {
                        for (pos.y = 0; pos.y < blockSize; pos.y++, t++)
                        {
                            tiles[t] = block * blockSize + pos;
                        }
                    }
                }

                return tiles;
            }
        }
    }
}