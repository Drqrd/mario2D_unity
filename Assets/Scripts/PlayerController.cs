using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        Move();

    }

    // Movement controller (check if key is pressed & does not equal previous move, if no key pressed move in same direction)
    Vector3 Move()
    {
        Vector3 playerMovement = Vector3.zero;
        return playerMovement;
    }
}
