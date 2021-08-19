using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSlope : MonoBehaviour
{
    [Header("Bools")]
    public bool update = false;
    public bool flip;

    [Header("Dimensions (height < width)")]
    [Range(1,20)]
    public int width = 1;
    [Range(1,15)]
    public int height = 1;

    [Header("Sprite")]
    public Sprite sprite;


    // Update when object is changed in editor
#if UNITY_EDITOR
    void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
    void _OnValidate()
    {
        if (update)
        {
            update = false;
            // Flips the slope if bool is on
            if (flip) { transform.localScale = new Vector3(-1f, 1f, 1f); }
            else { transform.localScale = new Vector3(1f, 1f, 1f); }

            // Get current active gameObjects
            int activeGameObjects = FindActiveGameObjects();

            if (height > activeGameObjects)
            { 
                // Checks if the width is greater than or equal to the height before executing
                if (width >= height) { more_AdjustCollidersAndSprites(); }
                else { Debug.Log("WARNING: height cannot be greater than width."); }
            }
            else if (height < activeGameObjects)
            {
                DisableUnusedGameObjects(activeGameObjects);
                if (width >= height) { less_AdjustCollidersAndSprites(); }
                else { Debug.Log("WARNING: height cannot be greater than width."); }
            }
        }
    }
#endif


    // Counts up the total number of active children
    private int FindActiveGameObjects()
    {
        int activeCount = 0;
        foreach (Transform child in transform) { if (child.gameObject.activeSelf) { activeCount++; } }
        return activeCount;
    }


    private void more_AdjustCollidersAndSprites()
    {
        int l_width = width;
        // Starting with the bottom row, builds a half pyramid by adjusting sprites and colliders
        for (int i = 0; i < height; i++)
        {
            Transform child = transform.GetChild(i);

            // Make sure that the gameobject is enabled
            child.gameObject.SetActive(true);

            // Adjust the colliders
            BoxCollider bc = child.GetComponent<BoxCollider>();
            bc.size = new Vector3(l_width, 1f, 1f);

            // Adjust the sprites and tile
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.size = new Vector2(l_width, 1f);

            // Adjust local position to form slope
            child.localPosition = new Vector3(l_width / 2f, i, 0f);

            // Subtract from width
            l_width--;
        }
    }

    private void less_AdjustCollidersAndSprites()
    {
        // Starting from the top, increase going down
        int l_width = width - (height - 1);
        
        // Starting with the bottom row, builds a half pyramid by adjusting sprites and colliders
        for (int i = height - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            // Make sure that the gameobject is enabled
            child.gameObject.SetActive(true);

            // Adjust colliders
            BoxCollider bc = child.GetComponent<BoxCollider>();
            bc.size = new Vector3(l_width, 1f, 1f);

            // Adjust sprites and tile
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.size = new Vector2(l_width, 1f);

            // Adjust local position to form slope
            child.localPosition = new Vector3(l_width / 2f, i, 0f);

            // Add to the width
            l_width++;
        }
    }

    // Disable extra gameObjects
    private void DisableUnusedGameObjects(int ii)
    {
        for (int i = ii - 1; i >= height; i--)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
