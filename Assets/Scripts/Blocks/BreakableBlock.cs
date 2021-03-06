using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : Block, BlockInterface
{

    private const int blockScore = 50;

    // If hit a breakable block as small mario, bump it. Else, destroy it
    public void Hit(Collision collision)
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().GetState() == "Small Mario")
        {
            StartCoroutine(Bump()); 
        }
        else
        {
            // Disable gameobject
            transform.gameObject.SetActive(false);

            // Add score
            Score.AddScore(blockScore);

            AudioController.PlaySound("Break Block");

            transform.parent.parent.GetComponent<UpdateInteractables>().NewColliders(transform, collision);
        }
    }

    public bool GetIsBumping()
    {
        return isBumping;
    }
}
