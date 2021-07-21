using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBlock : Block, BlockInterface
{
    private Sprite hitSprite;

    private void Start()
    {
        hitSprite = (Sprite)Resources.Load("Sprites/Hit_OW", typeof(Sprite));
    }

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

    public Sprite GetHitSprite()
    {
        return hitSprite;
    }
}
