using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInteractableBlock : MonoBehaviour
{
    [Range(1, 4), Header("Dimensions")]
    public int x = 1;
    [Range(1, 4)]
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

    [Header("Debug")]
    public bool dontUpdate;

    private List<GameObject> blocks;

    public void OnValidate() 
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }

    // Update when object is changed in editor
#if UNITY_EDITOR
    void _OnValidate()
    {
        if (!dontUpdate)
        {
            UpdateObject();
            UpdateBlocks();
        }
    }
#endif

    void UpdateObject()
    {
        bc.size = new Vector2(x, y);
    }

    void UpdateBlocks()
    {   
        if (blocks.Count < x * y)
        {
            for (int i = blocks.Count; blocks.Count < x * y; i++)
            {
                Vector2 pos;
                pos.x = i % x;
                pos.y = Mathf.Floor(i / y);

                if (custom)
                {
                    GameObject obj = new GameObject("Custom " + (i + 1));
                    obj.transform.parent = transform;
                    obj.transform.localPosition = pos;
                }

                if (breakableOnly)
                {
                    GameObject obj = Instantiate(breakableTop);
                    obj.transform.parent = transform;
                    obj.transform.localPosition = pos;
                }
            }
        }

        // If size reduced, delete blocks
        if (blocks.Count > x * y)
        {
            for (int i = blocks.Count - 1; blocks.Count > x * y; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
                blocks.RemoveAt(i);
            }
        }
    }

    public void DestroyChildren()
    {

        for (int sub = 0; sub < 100; sub++) { for (int i = 0; i < transform.childCount; i++) { DestroyImmediate(transform.GetChild(i).gameObject); } } 
    }
}
