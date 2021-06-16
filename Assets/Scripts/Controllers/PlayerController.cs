using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Movement Keys
    [Header("Controls")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.S;
    public KeyCode fireKey = KeyCode.K;
    public KeyCode runKey = KeyCode.J;

    [Header("Player Components")]
    public Rigidbody rigidBody;
    public Animator animator;
    public BoxCollider boxCollider;
    
    // Vectors for changing scale
    private Quaternion faceLeft = Quaternion.Euler(0f, 180f, 0f);
    private Quaternion faceRight = Quaternion.Euler(0f, 0f, 0f);

    // Movement
    private float jumpVelocity = 5.5f, maxFallVelocity = -10f, jumpHeight, maxJumpHeight;
    private float runVelocity = 6f, maxRunVelocity = 10f;
    private Vector3 yGravity = 5f * Physics.gravity;
    private Vector3 zeroGravity = 0f * Physics.gravity;
    private bool stopXVelocity, stopYVelocity;
    private bool isGrounded;
    private bool isRunning, isRunningLeft, isRunningRight, isJumping, isCrouching, isTurning, isInMotionX, isInMotionY;
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
    private bool noInteraction;

    // For timing
    float bumpTimer, bumpSeconds, bumpDuration = 1f;

    // Loading resources
    PhysicMaterial noFriction, playerFriction;

    private void Start()
    {
        noFriction = (PhysicMaterial)Resources.Load("Materials/NoFriction",typeof(PhysicMaterial));
        playerFriction = (PhysicMaterial)Resources.Load("Materials/PlayerFriction", typeof(PhysicMaterial));

        if (SceneManager.GetActiveScene().name == "W1-L1") { state = "Small Mario"; }

        // Adjust gravity
        Physics.gravity = yGravity;
    }

    // Physics based movement
    private void FixedUpdate()
    {
        CheckInputs();

        // Movement
        Move();

        // Timers and such
        jumpHeight = rigidBody.worldCenterOfMass.y;
        if (hasBumped) { BumpSeconds(); }

        // Control parameters in information
        Animate();

        // Speed cap
        CapVelocity();
    }

    // Check if moving
    void CheckInputs()
    {
        // bool suburigidBodyia
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
        isTurning =  Mathf.Abs(rigidBody.velocity.x) > 2f && (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(rigidBody.velocity.x));
    }

    void UpdateState()
    {
        if (isSmall) { state = "Small Mario"; }
        if (isLarge) { state = "Large Mario"; }
        if (isFire) { state = "Fire Mario"; }
    }

    // Set animatorations
    void Animate()
    {
        // The easy ones
        animator.SetBool("_isRunning", isRunning);
        animator.SetBool("_isJumping", isJumping);
        animator.SetBool("_isTurning", isTurning);
        

        // For controlling how fast the run animation is played
        float runSpeedMultiplier = 1f;
        if (runSpeedMultiplier < Mathf.Abs(rigidBody.velocity.x)) { runSpeedMultiplier = Mathf.Abs(rigidBody.velocity.x); }
        animator.SetFloat("_runSpeedMultiplier", runSpeedMultiplier);

        // If velocity.x is not 0
        if (rigidBody.velocity.x != 0f) { isInMotionX = true; }
        else { isInMotionX = false; }
        animator.SetBool("_isInMotionX", isInMotionX);

        // If velocity.y is not 0
        if (rigidBody.velocity.y != 0f) { isInMotionY = true; }
        else { isInMotionY = false; }
        animator.SetBool("_isInMotionY", isInMotionY);
    }

    // Movement controller (check if key is pressed & does not equal previous move, if no key pressed move in same direction)
    void Move()
    {        
        // flip the sprite
        if (isRunningLeft && transform.rotation != faceLeft) { transform.rotation = faceLeft; }
        else if (isRunningRight && transform.rotation != faceRight) { transform.rotation = faceRight; }

        // Run detection
        Run();

        // Add friction if no input and moving
        if ((Mathf.Sign(rigidBody.velocity.x) != Mathf.Sign(Input.GetAxisRaw("Horizontal")) || Input.GetAxisRaw("Horizontal") == 0) && rigidBody.velocity.x != 0f) { boxCollider.material = playerFriction; }
        else { boxCollider.material = noFriction; }

        // Jumping detection
        if (isGrounded) { isJumping = false; Jump(); }
    }

    // Run detection
    void Run()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if (runToggle) { direction *= 2; }
        rigidBody.AddForce(Vector2.right * direction * runVelocity);
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
            maxJumpHeight = rigidBody.worldCenterOfMass.y + 4f;
            StartCoroutine(JumpTimer());

            rigidBody.velocity += new Vector3(0f,jumpVelocity,0f);
        }  
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
        if (rigidBody.velocity.y < 0 && rigidBody.velocity.y < maxFallVelocity) { rigidBody.velocity = new Vector3(rigidBody.velocity.x, maxFallVelocity, rigidBody.velocity.z); }
        if (rigidBody.velocity.x > maxRunVelocity) { rigidBody.velocity = new Vector3(maxRunVelocity, rigidBody.velocity.y, rigidBody.velocity.z); }
        if (rigidBody.velocity.x < -maxRunVelocity) { rigidBody.velocity = new Vector3(-maxRunVelocity, rigidBody.velocity.y, rigidBody.velocity.z); }
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
        // Get face of hit
        Vector2 direction = DetectCollisionSide(collision);

        // If you collide with side... 
        if (direction == Vector2.left || direction == Vector2.right)
        {
            // If in air, stop
            if (rigidBody.velocity.y != 0) { stopXVelocity = true; stopYVelocity = true; }

            // Play bump sound if on ground
            if (rigidBody.velocity.y == 0) { if (!hasBumped) { Bump(); } }
        }

        // If you hit block, stop jump
        if (direction == Vector2.down)
        {
            stopYVelocity = true;
            Bump();
            if (collision.gameObject.tag == "Breakable Block")
            {
                StartCoroutine(BlockController.HitBreakableBlock(collision));
            }
            if (collision.gameObject.tag == "Single Coin Block")
            {
                StartCoroutine(BlockController.HitSingleCoinBlock(collision));
            }
        }

        // determines if you can jump again
        if (direction == Vector2.up) { isGrounded = true; }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (rigidBody.velocity.y != 0f) { isGrounded = false; }
        stopXVelocity = false;
    }
}
