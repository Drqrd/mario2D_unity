using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface with 
public interface BlockInterface
{
    void Hit(Collision collision);

    public bool GetIsBumping();
}
