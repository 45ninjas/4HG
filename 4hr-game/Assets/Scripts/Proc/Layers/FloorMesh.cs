using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc.Layers
{
    [CreateAssetMenu(menuName = "Proc Layer/Floor Mesh", fileName = "Floor mesh layer")]
    public class FloorMesh : ProcLayer
    {
        Store store;

        Mesh floor;
        public int totalFloorTypes = 9;
        public int atlasPowSize = 3;

        [Range(0f, 1.5f)]
        public float perlinFrequency = 0.08f;

        public bool randomRotation = false;

        List<Vector3> vertices;
        List<Vector2> uvs;
        List<Vector3> normals;
        List<int> triangles;

        public Material floorMaterial;

        int vertIndex, triIndex;

        Vector2Int[] uvCorners = new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(0,1),
            new Vector2Int(1,1),
            new Vector2Int(1,0)
        };

        private Vector2Int GetUvCorner(int index)
        {
            index = index % 4;

            return uvCorners[index];
        }

        public override void Apply(Store store, System.Random random)
        {
            this.store = store;
            int materialA = random.Next(0, totalFloorTypes);
            int materialB = random.Next(0, totalFloorTypes);

            vertIndex = 0;
            triIndex = 0;

            vertices = new List<Vector3>();
            uvs = new List<Vector2>();
            triangles = new List<int>();
            normals = new List<Vector3>();

            Vector2Int perlinOffset = new Vector2Int(random.Next(-2000, 2000), random.Next(-2000, 2000));

            foreach (var tile in store.tiles)
            {
                // Randomly choose a floor type.
                float perlin = Mathf.PerlinNoise((tile.Key.x + perlinOffset.x) * perlinFrequency, (tile.Key.y + perlinOffset.y) * perlinFrequency);
                if (perlin > 0.5f)
                {
                    tile.Value.FloorType = materialA;
                }
                else
                    tile.Value.FloorType = materialB;

                if(randomRotation)
                    tile.Value.FloorRotation = random.Next(0, 4);

                AddQuad(tile.Key);
            }


            // Create the mesh.
            floor = new Mesh();
            floor.name = "Proc Store Floor";

            floor.SetVertices(vertices);
            floor.SetTriangles(triangles, 0);
            floor.SetNormals(normals);
            floor.SetUVs(0, uvs);

            floor.RecalculateBounds();
            floor.RecalculateTangents();

            // Create a game object to render the floor.
            CreateGameObject(store.Bounds, store.transform);
        }

        private void CreateGameObject(RectInt storeBounds, Transform parent)
        {
            GameObject floorGameObject = new GameObject(
                "Proc Store Floor",
                typeof(MeshFilter),
                typeof(MeshRenderer),
                typeof(BoxCollider)
            );

            floorGameObject.transform.SetParent(parent, true);

            floorGameObject.GetComponent<MeshFilter>().sharedMesh = floor;

            var renderer = floorGameObject.GetComponent<MeshRenderer>();
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.material = floorMaterial;

            var boxCollider = floorGameObject.GetComponent<BoxCollider>();
            boxCollider.center = new Vector3(storeBounds.center.x, -1f, storeBounds.center.y);
            boxCollider.size = new Vector3(storeBounds.width + 1, 2f, storeBounds.height + 1);
        }

        private void AddQuad(Vector2Int pos)
        {
            // Place the vertices.
            vertices.Add(new Vector3(pos.x - 0.5f, 0, pos.y - 0.5f));
            vertices.Add(new Vector3(pos.x - 0.5f, 0, pos.y + 0.5f));
            vertices.Add(new Vector3(pos.x + 0.5f, 0, pos.y + 0.5f));
            vertices.Add(new Vector3(pos.x + 0.5f, 0, pos.y - 0.5f));

            int floorType = store.tiles[pos].FloorType;
            int floorRotation = store.tiles[pos].FloorRotation;

            // Get the position of the floor texture.
            Vector2Int uvTile = new Vector2Int(floorType % atlasPowSize, floorType / atlasPowSize);

            // Add a uv and normal for each new vertex.
            for (int i = 0; i < 4; i++)
            {
                // Convert the UV corner to a vector2 and devide it by the size to reduce the uv space to 0-1.
                uvs.Add((Vector2)(uvTile + GetUvCorner(i + floorRotation)) / atlasPowSize);

                // Up normal, the floor is down therefore sky is up.
                normals.Add(Vector3.up);
            }

            // Add the two triangles required to create a quad.
            triangles.Add(vertIndex + 0);
            triangles.Add(vertIndex + 1);
            triangles.Add(vertIndex + 2);

            triangles.Add(vertIndex + 0);
            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 3);

            vertIndex += 4;
            triIndex += 6;
        }
    }
}