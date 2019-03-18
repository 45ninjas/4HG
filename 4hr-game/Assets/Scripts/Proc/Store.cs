using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NineFive.Proc.Layers;

namespace NineFive.Proc
{
    public class Store : MonoBehaviour
    {
        public ProcLayer[] layers;
        System.Random random;

#if UNITY_EDITOR
        const int lineCount = 10;
#endif

        public Dictionary<Vector2Int, TileData> tiles;

        [HideInInspector]
        public string StartingSeed = "Hello World";

        [HideInInspector]
        public int Seed;

        [HideInInspector]
        public RectInt Bounds;

        public void CreateStore(string seed)
        {
            CreateStore(seed.GetHashCode());
        }
        public void CreateStore()
        {
            CreateStore(System.Environment.TickCount);
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

            tiles = new Dictionary<Vector2Int, TileData>();

            // Create a new random number generator with the provided seed.
            Seed = seed;
            random = new System.Random(seed);

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].Apply(this, random);
            }

#if UNITY_EDITOR

            string[] lines = null;

            try
            {
                lines = System.IO.File.ReadAllLines("last-seeds.txt", System.Text.Encoding.UTF8);
            }
            catch (System.Exception)
            {

            }


            List<string> newLines = new List<string>();
            newLines.Add(seed.ToString());

            if (lines != null)
            {
                for (int i = 0; i < lineCount - 1; i++)
                {
                    if (i < lines.Length)
                        newLines.Add(lines[i]);
                }
            }

            newLines.Add(string.Format("These where the last {0} seeds to be generated. In Dec-ending order.", lineCount));

            System.IO.File.WriteAllLines("last-seeds.txt", newLines);
#endif
        }

        private void Start()
        {
            if (string.IsNullOrWhiteSpace(StartingSeed))
                CreateStore();
            else
                CreateStore(StartingSeed);
        }

        private void OnDrawGizmosSelected()
        {
            if (tiles != null)
            {
                foreach (var tile in tiles)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(new Vector3(tile.Key.x, 0, tile.Key.y), new Vector3(1, 0.01f, 1));
                    Gizmos.color = new Color(0, 1f, 0, 0.2f);
                    Gizmos.DrawCube(new Vector3(tile.Key.x, 0, tile.Key.y), new Vector3(1, 0.01f, 1));
                }

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(new Vector3(Bounds.center.x, 2.5f, Bounds.center.y), new Vector3(Bounds.size.x + 1, 5, Bounds.size.y + 1));

                Gizmos.DrawLine(new Vector3(Bounds.xMin, 0, Bounds.yMax), new Vector3(Bounds.xMax, 0, Bounds.yMin));
                Gizmos.DrawLine(new Vector3(Bounds.xMin, 5, Bounds.yMax), new Vector3(Bounds.xMax, 5, Bounds.yMin));
            }

            foreach (ProcLayer layer in layers)
            {
                layer.DrawGizmos();
            }
        }
    }
}