using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : Block, BlockInterface
{
    public Sprite hitSprite;

    public GameObject child;

    private const int coinScore = 200;

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
