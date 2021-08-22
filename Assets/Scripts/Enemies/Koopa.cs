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

    new private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
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
                    rb.velocity = new Vector3(moveSpeed * 5f * moveDirection, rb.velocity.y, 0f);
                }
                rb.velocity = new Vector3(moveSpeed * 5f * moveDirection, rb.velocity.y, 0f);
            }

            // If not dying or in shell, move left
            if (!dyingToFire && !isInShell && !comingOutOfShell) 
            { 
                rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0f); 
            }

            else if (dyingToFire) 
            {
                DiedToFireAnimation();
                dyingToFire = false;
            }
        }
    }

    public bool ShellIsMoving
    {
        get { return shellIsMoving; }
        set { shellIsMoving = value; }
    }
    
    public bool IsInShell
    {
        get { return isInShell; }
        set { isInShell = value; }
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
            AudioController.PlaySound("Kick");
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
        rb.isKinematic = true;
        InShellCollider();
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(8f);
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
        // Every .2 seconds, check to see if it is coming out of the shell
        yield return new WaitForSeconds(.2f);

        // If moving, shellMultiplier = 1, dont do anything else
        if (Mathf.Abs(rb.velocity.x) > 0) 
        {
            shellMultiplier = 1f;
        }
        // If reached animation threshold, reset the shellMultiplier and dont do anything else
        else if (shellMultiplier > 2.6f)
        {
            OutOfShellCollider();
            comingOutOfShell = false;
            shellMultiplier = 1f;
        }
        // If not moving, increase shellMultiplier and do it again
        else
        {
            shellMultiplier += .1f;
            StartCoroutine(OutOfShell());
        }
    }



    private void InShellCollider()
    {
        bc.center = new Vector3(0f, -.5f, 0f);
        bc.size = Vector3.one;
    }

    private void OutOfShellCollider()
    {
        bc.center = new Vector3(0f, -0.275f, 0f);
        bc.size = new Vector3(1, 1.45f, 1f);
    }

    public void ShellWasHit()
    {
        shellIsMoving = !shellIsMoving;
        Debug.Log(shellIsMoving);
        if (shellIsMoving)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;
        }
    }

    new private void OnCollisionEnter(Collision collision)
    {
        if (SideCollision(collision)) 
        { 
            if (!isInShell && !comingOutOfShell)
            {
                // Change movement direction
                moveSpeed = -moveSpeed;

                // Since sprite isnt the same both ways, flip it
                if (moveSpeed > 0) { transform.localScale = new Vector3(-1f, 1f, 1f); }
                else { transform.localScale = new Vector3(1f, 1f, 1f); }
            }
            if (shellIsMoving)
            {
                
                if (collision.gameObject.tag == "Enemy")
                {
                    
                    StartCoroutine(collision.gameObject.GetComponent<EnemyInterface>().DeathTimer("Fireball"));
                }
            }
        }
    }
}
