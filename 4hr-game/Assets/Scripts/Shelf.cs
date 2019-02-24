using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public float UpperLimit = 2f;
    public float LowerLimit = 0.2f;

    public float MinShelfHeight = 0.2f;

    [SerializeField]
    private Transform shelfChild;

    //[HideInInspector]
    public float ShelfSpacing;
    //[HideInInspector]
    public float OverallHeight;

    //[HideInInspector]
    public int MaximumShelves;

    public Dictionary<Transform, object> shelves;

    // Start is called before the first frame update
    void Start()
    {
        ReconfigureShelves();
    }

    [ContextMenu("Reconfigure Shelves")]
    public void ReconfigureShelves()
    {
        OverallHeight = UpperLimit - LowerLimit;
        MaximumShelves = Mathf.FloorToInt(OverallHeight / MinShelfHeight);

        ShelfSpacing = OverallHeight / MaximumShelves;

        if (shelves != null)
        {
            // First, delete all the children shelves.
            Transform[] transforms = new Transform[shelves.Count];
            shelves.Keys.CopyTo(transforms, 0);

            for (int i = 0; i < transforms.Length; i++)
            {
                shelves.Remove(transforms[i]);
                if (transforms[i] != shelfChild)
                    Destroy(transforms[i].gameObject);
            }
        }
        else
            shelves = new Dictionary<Transform, object>();

        // Add the shelf child.
        AddShelf(0, shelfChild);

        for (int i = 1; i < MaximumShelves; i++)
        {
            AddShelf(i);
        }
    }
    void AddShelf(int index, Transform transform = null)
    {
        if(transform == null)
            transform = Instantiate(shelfChild.gameObject, this.transform, false).transform;

        shelves.Add(transform, null);

        Vector3 pos = transform.localPosition;

        pos.y = LowerLimit + index * ShelfSpacing;

        transform.localPosition = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(Vector3.up * (((UpperLimit - LowerLimit) / 2) + LowerLimit), new Vector3(1f, UpperLimit - LowerLimit, 1f));
    }
}
