using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NineFive.Proc.Layers;

namespace NineFive.Proc
{
    [CustomEditor(typeof(Store))]
    public class ProcStoreEditor : Editor
    {
        GUIStyle pic;

        public override void OnInspectorGUI()
        {
            if (pic == null)
            {
                pic = new GUIStyle();
                pic.normal.background = null;
            }
            Store store = (Store)target;

            // Draw the standard UI.
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();

            bool useSeedBefore = !string.IsNullOrEmpty(store.StartingSeed);
            bool useSeed = EditorGUILayout.Toggle("Use Seed", useSeedBefore);

            if (useSeed != useSeedBefore)
            {
                if (useSeed == false)
                    store.StartingSeed = null;
                else if (useSeed)
                    store.StartingSeed = System.Environment.TickCount.ToString();
            }

            EditorGUI.BeginDisabledGroup(!useSeed);
            store.StartingSeed = EditorGUILayout.TextField("Seed", store.StartingSeed);
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                    PreviewStore(store);
            }

            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Preview", EditorStyles.toolbarButton))
            {
                if(!Application.isPlaying)
                    PreviewStore(store);
            }
            float height = EditorGUIUtility.currentViewWidth - 21;

            GUILayout.EndHorizontal();

            GUILayout.Label("Preview", pic, GUILayout.ExpandWidth(true), GUILayout.Height(height));
        }

        void PreviewStore(Store store)
        {
            FloorPlan floorPlan = null;

            foreach (var layer in store.layers)
            {
                if ((FloorPlan)layer != null)
                {
                    floorPlan = (FloorPlan)layer;
                    break;
                }
            }

            if (floorPlan == null)
                return;

            const int size = 128;
            var preview = new Texture2D(size, size);
            preview.filterMode = FilterMode.Point;

            store.tiles = new Dictionary<Vector2Int, TileData>();

            System.Random rnd;

            if (string.IsNullOrEmpty(store.StartingSeed))
                rnd = new System.Random(System.Environment.TickCount);
            else
                rnd = new System.Random(store.StartingSeed.GetHashCode());

            floorPlan.Apply(store, rnd);
            var tiles = floorPlan.GetTiles();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    preview.SetPixel(x, y, Color.black);
                }
            }

            foreach (var tile in tiles)
            {
                preview.SetPixel(tile.x + (size / 2), tile.y + (size / 2), Color.green);
            }

            preview.Apply();
            pic.normal.background = preview;
        }
    }
}