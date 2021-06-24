using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCoinBlock : MonoBehaviour, IInterface
{
    private static Vector3 bumpUp = new Vector3(0f, 0.25f, 0f);
    private Vector3 bumpDown = -bumpUp;
    private float animationTime = .05f;
    private Sprite hitSprite;
    private bool coined = false;

    private void Start()
    {
        hitSprite = (Sprite)Resources.Load("Sprites/Hit_OW", typeof(Sprite));
    }

    public IEnumerator Hit(Collision collision)
    {
        if (!coined)
        {
            coined = true;
            AudioController.PlaySound("Bump");
            AudioController.PlaySound("Coin");
            float initialTime = Time.time;
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = hitSprite;
            transform.Translate(bumpUp);
            SpawnCoin();
            yield return new WaitUntil(() => Time.time > initialTime + animationTime);

            transform.Translate(bumpDown);
        }
    }

    void SpawnCoin()
    {

    }
}
