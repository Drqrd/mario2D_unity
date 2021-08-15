using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour, EnemyInterface
{
    private Rigidbody rb;
    private Animator anim;
    private float moveSpeed = -1.5f;
    private float deathDuration = .2f;
    private bool dyingToStomp = false;
    private bool dyingToFire= false;
    private bool isEnabled = false, executedFireAnimation = false;
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
    private void DiedToFireAnimation()
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

    public IEnumerator DeathTimer(string slainBy = "NaN")
    {
        if (slainBy == "Stomp")
        {
            dyingToStomp = true;
            AudioController.PlaySound("Stomp");
            yield return new WaitForSeconds(deathDuration);
            gameObject.SetActive(false);
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

    private bool SideCollision(Collision col)
    {
        float angle = Vector3.Angle(col.contacts[0].normal, Vector3.up);
        if (Mathf.Approximately(angle, 90f))
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SideCollision(collision)) { moveSpeed = -moveSpeed; }
    }
}
