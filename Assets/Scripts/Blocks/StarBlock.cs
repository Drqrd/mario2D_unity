using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBlock : PowerupBlock, BlockInterface
{
    // Causes hit and spawns powerup
    new public void Hit(Collision collision)
    {
        if (!GetFirstHit())
        {
            SetFirstHit(true);
            AudioController.PlaySound("Powerup Appears");
            GetComponent<SpriteRenderer>().sprite = hitSprite;
            StartCoroutine(Bump());
            SpawnPowerup();
        }
    }
}
