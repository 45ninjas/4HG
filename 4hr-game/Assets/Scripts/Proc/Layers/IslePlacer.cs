using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc.Layers
{
    [CreateAssetMenu(menuName = "Proc Layer/Isle Placer", fileName = "Isle Placer")]
    public class IslePlacer: ProcLayer
    {
        public int Spacing = 2;
        public GameObject ShelfPrefab;

        Vector2Int direction;

        List<List<Vector2Int>> lines;

        private System.Random random;

        public override void Apply(Store store, System.Random random)
        {
            this.random = random;
            direction = ProcHelper.RandomDirection(random);

            lines = new List<List<Vector2Int>>();

        }
    }
}