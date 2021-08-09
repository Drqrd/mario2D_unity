using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHorizontalPipe : MonoBehaviour
{
    private const int MAX_LENGTH = 8;

    public bool update = false;

    public BoxCollider bc;


    [Header("Parameters")]
    [Range(1, MAX_LENGTH)]
    public int lengthOfBody;


    private void OnValidate()
    {
        if (update)
        {
            update = false;
            AdjustPipe();
            UpdateCollider();
        }
    }
    private void AdjustPipe()
    {

        // Updates end bits
        transform.Find("TopRight").localPosition = new Vector3(lengthOfBody, 1f, 0f);
        transform.Find("BottomRight").localPosition = new Vector3(lengthOfBody, 0f, 0f);

        Transform tm = transform.Find("TopMiddle");
        Transform bm = transform.Find("BottomMiddle");

        // Middle position updates
        for (int i = 1; i <= lengthOfBody; i++)
        {
            tm.transform.GetChild(i).localPosition = new Vector3(i - 1, 1f, 0f);
            bm.transform.GetChild(i).localPosition = new Vector3(i - 1, 0f, 0f);
        }

        // Shrink middle pipes
        for (int i = lengthOfBody; i <= MAX_LENGTH; i++)
        {
            tm.transform.GetChild(i).localPosition = new Vector3(lengthOfBody - 1, 1f, 0f);
            bm.transform.GetChild(i).localPosition = new Vector3(lengthOfBody - 1, 0f, 0f);
        }
    }

    private void UpdateCollider()
    {
        bc.center = new Vector3(lengthOfBody / 2f -0.5f, 0.5f, 0f);
        bc.size = new Vector3(lengthOfBody + 2f, 2f, 1f);
    }
}
