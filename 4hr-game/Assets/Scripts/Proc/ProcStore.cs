using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc
{
    public class ProcStore : MonoBehaviour
    {
        System.Random random;

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

        public void CreateStore(int seed)
        {
            random = new System.Random(seed);
            Seed = seed;
        }

        private void Start()
        {
            CreateStore(10220);
        }

        public class FloorPlan
        {
            public int blockSize = 4;

            public int Moves = 3;

            public IEnumerable<Vector2Int> GetTiles(System.Random random)
            {
                Vector2Int[] square = new Vector2Int[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, 0)
                };
                //Vector2Int[] blocks = new Vector2Int[blockCount];
                // From the starting block, randomly place blocks to their neighbors.
                for (int i = 1; i < Moves; i++)
                {
                    
                }

                return square;
            }
        }
    }
}