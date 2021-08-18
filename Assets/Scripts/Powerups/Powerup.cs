using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider bc;
    new private Transform transform;
    private float emergeSpeed;
    private bool finishedEmerging;
    private float emergedPos;

    private const int powerupScore = 1000;

    // Default constructor automatically called by children
    public Powerup()
    {
        emergeSpeed = 1f;
    }

    public IEnumerator EmergeFromBlock()
    {
        rb.velocity = new Vector3(0f, emergeSpeed, 0f);
        yield return new WaitUntil(() => transform.localPosition.y >= emergedPos);
        finishedEmerging = true;
    }

    // Get / Set for rigidbody
    public Rigidbody Rb
    {
        get { return rb; }
        set { rb = value; }
    }

    // Get / Set for box collider
    public BoxCollider Bc
    {
        get { return bc; }
        set { bc = value; }
    }
    public bool FinishedEmerging
    {
        get { return finishedEmerging; }
        set { finishedEmerging = value; }
    }

    public void SetTransform(Transform t)
    {
        transform = t;
    }

    public void SetEmergedPos(float f)
    {
        emergedPos = f;
    }

    public int GetScore()
    {
        return powerupScore;
    }
   
    public void HidePowerup()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1f);
    }

    public void RevealPowerup()
    {
        StartCoroutine(EmergeFromBlock());
    }
}
