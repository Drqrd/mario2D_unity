using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private static Vector3 bumpUp = new Vector3(0f,0.25f,0f);
    private static Vector3 bumpDown = -bumpUp;
    private static float animationTime =.05f;

    public static IEnumerator HitBreakableBlock(Collision collision)
    {
        if (PlayerController.state == "Small Mario")
        {
            float initialTime = Time.time;
            collision.transform.Translate(bumpUp);
            yield return new WaitUntil(() => Time.time > initialTime + animationTime);
            collision.transform.Translate(bumpDown);
        }
        else
        {

        }
    }

    public static IEnumerator HitSingleCoinBlock(Collision collision)
    {
        float initialTime = Time.time;
        collision.transform.Translate(bumpUp);
        SpawnCoin();
        yield return new WaitUntil(() => Time.time > initialTime + animationTime);
        
        collision.transform.Translate(bumpDown);
    }

    static void SpawnCoin()
    {

    }
}
