using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Product", menuName = "Create Product")]
public class ProductDefinition : ScriptableObject
{
    // The title of the product.
    public string Title;
    // Other names this product could have.
    public string[] Alias;

    // The price of this product.
    public float Price;

    // The prefab used for this product.
    public GameObject Prefab;

    // This will be set by an icon generation script while the game is loading.
    [HideInInspector]
    public Sprite Icon;
}