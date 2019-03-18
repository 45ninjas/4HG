using UnityEngine;

namespace NineFive.Proc
{
    static class ProcHelper
    {
        /// <summary>
        /// Directions in an easy to use array. Clockwise starting at up.
        /// </summary>
        public static Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        /// <summary>
        /// Gets a random direction from the directions array.
        /// </summary>
        /// <param name="rnd">The random object to use.</param>
        /// <returns>random direction from the directions array.</returns>
        public static Vector2Int RandomDirection(System.Random rnd)
        {
            return directions[rnd.Next(0, 4)];
        }

        /// <summary>
        /// Gets a random index for the directions array.
        /// </summary>
        /// <param name="rnd">The random object to use.</param>
        /// <returns>random index for the directions array.</returns>
        public static int RandomDirectionIndex(System.Random rnd)
        {
            return rnd.Next(0, 4);
        }
    }
}
