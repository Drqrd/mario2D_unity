using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInteractables : MonoBehaviour
{
    private const int X_MIN = 1, X_MAX = 50;

    [Range(X_MIN, X_MAX)]
    public int x = 1;

    /*
    public GameObject initialSprite;
    public bool newObject = true;
    */
    public bool update = false;


    private PhysicMaterial bcMaterial;

    private void OnValidate()
    {
        if (update)
        {
            update = false;
            transform.Find("Colliders").transform.Find("Collider").GetComponent<BoxCollider>().size = new Vector3(transform.Find("Sprites").transform.childCount, 1f, 1f);
            UpdateTheSprites();
        }
    }

    private void Start()
    {
        transform.Find("Colliders").transform.Find("Collider").GetComponent<BoxCollider>().size = new Vector3(transform.Find("Sprites").transform.childCount, 1f, 1f);
    }

    /// <summary>
    /// ALL BELOW COMMENTED OUT WAS FOR EDITOR TESTING
    /// </summary>
    /*
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
        bcMaterial = (PhysicMaterial)Resources.Load("Materials/NoFriction", typeof(PhysicMaterial));
    }

    void SpawnSprites()
    {
        if (transform.Find("Sprites") == null)
        {
            GameObject parent = new GameObject("Sprites");
            parent.transform.parent = transform;
            parent.transform.localPosition = Vector3.zero;
            for (int i = 0; i < X_MAX; i++)
            {
                GameObject obj = Instantiate(initialSprite);
                obj.transform.parent = parent.transform;
                obj.transform.localPosition = new Vector3((i - (float)X_MAX) + X_MAX / 2f + .5f, 0f, 0f);
            }
        }
    }

    void SpawnBoxColliders()
    {
        if (transform.Find("Colliders") == null)
        {
            GameObject obj = new GameObject("Colliders");
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
            GameObject child = new GameObject("Collider");
            child.transform.parent = obj.transform;
            child.transform.localPosition = Vector3.zero;
            BoxCollider bc = child.AddComponent<BoxCollider>();
            bc.material = bcMaterial;
            bc.size = new Vector3(X_MIN, 1, 1);
            child.tag = ("BlockColliders");
        }
    }


    //  Disables and enables sprites depending on x, also adjusts positions
    void IEditorUpdateSprites()
    {
        Transform parent = transform.Find("Sprites");
        int activeChildren = ActiveChildCount(parent);

        // Enable / disable
        if (activeChildren < x)
        {
            for (int i = activeChildren; i < x; i++)
            {
                GameObject obj = parent.GetChild(i).gameObject;
                obj.SetActive(true);
            }
        }
        else if (activeChildren > x)
        {
            for (int i = activeChildren - 1; i >= x; i--)
            {
                GameObject obj = parent.GetChild(i).gameObject;
                obj.SetActive(false);
            }
        }

        // Adjusts positions
        activeChildren = ActiveChildCount(parent);
        for (int i = 0; i < activeChildren; i++)
        {
            GameObject obj = parent.GetChild(i).gameObject;
            obj.transform.localPosition = new Vector3(i - (activeChildren / 2f) + 0.5f, 0f, 0f);
        }

    }

    void IEditorUpdateBoxColliders()
    {
        transform.Find("Colliders").GetChild(0).GetComponent<BoxCollider>().size = new Vector3(x, 1f, 1f);
    }

    int ActiveChildCount(Transform t)
    {
        int num = 0;
        foreach (Transform child in t) { if (child.gameObject.activeSelf) { num++; } }
        return num;
    }
    */
    /// <summary>
    /// END OF EDITOR TESTING STUFF
    /// </summary>

    void UpdateTheSprites()
    {
        Transform sprites = transform.Find("Sprites");
        for (int i = 0; i < sprites.childCount; i++)
        {
            GameObject obj = sprites.GetChild(i).gameObject;
            obj.transform.localPosition = new Vector3(i - (sprites.childCount / 2f) + 0.5f, 0f, 0f);
        }
    }

    public void FindBlock(Collision collision)
    {
        Vector3 collisionPos = collision.contacts[0].point;
        Transform sprites = transform.Find("Sprites").transform;
        float smallestDist = Vector3.Distance(sprites.GetChild(0).position, collisionPos);
        int id = 0;

        for (int i = 1; i < transform.Find("Sprites").childCount; i++)
        {
            if (sprites.GetChild(i).gameObject.activeSelf)
            {
                if (Vector3.Distance(sprites.GetChild(i).position, collisionPos) < smallestDist)
                {
                    smallestDist = Vector3.Distance(sprites.GetChild(i).position, collisionPos);
                    id = i;
                }
            }
        }
        Transform child = sprites.GetChild(id);
        StartCoroutine(child.GetComponent<IInterface>().Hit(collision));
    }

    // If hit end of box collider. adjust size and position
    // if hit in box collider. spawn new box collider. old box collider adjusted left, new box collider adjusted right
    public void NewColliders(Transform hitBlock, Collision collision)
    {
        // Mumbo jumbo to determine local position and see if it matches
        BoxCollider bc = collision.gameObject.GetComponent<BoxCollider>(); 
        const float offset = 0.5f;
        // Ends and size and blockPos change depending on box collider and child hit
        float ends = bc.bounds.extents.x;
        float size = bc.bounds.size.x;
        float blockPos = hitBlock.localPosition.x;

        // Sign changes depending on if the hit block is to the left (-) or to the right (+) of the center (Value is -1 or 1)
        float sign = Mathf.Sign(bc.bounds.center.x - hitBlock.position.x);

        // Records the local position of the center of the box collider (Normally, bc.bounds.center gives global position.
        // .InverseTransformPoint gets the localposition insead.
        float center = transform.InverseTransformPoint(bc.bounds.center).x;

        // Gets the block position, and adds the offset * -sign, which moves the block position to the left if (-) and vice versa
        // This is used to determine if the block is at the end of the object using the ends variable
        float isEnd = blockPos + -sign * offset;

        // If hitting end
        if (center + ends == isEnd || center - ends == isEnd)
        { 
            // Subtract block from collider, adjust position
            bc.size += new Vector3(-1f, 0f, 0f);
            bc.center += new Vector3(offset * sign, 0f, 0f);

            // disable the collider if it has no width
            if (bc.size.x == 0) { bc.enabled = false; }
        }
        else
        {
            // distBetween is determined by the size / 2f. If the size of the collider is 7, that means that the distance in between two centers
            // would be equal to 7.0f / 2.0f = 3.5f + 0.5f = 4.0f... this means that I only have to calculate the left center, and add distBetween
            // to determine the right center
            float distBetween = size * offset + offset;

            // numberToRight is the number of blocks to the right the hit block is from the center
            float numberToRight = size - ((blockPos - center) + ends + offset);
            float numberToLeft = size - numberToRight - 1f;

            // LeftCenter is the localPosition of the center of the new boxCollider
            float leftCenter = -(size * offset - offset) + (offset * (numberToLeft - 1f));

            // Debug Stuff
            /*
            Debug.Log("Center: " + center);
            Debug.Log("Size: " + size);
            Debug.Log("Ends: " + ends);
            Debug.Log("BlockPos: " + blockPos);
            Debug.Log("numberToLeft: " + numberToLeft);
            Debug.Log("numberToRight: " + numberToRight);
            Debug.Log("distBetween: " + distBetween);
            Debug.Log("--------------------------------");
            */

            // Modifies the old collider to take the shape of the leftover blocks to the left
            bc.center += new Vector3(leftCenter, 0f, 0f);
            bc.size = new Vector3(numberToLeft, 1f, 1f);

            // Create new collider to be shifted to the right
            GameObject child = new GameObject("Collider");
            child.transform.parent = transform.Find("Colliders").transform;
            child.transform.localPosition = Vector3.zero;
            child.tag = ("BlockColliders");
            BoxCollider newBc = child.AddComponent<BoxCollider>();
            
            // Modifies the new colliders to take the shape of the leftover blocks to the right
            newBc.center = new Vector3(bc.center.x + distBetween, 0f, 0f);
            newBc.size = new Vector3(numberToRight, 1f, 1f);
            newBc.material = bcMaterial;
        }
    }
}

