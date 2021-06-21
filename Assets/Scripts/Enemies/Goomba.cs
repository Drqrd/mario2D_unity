using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private float moveSpeed = -1.5f;
    private float deathDuration = .2f;
    private bool dying = false;
    private bool isEnabled = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (isEnabled)
        {
            if (!dying) { rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0f); }
            else { rb.velocity = Vector3.zero; }
            SetAnimations();
        }
    }

    private void SetAnimations()
    {
        anim.SetBool("_dying", dying);
    }

    public void Enable()
    {
        if (!isEnabled)
        {
           isEnabled = true;
        }
    }

    public IEnumerator DeathTimer()
    {
        dying = true;
        AudioController.PlaySound("Stomp");
        yield return new WaitForSeconds(deathDuration);
        gameObject.SetActive(false);
    }
}
