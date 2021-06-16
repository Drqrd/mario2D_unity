using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInteractableBlock : MonoBehaviour
{
    [Range(1, 4), Header("Dimensions")]
    public int x = 1;
    [Range(1,4)]
    public int y = 1;

    [Header("Type")]
    public bool breakableOnly;
    public bool custom;

    [Header("Objects")]
    public BoxCollider bc;

    [Header("Prefabs")]
    public GameObject breakableTop;
    public GameObject breakable;
    public GameObject question;
    public GameObject hit;

    private List<GameObject> blocks = new List<GameObject>();

    private void OnValidate()
    {
        UpdateColliders();
        UpdateBlocks();
    }

    void UpdateColliders()
    {
        Debug.Log("Colliders Running");
        bc.size = new Vector2(x, y);
    }

    void UpdateBlocks()
    {
        Debug.Log(blocks.Count);
        if (blocks.Count < x * y)
        {
            for (int i = blocks.Count; blocks.Count < x * y; i++)
            {
                Debug.Log(i);
            }
        }
    }
}
