using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy, EnemyInterface
{
    protected bool isInShell = false;
    protected bool comingOutOfShell = false;
    protected bool shellIsMoving = false;
    protected float shellMultiplier = 1f;

    private GameObject reference;
    private const int fireScore = 100, stompScore = 200;
    private float moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        reference = GameObject.Find("Player");
    }
    private void FixedUpdate()
    {
        if (isEnabled)
        {
            UpdateAnimations();

            if (shellIsMoving)
            {
                if (rb.velocity.x == 0)
                {
                    // Get the direction in which the shell needs to move
                    moveDirection = reference.GetComponent<Rigidbody>().velocity.x > 0 ? 1f : -1f;
                    rb.velocity = new Vector3(moveSpeed * 2f * moveDirection, rb.velocity.y, 0f);
                }
                rb.velocity = new Vector3(moveSpeed * 2f, rb.velocity.y, 0f);
            }

            // If not dying, move left
            if (!dyingToStomp && !dyingToFire) { rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0f); }

            else if (dyingToStomp)
            {   
                rb.velocity = Vector3.zero;
                StartCoroutine(Shelled());
            }

            else if (dyingToFire) { DiedToFireAnimation(); }
        }
    }

    private void UpdateAnimations()
    {
        // Animation controlled by shelled IEnumerator
        anim.SetBool("_shelled", isInShell);

        // Resets every time the shell is touched or is moving 
        anim.SetBool("_comingOutOfShell", comingOutOfShell);

        // Speed of the coming out of shell animation
        anim.SetFloat("_shellAnimationSpeed", shellMultiplier);
    }

    public bool IsInShell
    {
        get { return isInShell; }
        set { isInShell = value; }
    }

    // Flip the goomba upside down, make it jump a little and turn off its collision
    public IEnumerator DeathTimer(string slainBy = "NaN")
    {
        if (slainBy == "Stomp")
        {
            if (!isInShell)
            {
                StartCoroutine(Shelled());
                AudioController.PlaySound("Stomp");
            }
            else { yield break; }
        }

        else if (slainBy == "Fireball")
        {
            dyingToFire = true;
            AudioController.PlaySound("Bump");
            yield return new WaitForSeconds(deathDuration);
            gameObject.SetActive(false);
        }

        // Disable if off screen to left or bottom
        else
        {
            gameObject.SetActive(false);
            yield break;
        }
    }

    // Recursive enumerator, exits when 4 seconds have passed and shell isnt moving
    protected IEnumerator Shelled()
    {
        isInShell = true;
        yield return new WaitForSeconds(4f);
        if (Mathf.Abs(rb.velocity.x) > 0) { StartCoroutine(Shelled()); }
        // transition to the coming out of shell animation
        else 
        { 
            isInShell = false; 
            comingOutOfShell = true;
            StartCoroutine(OutOfShell());
        }
    }

    protected IEnumerator OutOfShell()
    {
        yield return new WaitForSeconds(0.1f);
        if (Mathf.Abs(rb.velocity.x) > 0) 
        {
            shellMultiplier += .1f;
            StartCoroutine(OutOfShell()); 
        }
        else
        {
            shellMultiplier = 1f;
        }
    }

    public void ShellWasHit()
    {
        shellIsMoving = !shellIsMoving;
    }

    new private void OnCollisionEnter(Collision collision)
    {
        if (SideCollision(collision)) 
        { 
            // Change movement direction
            moveSpeed = -moveSpeed;

            // Since sprite isnt the same both ways, flip it
            if (moveSpeed > 0) { transform.localScale = new Vector3(-1f, 1f, 1f); }
            else { transform.localScale = new Vector3(1f, 1f, 1f); }
        }
    }
}
