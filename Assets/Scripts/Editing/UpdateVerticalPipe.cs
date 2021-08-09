using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVerticalPipe : MonoBehaviour
{
    private const int MAX_LENGTH = 8;

    public bool update = false;

    public BoxCollider bc;
    public BoxCollider topCollider;


    [Header("Parameters")]
    public bool hasTop = false;
    [Range(1, MAX_LENGTH)]
    public int lengthOfBody;


    private void OnValidate()
    {
        if (update)
        {
            update = false;
            Top();
            AdjustPipe();
            UpdateCollider();
        }
    }

    private void Top()
    {
        if (hasTop) 
        { 
            transform.Find("Top").gameObject.SetActive(true);
            topCollider.enabled = true;
        }
        else 
        { 
            transform.Find("Top").gameObject.SetActive(false);
            topCollider.enabled = false;
        }
    }

    private void AdjustPipe()
    {
        Transform ml = transform.Find("Middle Left");
        Transform mr = transform.Find("Middle Right");

        // Middle position updates
        for (int i = 0; i < lengthOfBody; i++)
        {
            ml.transform.GetChild(i).localPosition = hasTop == true ? new Vector3(0f, 0f - i, 0f) : new Vector3(0f, -1f - i, 0f);
            mr.transform.GetChild(i).localPosition = hasTop == true ? new Vector3(1f, 0f - i, 0f) : new Vector3(1f, -1f - i, 0f);
        }

        // Shrink middle pipes
        for (int i = lengthOfBody; i <  MAX_LENGTH; i++)
        {
            ml.transform.GetChild(i).localPosition = hasTop == true ? new Vector3(0f, -lengthOfBody + 1f, 0f) : new Vector3(0f, -lengthOfBody, 0f);
            mr.transform.GetChild(i).localPosition = hasTop == true ? new Vector3(1f, -lengthOfBody + 1f, 0f) : new Vector3(1f, -lengthOfBody, 0f);
        }
    }

    private void UpdateCollider()
    {
        bc.center = new Vector3(0.5f, -lengthOfBody / 2f + 0.5f, 0f);
        bc.size = new Vector3(2f, -lengthOfBody, 1f);
    }
}
