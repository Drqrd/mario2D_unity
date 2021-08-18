using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Powerup
{
    private float moveDirection;
    private float moveSpeed = 3f;
    private float jumpVelocity = 2f;
    private bool executedOnce = false;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Bc = GetComponent<BoxCollider>(); 
        SetTransform(transform);
        SetEmergedPos(transform.localPosition.y + 1f);

        // Hide the powerup behind the block
        HidePowerup();
    }

    private void FixedUpdate()
    {
        if (FinishedEmerging)
        {
            if (FinishedEmerging)
            {
                if (!executedOnce) { InitialWork(); }
                Rb.velocity = new Vector3(moveSpeed * moveDirection, Rb.velocity.y, 0f);
            }

            if (Rb.velocity.y == 0) { Rb.velocity = new Vector3(moveSpeed * moveDirection, jumpVelocity, 0f); }
        }
    }

    public void InitialWork()
    {
        // Get initial moveDirection
        moveDirection = Mathf.Sign(-transform.InverseTransformPoint(transform.localPosition).x - GameObject.Find("Player").transform.position.x);

        // make position of powerup on z = 0 for proper collisions
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

        // other stuff to do once position is corrected
        Bc.enabled = true;
        Rb.useGravity = true;
        FinishedEmerging = true;

        executedOnce = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        moveDirection = -moveDirection;
        if (collision.gameObject.name == "Player")
        {
            PlayerController reference = collision.transform.GetComponent<PlayerController>();

            // Add to score
            Score.AddScore(GetScore());

            // make mario invincible
            
            StartCoroutine(reference.Invincibility());


            // Disable game object
            transform.gameObject.SetActive(false);
        }
    }
}
