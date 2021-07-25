using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private const int MAX_STOMP_MULTIPLIER = 9;

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
    private float jumpVelocity = 20f, maxYVelocity = 20f, jumpHeight, maxJumpHeight;
    private float runVelocity = 6f, maxRunVelocity = 10f;
    private Vector3 yGravity = 4.5f * Physics.gravity;
    private Vector3 zeroGravity = 0f * Physics.gravity;
    private bool stopXVelocity, stopYVelocity;
    private bool isGrounded;
    private bool isRunning, isRunningLeft, isRunningRight, isJumping, isCrouching, isTurning, isInMotionX, isInMotionY;
    private bool queueFire;
    private bool hasBumped = false;
    private bool runToggle;
    private bool isColliding;

    // State of mario
    private string state;

    // Other stuff like throwing fireballs or crouching
    // private bool powerUp = false;
    private bool isSmall = true;
    private bool isLarge = false;
    private bool isFire = false;
    private bool toLarge = false;
    private bool toFire = false;
    private bool isInvincible = false;
    private float powerUpTime = 1f;

    // For timing
    private float bumpDuration = .2f;

    // Loading resources
    PhysicMaterial noFriction, playerFriction;

    private int stompMultiplier;

    private void Start()
    {
        noFriction = (PhysicMaterial)Resources.Load("Materials/NoFriction",typeof(PhysicMaterial));
        playerFriction = (PhysicMaterial)Resources.Load("Materials/PlayerFriction", typeof(PhysicMaterial));

        if (SceneManager.GetActiveScene().name == "W1-L1") { SetState("Small Mario"); }
        SetState("Large Mario");
        // Adjust gravity
        Physics.gravity = yGravity;
    }


    // Physics based movement
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P)) { Debug.Log(isGrounded); }

        CheckInputs();

        UpdateState();

        // Movement
        Move();

        // Timers and such
        jumpHeight = rigidBody.worldCenterOfMass.y;

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

    public void SetState(string state)
    {
        if (state == "Small Mario") { isSmall = true; isLarge = false; isFire = false;}
        else if (state == "Large Mario") { isSmall = false; isLarge = true; isFire = false;}
        else if (state == "Fire Mario") { isSmall = false; isLarge = false; isFire = true;}
        else { Debug.Log("ERR: Cannot set this state: " + state); }
        PowerUp();
    }

    // Set animatorations
    void Animate()
    {
        // The easy ones
        animator.SetBool("_isRunning", isRunning);
        animator.SetBool("_isJumping", isJumping);
        animator.SetBool("_isTurning", isTurning);
        animator.SetBool("_toLarge", toLarge);
        animator.SetBool("_toFire", toFire);
        animator.SetBool("_isInvincible", isInvincible);

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
            rigidBody.velocity += new Vector3(0f,jumpVelocity,0f);
        }  
    }   

    // Easy fix for bad code
    void CapVelocity()
    {
        if (rigidBody.velocity.y < 0 && rigidBody.velocity.y < -maxYVelocity) { rigidBody.velocity = new Vector3(rigidBody.velocity.x, -maxYVelocity, rigidBody.velocity.z); }
        if (rigidBody.velocity.x > maxRunVelocity) { rigidBody.velocity = new Vector3(maxRunVelocity, rigidBody.velocity.y, rigidBody.velocity.z); }
        if (rigidBody.velocity.x < -maxRunVelocity) { rigidBody.velocity = new Vector3(-maxRunVelocity, rigidBody.velocity.y, rigidBody.velocity.z); }
        if (rigidBody.velocity.y > 0 && rigidBody.velocity.y > maxYVelocity) { rigidBody.velocity = new Vector3(rigidBody.velocity.x, maxYVelocity, rigidBody.velocity.z); }
    }

    IEnumerator BumpTimer()
    {
        hasBumped = true;
        yield return new WaitForSeconds(bumpDuration);
        hasBumped = false;
    }

    private void Bounce()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 10f, 0f);
    }

    private IEnumerator PowerUp()
    {
        float time = Time.time;
        if (isLarge)
        {
            Vector3 previousVelocity = rigidBody.velocity;
            rigidBody.isKinematic = true;
            toLarge = true;
            yield return new WaitUntil(() => Time.time >= time + powerUpTime);
            toLarge = false;
            rigidBody.isKinematic = false;
            rigidBody.velocity = previousVelocity;

        }
        else if (isFire)
        {
            yield return new WaitUntil(() => Time.time >= time + powerUpTime);
        }

        yield return new WaitForSeconds(0f);
    }

    public string GetState()
    {
        return state;
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
        string tag = collision.gameObject.tag;
        // Get face of hit
        Vector2 direction = DetectCollisionSide(collision);

        // If you collide with side... 
        if (direction == Vector2.left || direction == Vector2.right)
        {
            // If in air, stop
            if (rigidBody.velocity.y != 0) { stopXVelocity = true; stopYVelocity = true; }

            // Play bump sound if on ground
            if (rigidBody.velocity.y == 0) { if (!hasBumped) { AudioController.PlaySound("Bump"); StartCoroutine(BumpTimer()); } }
        }

        // If you hit block, stop jump
        if (direction == Vector2.down)
        {
            stopYVelocity = true;
            isColliding = true;
            if (tag == "BlockColliders" && !hasBumped)
            {
                collision.transform.parent.parent.GetComponent<UpdateInteractables>().FindBlock(collision);
                StartCoroutine(BumpTimer());
            }
        }

        // determines if you can jump again
        if (direction == Vector2.up) 
        { 
            isGrounded = true;
            if (tag == "Enemy")
            {
                if (collision.gameObject.name.Contains("Goomba"))
                {
                    StartCoroutine(collision.gameObject.GetComponent<Goomba>().DeathTimer("Stomp"));
                    Overlay.AddToScores(Score.GetStompedEnemyPoints(stompMultiplier));
                    if (stompMultiplier < MAX_STOMP_MULTIPLIER) { stompMultiplier++; }
                    if (Input.GetKey(jumpKey)) { Jump(); }
                    else { Bounce(); }
                }
            }
            else { stompMultiplier = 0; }
        }
    }

    // says isGrounded is false to make sure you cant jump
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        if (rigidBody.velocity.y != 0f) { isGrounded = false; }
        stopXVelocity = false;
    }
}
