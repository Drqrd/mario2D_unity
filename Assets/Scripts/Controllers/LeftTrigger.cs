using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Disabling");
            other.gameObject.GetComponent<EnemyInterface>().Disable();
        }
    }
}
