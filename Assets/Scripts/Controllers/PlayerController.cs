using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement Keys
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.S;
    public KeyCode fireKey = KeyCode.K;
    public KeyCode runKey = KeyCode.J;

    // Vectors for changing scale
    private Quaternion faceLeft = Quaternion.Euler(0f, 180f, 0f);
    private Quaternion faceRight = Quaternion.Euler(0f, 0f, 0f);

    // Movement
    private float jumpVelocity = 5.5f, maxFallVelocity = -10f, jumpHeight, maxJumpHeight;
    private float runVelocity = 6f, maxRunVelocity = 10f;
    private float storedDirection, slowDownDirection;
    private Vector3 yGravity = 5f * Physics.gravity;
    private Vector3 zeroGravity = 0f * Physics.gravity;
    private bool stopXVelocity, stopYVelocity;
    private bool isGrounded;
    private bool isRunning, isRunningLeft, isRunningRight, isJumping, isCrouching, isInMotionX, isInMotionY;
    private bool queueFire;
    private bool hasBumped = false;
    private bool runToggle;

    // State of mario
    public static string state;

    // Other stuff like throwing fireballs or crouching
    // private bool powerUp = false;
    private bool isSmall;
    private bool isLarge;
    private bool isFire;


    // For animation
    // static bool invincible = false;

    // For timing
    float bumpTimer, bumpSeconds, bumpDuration;

    // because I dont want to type get component
    Rigidbody rb;
    Animator anim;
    BoxCollider bc;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        anim = transform.GetComponent<Animator>();
        bc = transform.GetComponentInChildren<BoxCollider>();

        // Adjust gravity
        Physics.gravity = yGravity;
    }

    // Physics based movement
    private void FixedUpdate()
    {
        CheckInputs();

        // Update state of mario
        UpdateState();

        // Movement
        Move();

        // Timers and such
        jumpHeight = rb.worldCenterOfMass.y;
        if (hasBumped) { BumpSeconds(); }

        // Control parameters in information
        Animate();

        // Speed cap
        CapVelocity();
    }

    // Check if moving
    void CheckInputs()
    {
        // bool suburbia
        bool inputLeft = Input.GetKey(moveLeftKey);
        bool inputRight = Input.GetKey(moveRightKey);
        bool inputJump = Input.GetKey(jumpKey);
        bool inputCrouch = Input.GetKey(crouchKey);
        bool inputFire = Input.GetKey(fireKey);
        bool runToggle = Input.GetKey(runKey);

        // bool city
        isRunning = inputLeft || inputRight;
        isRunningLeft = inputLeft && !inputRight;
        isRunningRight = !inputLeft && inputRight;
        isJumping = inputJump;
        isCrouching = inputCrouch;
        queueFire = inputFire && isFire;
    }

    void UpdateState()
    {
        if (isSmall) { state = "Small Mario"; }
        if (isLarge) { state = "Large Mario"; }
        if (isFire) { state = "Fire Mario"; }
    }

    // Set animations
    void Animate()
    {
        // The easy ones
        anim.SetBool("_isRunning", isRunning);
        anim.SetBool("_isJumping", isJumping);

        // For controlling how fast the run animation is played
        float runSpeedMultiplier = 1f;
        if (runSpeedMultiplier < Mathf.Abs(rb.velocity.x)) { runSpeedMultiplier = Mathf.Abs(rb.velocity.x); }
        anim.SetFloat("_runSpeedMultiplier", runSpeedMultiplier);

        // If velocity.x is not 0
        if (rb.velocity.x != 0f) { isInMotionX = true; }
        else { isInMotionX = false; }
        anim.SetBool("_isInMotionX", isInMotionX);

        // If velocity.y is not 0
        if (rb.velocity.y != 0f) { isInMotionY = true; }
        else { isInMotionY = false; }
        anim.SetBool("_isInMotionY", isInMotionY);
    }

    // Movement controller (check if key is pressed & does not equal previous move, if no key pressed move in same direction)
    void Move()
    {        
        // flip the sprite
        if (bc != null)
        {
            if (isRunningLeft && transform.rotation != faceLeft) { transform.rotation = faceLeft; }
            else if (isRunningRight && transform.rotation != faceRight) { transform.rotation = faceRight; }
        }

        // Run detection
        Run();

        // Slow down if no input
        if (isGrounded && Input.GetAxisRaw("Horizontal") == 0f && rb.velocity.x != 0f)
        {
            if (Mathf.Sign(rb.velocity.x) == slowDownDirection ) { rb.AddForce(Vector2.right * storedDirection * runVelocity); }
            else { rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z); }
        }

        // Jumping detection
        if (isGrounded) { isJumping = false; Jump(); }
    }

    // Jump detection
    void Jump()
    { 
        if (Input.GetKey(KeyCode.Space))
        {   
            // Play jump sound
            AudioController.PlaySound("Jump-Super");

            // Disable gravity
            Physics.gravity = zeroGravity;
            maxJumpHeight = rb.worldCenterOfMass.y + 4f;
            StartCoroutine(JumpTimer());

            rb.velocity += new Vector3(0f,jumpVelocity,0f);
        }  
    }

    // Run detection
    void Run()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        
        if (runToggle) { direction *= 2; }

        // Getting slow down data
        if (Input.GetKey(moveLeftKey) || Input.GetKey(moveRightKey))
        {
            storedDirection = -direction * 2;
            if (Input.GetKey(moveLeftKey)) { slowDownDirection = -1f; }
            if (Input.GetKey(moveRightKey)) { slowDownDirection = 1f; }
        }
        rb.AddForce(Vector2.right * direction * runVelocity);
    }

    // Reenable physics after duration
    IEnumerator JumpTimer()
    {
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space) || jumpHeight > maxJumpHeight || stopYVelocity);
        Physics.gravity = yGravity;
        stopYVelocity = false;
    }

    // Easy fix for bad code
    void CapVelocity()
    {
        if (rb.velocity.y < 0 && rb.velocity.y < maxFallVelocity) { rb.velocity = new Vector3(rb.velocity.x, maxFallVelocity, rb.velocity.z); }
        if (rb.velocity.x > maxRunVelocity) { rb.velocity = new Vector3(maxRunVelocity, rb.velocity.y, rb.velocity.z); }
        if (rb.velocity.x < -maxRunVelocity) { rb.velocity = new Vector3(-maxRunVelocity, rb.velocity.y, rb.velocity.z); }
    }

    void Bump()
    {
        AudioController.PlaySound("Bump");
        hasBumped = true;
        StartCoroutine(BumpTimer());
    }

    IEnumerator BumpTimer()
    {
        yield return new WaitUntil(() => bumpSeconds >= bumpDuration);
        hasBumped = false;
        bumpTimer = 0f;
        bumpSeconds = 0f;
    }

    // for determining if you can play bump sound again
    void BumpSeconds()
    {
        bumpTimer += Time.time;
        bumpSeconds = bumpTimer % 60f;
    }

    // Detects collision side, only for collision enter
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

    // Detect collisions
    private void OnCollisionEnter(Collision collision)
    {
        Vector2 direction = DetectCollisionSide(collision);

        // If you collide with side... 
        if (direction == Vector2.left || direction == Vector2.right)
        {
            // If in air, stop
            if (rb.velocity.y != 0) { stopXVelocity = true; stopYVelocity = true; }

            // Play bump sound if on ground
            if (rb.velocity.y == 0) { if (!hasBumped) { Bump(); } }
        }

        // If you hit block, stop jump
        if (direction == Vector2.down) 
        { 
            stopYVelocity = true; 
            Bump();
            if (collision.gameObject.tag == "Breakable Block") { BlockController.HitBreakableBlock(collision); }
        }

        // determines if you can jump again
        if (collision.transform.tag == "Floor") { isGrounded = true; }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor") { isGrounded = false; }
        stopXVelocity = false;
    }
}
