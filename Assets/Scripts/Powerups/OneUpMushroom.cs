using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUpMushroom : Mushroom
{
    new public void OnCollisionEnter(Collision collision)
    {
        MoveDirection = -MoveDirection;
        if (collision.gameObject.name == "Player")
        {
            // give one up
            // ENABLE HERE

            // Disable game object
            transform.gameObject.SetActive(false);
        }
    }
}
