using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Powerup, PowerupInterface
{
    private float moveDirection;
    private float moveSpeed = 3f;
    private float jumpVelocity = 2f;
    private bool executedOnce = false;

    private void Start()
    {
        jumpVelocity *= 8f;
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

    public void RevealPowerup()
    {
        StartCoroutine(EmergeFromBlock());
    }

    Vector2 DetectCollisionSide(Collision col)
    {
        float angle = Vector3.Angle(col.contacts[0].normal, Vector3.up);
        if (Mathf.Approximately(angle, 0f)) { return Vector2.up; }
        if (Mathf.Approximately(angle, 180f)) { return Vector2.down; }
        if (Mathf.Approximately(angle, 90f))
        {
            Vector3 cross = Vector3.Cross(Vector3.forward, col.contacts[0].normal);
            if (cross.y > 0f) { return Vector2.left; }
            else { return Vector2.right; }
        }
        return Vector2.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Vector2 side = DetectCollisionSide(collision);
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
        if (side == Vector2.left || side == Vector2.right) { moveDirection = -moveDirection; }
    }
}
