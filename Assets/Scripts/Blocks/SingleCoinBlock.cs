using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCoinBlock : MonoBehaviour
{
    private static Vector3 bumpUp = new Vector3(0f, 0.25f, 0f);
    private static Vector3 bumpDown = -bumpUp;
    private static float animationTime = .05f;

    public IEnumerator HitSingleCoinBlock(Collision collision)
    {
        Debug.Log("Coin");
        float initialTime = Time.time;
        collision.transform.Translate(bumpUp);
        SpawnCoin();
        yield return new WaitUntil(() => Time.time > initialTime + animationTime);
        
        collision.transform.Translate(bumpDown);
    }

    void SpawnCoin()
    {

    }
}
