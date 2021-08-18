using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Powerup, PowerupInterface
{
    private float moveDirection;
    private float moveSpeed = 3f;
    private bool executedOnce = false;
    private bool alreadyJumping = false;

    private void Start()
    {
        // Set the rigidbody, collider and localPosition of the powerup
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
            if (!executedOnce) { InitialWork(); }
            Rb.velocity = new Vector3(moveSpeed * moveDirection, Rb.velocity.y, 0f);
        }

        if (Rb.velocity.y == 0) { alreadyJumping = false; }
    }

    public float MoveDirection
    {
        get { return moveDirection; }
        set { moveDirection = value; }
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

            // if small mario, make large mario
            if (reference.GetState() == "Small Mario") { reference.SetState("Large Mario"); }

            // Disable game object
            transform.gameObject.SetActive(false);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Collider") 
        {
            GameObject block = collision.transform.parent.parent.GetComponent<UpdateInteractables>().FindBlockForMushroom(collision);
            if (block.GetComponent<BlockInterface>().GetIsBumping()) 
            {
                if (!alreadyJumping)
                {
                    alreadyJumping = true;
                    Rb.velocity = new Vector3(Rb.velocity.x, 10f, 0f);
                }
            }
        }
    }
}
