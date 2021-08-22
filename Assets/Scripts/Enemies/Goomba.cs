using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy, EnemyInterface
{
    private const int fireScore = 100, stompScore = 200;

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
}
