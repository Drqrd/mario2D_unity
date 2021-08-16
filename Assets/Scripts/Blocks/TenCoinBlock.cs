using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenCoinBlock : CoinBlock, BlockInterface
{
    private const int MAX_HITS = 10;
    private int hitNum = 0;

    new public void Hit(Collision collision)
    {
        if (hitNum < MAX_HITS)
        {
            StartCoroutine(Bump());
            AudioController.PlaySound("Coin");
            if (hitNum == MAX_HITS - 1)
            {
                GetComponent<SpriteRenderer>().sprite = hitSprite;
            }
            SpawnCoin();
            hitNum += 1;
        }
    }
}
