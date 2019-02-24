using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc.Layers
{
    [CreateAssetMenu(menuName = "Proc Layer/Wall Placer", fileName = "wall placer")]
    public class WallPlacer : ProcLayer
    {
        public GameObject WallPrefab;
        public GameObject WindowPrefab;

        private Transform wallParent;

        public override void Apply(Store store, System.Random random)
        {
            wallParent = new GameObject("Walls").transform;
            wallParent.SetParent(store.transform);

            List<Vector3Int> frontWalls = new List<Vector3Int>();

            // Get the highest (y) value.
            int northMost = int.MinValue;

            foreach (var pos in store.tiles.Keys)
            {
                if (pos.y > northMost)
                    northMost = pos.y;
            }

            // Now that we know what's the highest value. Only add the ones to the list.

            foreach (var tile in store.tiles.Keys)
            {
                GameObject prefab = WallPrefab;

                // If the wall is the north most wall. Make it a window wall.
                if (tile.y >= northMost)
                    prefab = WindowPrefab;

                // Up, north (+z)
                if (!store.tiles.ContainsKey(tile + ProcHelper.directions[0]))
                    SpawnWall(tile, ProcHelper.directions[0], prefab);

                // right, east (+x)
                if (!store.tiles.ContainsKey(tile + ProcHelper.directions[1]))
                    SpawnWall(tile, ProcHelper.directions[1], prefab);

                // down, south (-z)
                if (!store.tiles.ContainsKey(tile + ProcHelper.directions[2]))
                    SpawnWall(tile, ProcHelper.directions[2], prefab);

                // left, west (-x)
                if (!store.tiles.ContainsKey(tile + ProcHelper.directions[3]))
                    SpawnWall(tile, ProcHelper.directions[3], prefab);
            }
        }

        void SpawnWall(Vector2Int pos, Vector2Int direction, GameObject prefab)
        {
            GameObject wall = Instantiate(prefab, wallParent, true);

            // Get the direction the wall needs to face.
            Vector3 normal = new Vector3(direction.x, 0, direction.y);

            wall.transform.position = new Vector3(pos.x, 0, pos.y) + normal * 0.5f;
            wall.transform.rotation = Quaternion.LookRotation(-normal, Vector3.up);
        }
    }
}