using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc.Layers
{
    [CreateAssetMenu(menuName = "Proc Layer/Isle Placer", fileName = "Isle Placer")]
    public class IslePlacer: ProcLayer
    {
        public int Spacing = 1;
        public GameObject ShelfPrefab;

        Vector2Int forwardDir;
        int forwardSize;

        Vector2Int sidewaysDir;
        int sidewaysSize;

        List<List<Vector2Int>> lines;

        private Store store;

        private System.Random random;

        public override void Apply(Store store, System.Random random)
        {
            this.random = random;
            this.store = store;

            // Get the random directions.
            RandomDirections();

            lines = new List<List<Vector2Int>>();

            int lineCount = sidewaysSize / (1 + Spacing);

            for (int i = 0; i < lineCount; i++)
            {
                int x = i * (1 + Spacing);
            }
        }

        /// <summary>
        /// Sets the forwardDir and sidewaysDir of the store.
        /// </summary>
        void RandomDirections()
        {
            // Set a random forward direction.
            int dirIndex = ProcHelper.RandomDirectionIndex(random);

            // Set the forward size and direction.
            forwardDir = ProcHelper.directions[dirIndex];
            forwardSize = GetSize(dirIndex);

            // Increase the dirIndex by one, if it's out of range for the direction
            // array, loop it back to 0.
            dirIndex++;
            if (dirIndex > 3)
                dirIndex = 0;

            // Set the sideways size and direction.
            sidewaysDir = ProcHelper.directions[dirIndex];
            sidewaysSize = GetSize(dirIndex);
        }

        /// <summary>
        /// Get the size of the store in the specified direction.
        /// </summary>
        /// <param name="direction">The direction we need the size of</param>
        /// <returns>Size of the store in the direction.</returns>
        int GetSize(int direction)
        {
            if (direction == 0 || direction == 2)
                return store.Bounds.height;
            else if (direction == 1 || direction == 3)
                return store.Bounds.width;
            else
                throw new ArgumentException("The specified direction is not a valid direction.");
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, new Vector3(sidewaysDir.x, 0, sidewaysDir.y));

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, new Vector3(forwardDir.x, 0, forwardDir.y));
        }
    }
}