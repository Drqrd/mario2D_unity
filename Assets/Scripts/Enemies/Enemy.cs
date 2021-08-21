using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    protected Animator anim;
    protected float moveSpeed = -1.5f;
    protected float deathDuration = .2f;
    protected bool dyingToStomp = false;
    protected bool dyingToFire = false;
    protected bool isEnabled = false, executedFireAnimation = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (isEnabled)
        {
            // If not dying, move in moveSpeed direction
            if (!dyingToStomp && !dyingToFire) { rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0f); }
        }
    }



    // Flip the sprite upside down, make it jump a little and turn off its collision
    protected void DiedToFireAnimation()
    {
        if (!executedFireAnimation)
        {
            executedFireAnimation = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            rb.rotation = Quaternion.Euler(180f, 0f, 0f);
            rb.velocity = new Vector3(0f, 10f, 0f);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void Enable()
    {
        if (!isEnabled)
        {
            isEnabled = true;
        }
    }

    protected bool SideCollision(Collision col)
    {
        float angle = Vector3.Angle(col.contacts[0].normal, Vector3.up);
        if (Mathf.Approximately(angle, 90f))
        {
            return true;
        }
        return false;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (SideCollision(collision)) { moveSpeed = -moveSpeed; }
    }
}
