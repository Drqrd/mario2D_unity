using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChildController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        transform.parent.gameObject.GetComponent<UpdateInteractables>();
    }
}
