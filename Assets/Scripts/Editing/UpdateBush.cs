using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBush : MonoBehaviour
{
    private const int MAX_LENGTH = 3;

    [Range(1, MAX_LENGTH), Header("Length Of Bush")]
    public int length = 1;

    private void OnValidate()
    {
        UpdateObject();
    }


    void UpdateObject()
    {
        // Updates end bit
        transform.Find("Right").localPosition = new Vector3(length, 0f, 0f);

        // Middle position updates
        for (int i = 1; i <= length; i++)
        {
            transform.Find("Middle " + i).localPosition = new Vector3(i - 1, 0f, i * .1f);
        }

        // Shrinks middle bushes
        for (int i = length; i <= MAX_LENGTH; i++)
        {
            transform.Find("Middle " + i).localPosition = new Vector3(length - 1, 0f, i * .1f);
        }

    }
}
