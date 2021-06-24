using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour, IInterface
{
    private static Vector3 bumpUp = new Vector3(0f, 0.25f, 0f);
    private static Vector3 bumpDown = -bumpUp;
    private static float animationTime = .05f;
    public IEnumerator Hit(Collision collision)
    { 
        if (PlayerController.state == "Small Mario")
        {
            AudioController.PlaySound("Bump");
            float initialTime = Time.time;
            transform.Translate(bumpUp);
            yield return new WaitUntil(() => Time.time > initialTime + animationTime);
            transform.Translate(bumpDown);
        }
        else
        {
            transform.gameObject.SetActive(false);
            transform.parent.parent.GetComponent<UpdateInteractables>().NewColliders(transform, collision);
        }
    }
}
