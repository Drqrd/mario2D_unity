using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInteractables : MonoBehaviour
{
    private const int X_MIN = 1, X_MAX = 50;

    [Range(X_MIN, X_MAX)]
    public int x = 1;

    public GameObject initialSprite;
    public bool update = false; 
    public bool newObject = true;

    private PhysicMaterial bcMaterial;
    private float leftX, rightX;

    private List<BoxCollider> bcs = new List<BoxCollider>();

    // Update when object is changed in editor
    #if UNITY_EDITOR
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
    void _OnValidate()
    {
        bcMaterial = (PhysicMaterial)Resources.Load("Materials/NoFriction", typeof(PhysicMaterial));

        if (newObject) { SpawnSprites(); SpawnBoxColliders(); newObject = false; }

        if (update)
        {
            IEditorUpdateSprites();
            IEditorUpdateBoxColliders();
        }
    }
    #endif

    private void Start()
    {
        leftX = transform.GetChild(0).localPosition.x;
        rightX = transform.GetChild(transform.childCount - 1).localPosition.x;
        bcMaterial = (PhysicMaterial)Resources.Load("Materials/NoFriction", typeof(PhysicMaterial));
        bcs.Add(GetComponent<BoxCollider>());
    }

    void SpawnSprites()
    {
        if (transform.childCount == 0)
        {
            for (int i = 0; i < X_MAX; i++)
            {
                GameObject obj = Instantiate(initialSprite);
                obj.transform.parent = transform;
                obj.transform.localPosition = new Vector3((i - (float)X_MAX) + X_MAX / 2f + .5f, 0f, 0f);
            }
        }
    }

    void SpawnBoxColliders()
    {
        if (GetComponent<BoxCollider>() == null)
        {
            BoxCollider bc = transform.gameObject.AddComponent<BoxCollider>();
            bc.material = bcMaterial;
            bc.size = new Vector3(X_MIN, 1, 1);
        }
    }

    /// <summary>
    ///  Disables and enables sprites depending on x, also adjusts positions
    /// </summary>
    void IEditorUpdateSprites()
    {
        int activeChildren = ActiveChildCount();

        // Enable / disable
        if (activeChildren < x)
        {
            for (int i = activeChildren; i < x; i++)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                obj.SetActive(true);
            }
        }
        else if (activeChildren > x)
        {
            for (int i = activeChildren - 1; i >= x; i--)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                obj.SetActive(false);
            }
        }

        // Adjusts positions
        activeChildren = ActiveChildCount();
        for (int i = 0; i < activeChildren; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            obj.transform.localPosition = new Vector3(i - (activeChildren / 2f) + 0.5f, 0f, 0f);
        }
        
    }

    void IEditorUpdateBoxColliders()
    {
        GetComponent<BoxCollider>().size = new Vector3(x, 1f, 1f);
    }

    int ActiveChildCount()
    {
        int num = 0;
        foreach (Transform child in transform) { if (child.gameObject.activeSelf) { num++; } }
        return num;
    }

    public void FindBlock(Collision collision)
    {
        Vector3 collisionPos = collision.contacts[0].point;
        float smallestDist = Vector3.Distance(transform.GetChild(0).position, collisionPos);
        int id = 0;
        
        for (int i = 1; i < X_MAX; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                if (Vector3.Distance(transform.GetChild(i).position, collisionPos) < smallestDist)
                {
                    smallestDist = Vector3.Distance(transform.GetChild(i).position, collisionPos);
                    id = i;
                }
            }
        }

        StartBlockEvent(id, collision);
    }

    public void StartBlockEvent(int id, Collision collision)
    {
        if (transform.GetChild(id).tag == "Breakable Block")
        {
            StartCoroutine(BlockController.HitBreakableBlock(id, collision));
        }
    }
    // if hit end of box collider. Find box collider hit. adjust size and position
    // if hit in box collider. Find box collider hit. Destroy that box collider. Create 2 new box colliders 
    public void NewColliders(int id, Collision collision)
    {
        Transform hitBlock = transform.GetChild(id);
        float offset = 0f;
        if (transform.position.x % 2f != 0f) { offset = 0.5f; }
        Bounds bounds = collision.collider.bounds;
        float ends = bounds.extents.x;
        // NEEDS ASSISTANCE
        float blockPos = Mathf.Abs(hitBlock.localPosition.x) + offset + transform.InverseTransformPoint(bounds.center).x;

        Debug.Log(ends);
        Debug.Log(blockPos);

        if (ends == blockPos)
        {
            float sign = Mathf.Sign(bounds.center.x-hitBlock.position.x);
            if (bcs.Count > 1)
            {

            }
            else
            {
                bcs[0].size += new Vector3(-1f, 0f, 0f);
                bcs[0].center += new Vector3(0.5f * sign, 0f, 0f);
            }
            
        }
        else
        {

            BoxCollider new_bc = transform.gameObject.AddComponent<BoxCollider>();
        }
    }
}
