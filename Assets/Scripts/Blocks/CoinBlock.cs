using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : Block, BlockInterface
{
    private Sprite hitSprite;

    public GameObject child;

    private const int coinScore = 200;

    private void Start()
    {
        hitSprite = (Sprite)Resources.Load("Sprites/Hit_OW", typeof(Sprite));
    }

    public void Hit(Collision collision)
    {
        if (!GetFirstHit())
        {
            SetFirstHit(true);
            StartCoroutine(Bump());
            AudioController.PlaySound("Coin");
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = hitSprite;
            SpawnCoin();
        }
        else { AudioController.PlaySound("Bump"); }
    }

    public bool GetIsBumping()
    {
        return isBumping;
    }


    public void SpawnCoin()
    {
        // Add score
        Score.AddScore(coinScore);
        StartCoroutine(child.GetComponent<BlockCoin>().RevealCoin());
    }
}
