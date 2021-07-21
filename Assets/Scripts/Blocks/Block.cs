using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 bump;
    private float animationTime;
    private bool firstHit;
    public bool isBumping;

    public Block()
    {
        bump = new Vector3(0f, 0.25f, 0f);
        animationTime = .05f;
        firstHit = false;
    }


    // Every block does this when it is hit by mario
    public IEnumerator Bump()
    {
        isBumping = true;
        AudioController.PlaySound("Bump");
        float initialTime = Time.time;
        transform.Translate(GetBumpDistance());
        yield return new WaitUntil(() => Time.time > initialTime + GetAnimationTime());
        transform.Translate(-GetBumpDistance());
        isBumping = false;
    }

    // Returns the value for bump
    public Vector3 GetBumpDistance()
    {
        return bump;
    }

    // Returns the value for animationTime
    public float GetAnimationTime()
    {
        return animationTime;
    }

    // Get and set for the firstHit val used by single coin blocks and powerup blocks
    public void SetFirstHit(bool val)
    {
        firstHit = val;
    }
    public bool GetFirstHit()
    {
        return firstHit;
    }
}
