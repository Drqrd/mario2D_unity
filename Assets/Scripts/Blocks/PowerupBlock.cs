using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBlock : Block, BlockInterface
{
    public Sprite hitSprite;

    public void Hit(Collision collision)
    {
        if (!GetFirstHit())
        {
            SetFirstHit(true);
            AudioController.PlaySound("Powerup Appears");
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = hitSprite;
            StartCoroutine(Bump());
        }
    }

    public bool GetIsBumping()
    {
        return isBumping;
    }

    public void SpawnPowerup()
    {
        transform.GetChild(0).GetComponent<PowerupInterface>().RevealPowerup();
    }
}
