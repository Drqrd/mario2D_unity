using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    protected BoxCollider bc;
    protected Animator anim;
    protected float moveSpeed = -1.5f;
    protected float deathDuration = .2f;
    protected bool dyingToStomp = false;
    protected bool dyingToFire = false;
    protected bool isEnabled = false, executedFireAnimation = false;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
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

    public void Disable()
    {
        gameObject.SetActive(false);
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
