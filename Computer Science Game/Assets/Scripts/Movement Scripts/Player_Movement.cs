using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Player_Movement : MonoBehaviour 
{
    // Variable definition for all used variables in this entire movement script.
    // Each set of variables is defined in blocks relevant to where it is used.

    // Defining a reference to the character controller.
    public CharacterController controller; 

    // Defining parameters on how the player will move,
    // their speed while crouching or standing, as well as a current speed placeholder.
    // Values for jump height and count are also defined here.
    public float defaultSpeed = 20f; 
    public float crouchingSpeed = 15f;
    public float speed;
    public float jumpHeight = 3f;  
    public int jumpsRemaining = 1;

    // Defining values that control the strength of gravity
    // and the current altitude of the player.
    public float gravity = -9.81f;
    public float altitude;

    // Defining Transforms for where the groundChecks will be anchored and
    // distance from which the player will check for the ground.
    // A layer mask is also created to determine what surfaces to look for.
    public Transform groundCheck; 
    public Transform crouchingGroundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;

    // Defining Transforms for where the wall run checks will be anchored and
    // distance from which the player will check for a wall.
    // A layer mask is also created to determine what surfaces to look for.
    public Transform wallL; 
    public Transform wallR; 
    public float wallDistance = 0.4f; 
    public LayerMask wallMask; 

    // Defines a Vector3 reference for
    // the character controller's velocity.
    Vector3 velocity;

    // Creates a set of booleans that can be referred to
    // for the validation of the player's state.
    public bool onFloor;
    bool onWallA;
    bool onWallB;

    // Defines a set of variables that are used to refer to the
    // player's current crouching state.
    public bool crouching; 
    public bool crouching2;
    public bool crouchAnimation;
    // Defining variables that control the controllers positioning
    // and height within the game world.
    public float standingHeight = 2f; 
    public float crouchingHeight = 0.25f;
    Vector3 standingCentre = new Vector3(0, 0, 0); 
    Vector3 crouchingCentre = new Vector3(0, 0.5f, 0);
    // Defines the speed of the transition between crouching height
    // and crouching walk speed.
    public float timeToCrouch = 0.125f;
    public float timeToCrouchSpeed = 1.5f;

    // Defining a boolean that indecates if the player is currently
    // air dashing or not, and if it is on cooldown.
    public bool airDash = false;
    public float dashCooldown = 0;

    // Creates a Vector3 and Boolean used for reference of the grounding
    // state of the player, the type of slope and if they're on one too steep.
    // As well as the sliding speed for when sliding off a slope.
    Vector3 hitNormal;
    public bool onSlope;
    public float hitAngle;
    public float slideSpeed = 15f;

    // Defines the rigidbody controller used when grappling, the maximum
    // grapple distance, what can be grappled to and the origin point for
    // the raycast used to locate the grapple point.
    private Rigidbody rb;
    public float maxDistance = 100;
    public LayerMask whatCanGrapple;
    public new Transform camera;

    // Creates a set of boolean variables that can be referred to,
    // to see what state of movement the player is in
    public bool swinging;
    public bool swingingExit;
    public bool jumping;
    public bool moving;
    public bool grounded;

    // On collision with a game object, find the vertical rotation of
    // the object relative to the player.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    // Update is called once per frame
    void Update()
    {

        // Convert the hitAngle to a Vector3 and return the angle under hitNormal
        // The player is on a valid slope if the angle is between the set parameters.
        hitAngle = Vector3.Angle(Vector3.up, hitNormal);
        onSlope = (hitAngle > 45 & hitAngle < 88);

        // If the player is on a valid slope, apply velocity that pushes them away from
        // the slope. The steeper the slope, the less velocity will be added.
        // Horizontal velocity is multiplied by slide speed to make the movement noticable.
        if (onSlope)
        {
            velocity.y += (gravity * 2);
            velocity.x = ((1f - hitNormal.y) * hitNormal.x) * slideSpeed;
            velocity.z = ((1f - hitNormal.y) * hitNormal.z) * slideSpeed;
        }
        // Otherwise, add no velocity to the player.
        else
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        // Altitude is equal to the Y position of the player.
        altitude = transform.position.y;

        // If the player's altitude is below -15,
        // disable the controller and reposition the player, befor re-enabling the controller.
        if (altitude < -15)
        {
            controller.enabled = false;
            controller.transform.position = new Vector3(0, 0.4f, 0);
            controller.enabled = true;
        }

        // The player is currently on a wall if a surface of the correct type (layer mask)
        // is in range of either side of the player.
        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask);
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask);

        // If the player is not currently crouching, use the default groundCheck position.
        if (!crouching)
        {
            onFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 
        }
        // Otherwise, use the crouching groundcheck (Placed at the bottom of the player's crouching position)
        else onFloor = Physics.CheckSphere(crouchingGroundCheck.position, groundDistance, groundMask);

        // If on a wall, set the gravity to minimum and reset the player's remaining jumps.
        if ((onWallA || onWallB) & !onFloor & !Input.GetButtonDown("Jump") & !crouching) 
        {
            velocity.y = -1f; 
            jumpsRemaining = 2;
        }

        // An if statement to ensure the player is correctly grounded and that
        // the dash cooldown and jumps remaining are reset when on the floor.
        if (onFloor && velocity.y < 0)
        {
            velocity.y = -2; 
            dashCooldown = 1;
            jumpsRemaining = 2;
        }

        // Takes inputs for the player movement
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical"); 

        // Defines the Vector3 move as position transformations in the game world
        // moving in either direction dependin on if the axisMove is positive or negative
        Vector3 move = transform.right * xMove + transform.forward * zMove; 

        // Applies the move vector to the controller to move it based on the
        // defined speed of the controller
        controller.Move(speed * Time.deltaTime * move);

        // If the player presses the jump key, and they have jumps remaining, vertical velocity will be
        // applied to them based off their jump height and the strenghth of gravity.
        if (Input.GetButtonDown("Jump") && (jumpsRemaining > 0) && !crouching) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
            jumpsRemaining -= 1;
        }
        // If the player is crouching and tries to jump, they will be un-crouched
        // and follow the normal jump behaviors.
        else if (Input.GetButtonDown("Jump") && (jumpsRemaining > 0) && crouching && !crouchAnimation)
        {
            StartCoroutine(toCrouch()); 
            StartCoroutine(toCrouchSpeed()); 
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsRemaining -= 1; 
        }

        // If the player presses the crouch button, annd is on the floor
        // the coroutines that handle crouch state are called.
        if (Input.GetButtonDown("Crouch") && !crouchAnimation && onFloor)
        {
            StartCoroutine(toCrouch());
            StartCoroutine(toCrouchSpeed()); 
        }

        // If the player isn't using a speed altering ability
        // their speed is set to its default.
        if (!crouching && !airDash)
        {
            speed = defaultSpeed; 
        }

        // The time taken for the player to change standing states
        // changes depending on if they are currently crouching
        timeToCrouchSpeed = !crouching2 ? 1.5f : 0f;

        // Sets and applies the constant force of gravity to the player
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime); 

        // If the player presses their special ability button and is airborne, the dash cooldown will be set off
        // and the coroutine that handles dash speed will be called.
        if (Input.GetButtonDown("Special") && !onFloor && (!onWallA && !onWallB) && dashCooldown > 0)
        {
            airDash = true;
            dashCooldown = 0; 
            StartCoroutine(airDashSpeed());
            airDash = false;
        }

        // The following if statements are used to determine and set
        // the values of player movement state booleans.
        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumping = false;
        }

        if (Input.GetButtonDown("Horizontal") | Input.GetButtonDown("Vertical"))
        {
            moving = true;
        }

        if (Input.GetButtonUp("Horizontal") | Input.GetButtonUp("Vertical"))
        {
            moving = false;
        }

        // Applies a secondary ground check for when exiting a swing, as the player is always considered
        // to be on the ground when swinging.
        // This is to avoid later issues with gravity building up massive downward velocity when it shouldn't. 
        // This check is used for grounding when the default groundCheck is in use.
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Gets the rigidbody component of the player and
        // translates it's current directional velocity into
        // variables for controlling the player's swing speed
        rb = this.GetComponent<Rigidbody>();
        Vector3 velocityTrans = rb.velocity;
        float rbVx = rb.velocity.x;
        float rbVy = rb.velocity.y;
        float rbVz = rb.velocity.z;

        // If the player presses down their Right Mouse Button and a physics raycast for a grapple succeeds, the player will be classified as
        // swinging and the controller will be disabled in favour of the rigidbody controller and it's built in physics calculations.
        if (Input.GetButtonDown("Fire2") & Physics.Raycast(camera.position, camera.forward, out _, maxDistance, whatCanGrapple))
        {
            swinging = true;
            controller.enabled = false;
            this.GetComponent<Rigidbody>().useGravity = true;
        }

        // When the RMB is released the player will no longer be swinging,
        // they will follow the movement path of the rigidbody and have their jumps reset.
        if (Input.GetButtonUp("Fire2"))
        {
            swinging = false;
            swingingExit = true;
            jumpsRemaining = 2;
        }

        // While in rigidbody mode and not swinging the character controller will be re-enabled if
        // a movement event is triggered or the player becomes grounded.
        // The rigidbody's gravity is also disabled again to prevent issues with grounding.
        if((grounded || (onWallA || onWallB) || onSlope || jumping || moving) && !swinging && swingingExit)
        {
            controller.enabled = true;
            // If the player is jumping at this time, their vertical velocity is cancelled out in favour
            // of the jump calculation.
            if (jumping)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            // Otherwise, the player will retain the vertical velocity of the rigidbody.
            else
            {
                velocity.y = rbVy;
            }
            this.GetComponent<Rigidbody>().useGravity = false;
            swingingExit = false;
        }
       
        // If the controller is currently disabled, the player is considered to be grounded to avoid issues
        // with the gravity calculations
        if (controller.enabled == false)
        {
            onFloor = true;
            // If the velocity of the rigidbody is below 20 units when swinging,
            // incrament its velocity by one 2000th of itself. 
            if((rbVx < 20 & rbVx > -20 & rbVy < 20 & rbVy > -20 & rbVz < 20 & rbVz > -20) && swinging)
            {
                rb.velocity += new Vector3((rb.velocity.x / 2000), (rb.velocity.y / 2000), (rb.velocity.z / 2000));
            }
            
        }

    }

    // After each frame update, if the controller is enabled,
    // reset the velocity of the rigidbody to 0.
    // This is done to prevent velocity from previous grapple
    // swings from carrying over to the next if the controller
    // has been enabled since.
    private void LateUpdate()
    {
        if (controller.enabled == true)
        {
            Vector3 zeroVelocity = new Vector3(0, 0, 0);
            this.GetComponent<Rigidbody>().velocity = zeroVelocity;
        }
    }

    // Creates the coroutine which is responsible for player hitbox positioning
    // and transitioning it between crouching and standing.
    private IEnumerator toCrouch()
    {
        crouchAnimation = true; 

        // Defines a blank float used for counting how much time has passed.
        float timeSince = 0;
        
        // Sets the target height and centre based on if the player is crouching or not
        // before defining the current values which will be used as the original value in the Lerp function.
        float targetHeight = crouching ? standingHeight : crouchingHeight;
        float currentHeight = controller.height; 
        Vector3 targetCentre = crouching ? standingCentre : crouchingCentre;
        Vector3 currentCentre = controller.center; 

        // Over a set amount of time, the controller height and centre will be converted from
        // one state to the other, and the timer will be incremented by the time since the previous frame.
        while (timeSince < timeToCrouch) 
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeSince / timeToCrouch); 
            controller.center = Vector3.Lerp(currentCentre, targetCentre, timeSince / timeToCrouch);
            timeSince += Time.deltaTime; 
            yield return null;
        }

        // A validation check to ensure the controller height and centre are correctly set.
        controller.center = targetCentre;
        controller.height = targetHeight;

        // The boolean that indecated the current state is inverted.
        crouching = !crouching; 

        crouchAnimation = false;
    }

    // Creates the coroutine responsible for managing the speed of the player
    // when transitioning between crouching and standing.
    private IEnumerator toCrouchSpeed()
    {
        // Defines a blank float used for counting how much time has passed.
        float timeSince1 = 0;

        // Sets the target player speed depending on if they are crouching or not.
        float targetSpeed = crouching ? defaultSpeed : crouchingSpeed;
        float currentSpeed; 

        // If not crouching, the current speed is the default plus a boost
        if (!crouching2) 
        {
            currentSpeed = (defaultSpeed + 10);
        }
        // Otherwise the starting speed is the defined crouching speed
        else currentSpeed = crouchingSpeed; 

        // Over a set of time, the controller speed is transitioned from it's current speed
        // to the target speed and the timer will be incremented by the time since the last frame.
        while (timeSince1 < timeToCrouchSpeed) 
        {
            speed = Mathf.Lerp(currentSpeed, targetSpeed, timeSince1 / timeToCrouchSpeed); 
            timeSince1 += Time.deltaTime;
            yield return null;
        }

        // A validation check to ensure the speed of the player is correctly set.
        speed = targetSpeed;

        // The boolean that indecated the current state is inverted.
        crouching2 = !crouching2;
    }

    // Creates the coroutine responsible for applying the airDash boost
    // to the player.
    private IEnumerator airDashSpeed()
    {
        //  Defines a blank float used for counting how much time has passed.
        float timeSince2 = 0; 

        // Defines how long the speed transition should take
        float dashTime = 0.1f;

        // Over the set amount of time the controller speed is boosted up
        // to the target speed and the timer will be incremented by the time since the last frame.
        while (timeSince2 < dashTime) 
        {
            speed = Mathf.Lerp(speed, 200, timeSince2 / dashTime);
            timeSince2 += Time.deltaTime;  
            yield return null;
        }
    }

    }
