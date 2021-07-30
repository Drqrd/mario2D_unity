using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHorizontalPipe : MonoBehaviour
{
    private const int MAX_LENGTH = 10;

    public bool update = false;


    [Header("Parameters")]
    public bool hasTop;
        [Range(1, MAX_LENGTH)]
    public int lengthOfBody;


    private void OnValidate()
    {
        if (update)
        {
            update = false;
            AdjustPipe();
        }
    }
    private void AdjustPipe()
    {

        // Updates end bits
        transform.Find("TopRight").localPosition = new Vector3(lengthOfBody, 1f, 0f);
        transform.Find("BottomRight").localPosition = new Vector3(lengthOfBody, 0f, 0f);

        // Middle position updates
        for (int i = 1; i <= lengthOfBody; i++)
        {
            transform.Find("TopMiddle " + i).localPosition = new Vector3(i - 1, 1f, 0f);
            transform.Find("BottomMiddle " + i).localPosition = new Vector3(i - 1, 0f, 0f);
        }

        // Shrink middle pipes
        for (int i = lengthOfBody; i <= MAX_LENGTH; i++)
        {
            transform.Find("TopMiddle " + i).localPosition = new Vector3(lengthOfBody - 1, 1f, 0f);
            transform.Find("BottomMiddle " + i).localPosition = new Vector3(lengthOfBody - 1, 0f, 0f);
        }
    }
}
