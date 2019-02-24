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

        public Dictionary<Vector2Int, Transform> tiles;

        [SerializeField]
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

            tiles = new Dictionary<Vector2Int, Transform>();

            // Create a new random number generator with the provided seed.
            Seed = seed;
            random = new System.Random(seed);

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].Apply(this, random);
            }
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
                    Gizmos.DrawWireCube(new Vector3(tile.Key.x, 0, tile.Key.y), new Vector3(1, 0.01f, 1));
                    Gizmos.color = new Color(0, 1f, 0, 0.2f);
                    Gizmos.DrawCube(new Vector3(tile.Key.x, 0, tile.Key.y), new Vector3(1, 0.01f, 1));
                }

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(new Vector3(Bounds.center.x, 2.5f, Bounds.center.y), new Vector3(Bounds.size.x + 1, 5, Bounds.size.y + 1));

                Gizmos.DrawLine(new Vector3(Bounds.xMin, 0, Bounds.yMax), new Vector3(Bounds.xMax, 0, Bounds.yMin));
                Gizmos.DrawLine(new Vector3(Bounds.xMin, 5, Bounds.yMax), new Vector3(Bounds.xMax, 5, Bounds.yMin));
            }
        }
    }
}