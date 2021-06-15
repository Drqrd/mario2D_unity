using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject target;
    Transform leftBounds, rughtBounds;

    private void Start()
    {
        target = GameObject.Find("Player");
    }

    private void Update()
    {
        
    }
}
