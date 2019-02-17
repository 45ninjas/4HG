using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
            if (EditorGUI.EndChangeCheck())
            {
                PreviewStore(store);
            }

            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("Preview", EditorStyles.toolbarButton))
            {
                PreviewStore(store);
            }
            if (GUILayout.Button("Generate", EditorStyles.toolbarButton))
            {
                store.CreateStore(store.StartingSeed);
            }
            float height = EditorGUIUtility.currentViewWidth - 21;

            GUILayout.EndHorizontal();

            GUILayout.Label("Preview", pic, GUILayout.ExpandWidth(true), GUILayout.Height(height));
        }

        void PreviewStore(Store store)
        {
            const int size = 64;
            var preview = new Texture2D(size, size);
            preview.filterMode = FilterMode.Point;
            var floorPlan = new Store.FloorPlan(Store.blockSize, Store.blockMoves, new System.Random(store.StartingSeed.GetHashCode()));
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