using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBlock : PowerupBlock, BlockInterface
{
    // Causes hit and spawns powerup
    new public void Hit(Collision collision)
    {
        if (!GetFirstHit())
        {
            SetFirstHit(true);
            AudioController.PlaySound("Powerup Appears");
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = hitSprite;
            StartCoroutine(Bump());
            SpawnPowerup();
        }
    }
}
