using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody rb;
    private bool start = false;
    private bool moving = false;
    private float stopMovement;
    private float moveDirection;
    private float moveSpeed = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
        stopMovement = transform.localPosition.y + 1f;
        
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            rb.velocity = new Vector3(moveSpeed * moveDirection, rb.velocity.y, 0f);
        }
    }

    private void PlayAnimation()
    {
        while (transform.localPosition.y < stopMovement)
        {
            rb.velocity = new Vector3(0f, 2f, 0f);
        }
        moveDirection = Mathf.Sign(transform.InverseTransformPoint(transform.localPosition).x - GameObject.Find("Player").transform.position.x);
        moving = true;
    }

    public void StartShroom()
    {
        if (!start) { start = true; PlayAnimation(); }
    }
}
