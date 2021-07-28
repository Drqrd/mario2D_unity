using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private bool moveCoin = false;
    private float coinSpeed = .75f, maxTravelDist = 2.5f;
    private float atEndDuration = .1f;
    private Vector3 startPos, endPos;

    private void Start()
    {
        startPos = transform.localPosition;
        endPos = new Vector3(transform.localPosition.x, transform.localPosition.y + maxTravelDist, transform.localPosition.z);        
    }

    private void FixedUpdate()
    {
        // Move the coin to position
        if (moveCoin)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, coinSpeed);
        }
    }
    public IEnumerator RevealCoin()
    {
        moveCoin = true;
        yield return new WaitUntil(() => transform.localPosition.y >= endPos.y);
        moveCoin = false;
        yield return new WaitForSeconds(atEndDuration);
        transform.localPosition = startPos;
    }
}
