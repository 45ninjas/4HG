using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc
{
    public class FloorPlan: ProcLayer
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

        public override void Apply(Store store, System.Random random)
        {
            throw new NotImplementedException();
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
                    direction = Store.directions[random.Next(0, 4)];
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