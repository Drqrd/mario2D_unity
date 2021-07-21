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
    public void SetRigidbody(Rigidbody r)
    {
        rb = r;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    // Get / Set for box collider
    public void SetBoxCollider(BoxCollider c)
    {
        bc = c;
    }
    public BoxCollider GetBoxCollider()
    {
        return bc;
    }

    public void SetFinishedEmerging(bool val)
    {
        finishedEmerging = val;
    }
    public bool GetFinishedEmerging()
    {
        return finishedEmerging;
    }

    public void SetTransform(Transform t)
    {
        transform = t;
    }

    public void SetEmergedPos(float f)
    {
        emergedPos = f;
    }
}
