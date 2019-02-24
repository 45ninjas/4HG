using UnityEngine;

namespace NineFive.Proc
{
    static class ProcHelper
    {
        public static Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        public static Vector2Int RandomDirection(System.Random rnd)
        {
            return directions[rnd.Next(0, 4)];
        }
    }
}
