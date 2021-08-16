using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPowerupBlock : PowerupBlock, BlockInterface
{
    public GameObject player;

    private bool revealed = false;
    private bool enableCollider;
    private BoxCollider bc;



    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        bc = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // If the player is below the obj, enable collider
        if (!revealed)
        {
            enableCollider = player.transform.position.y < transform.parent.parent.position.y - .1f ? true : false;
            bc.enabled = enableCollider;
        }
    }

    new public void Hit(Collision collision)
    {
        if (!GetFirstHit())
        {
            SetFirstHit(true);
            AudioController.PlaySound("Powerup Appears");
            GetComponent<SpriteRenderer>().sprite = hitSprite;
            StartCoroutine(Bump());

            bc.enabled = true;
            revealed = true;

            EnableSprites();
            SpawnMushroom();
        }
    }

    private void EnableSprites()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    public void SpawnMushroom()
    {
        transform.GetChild(0).GetComponent<PowerupInterface>().RevealPowerup();
    }
}
