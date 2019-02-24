﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc.Layers
{
    [CreateAssetMenu(menuName = "Proc Layer/floor plan", fileName = "floor plan")]
    public class FloorPlan: ProcLayer
    {
        public int BlockSize = 4;
        public int Moves = 3;

        private System.Random random;

        public override void Apply(Store store, System.Random random)
        {
            this.random = random;

            var tiles = GetTiles();

            // Add blank tiles to the Store
            foreach (var tile in tiles)
                store.tiles.Add(tile, null);
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
            for (int i = 1; i < Moves; i++)
            {
                Vector2Int direction = new Vector2Int();

                int fails = 10;

                while (!SquareExists(position + direction, ref blocks) && fails > 0)
                {
                    direction = ProcHelper.directions[random.Next(0, 4)];
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

            Vector2Int[] tiles = new Vector2Int[blocks.Length * BlockSize * BlockSize];

            int t = 0;
            foreach (var block in blocks)
            {
                Vector2Int pos = new Vector2Int();
                for (pos.x = 0; pos.x < BlockSize; pos.x++)
                {
                    for (pos.y = 0; pos.y < BlockSize; pos.y++, t++)
                    {
                        tiles[t] = block * BlockSize + pos;
                    }
                }
            }

            return tiles;
        }
    }
}