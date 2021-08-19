using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKoopa : Enemy, EnemyInterface
{
    private Rigidbody rb;
    private Animator anim;
    private const int fireScore = 100, stompScore = 200;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (isEnabled)
        {
            // If not dying, move left
            if (!dyingToStomp && !dyingToFire) { rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0f); }

            else if (dyingToStomp) { rb.velocity = Vector3.zero; DiedToStompAnimation(); }

            else if (dyingToFire) { DiedToFireAnimation(); }
        }
    }

    private void DiedToStompAnimation()
    {
        anim.SetBool("_dying", dyingToStomp);
    }

    // Flip the goomba upside down, make it jump a little and turn off its collision

    public IEnumerator DeathTimer(string slainBy = "NaN")
    {
        if (slainBy == "Stomp")
        {
            StartCoroutine(Shelled());
            AudioController.PlaySound("Stomp");
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

    // CONTINUE HERE
    private IEnumerator Shelled()
    {
        yield return new WaitUntil(() => fireScore == 100);
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
