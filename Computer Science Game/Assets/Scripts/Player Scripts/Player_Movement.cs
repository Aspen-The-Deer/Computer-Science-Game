using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Player_Movement : MonoBehaviour // Creating a public class 'Player_Movement' which will be applied to the player controller
{
    public CharacterController controller; // Creating a public variable that defines which game object is the character controller

    public float defaultSpeed = 15f; // Creating a public float that defines the speed of the player character
    public float crouchingSpeed = 10f;
    public float speed;
    public float gravity = -9.81f; // Creating a public float that defines the strength of gravity on the player character
    public float jumpHeight = 3f; // Creating a public float that defines the height at which the player can jump
    public int jumpsRemaining = 1; // Creating a public integer that defines the number of times the player is able to jump before returning to the ground 

    public Transform groundCheck; // Creates a transformation that can be defined as an in game object used for ground checking
    public Transform crouchingGroundCheck;
    public float groundDistance = 0.5f; // Defines a radius around the ground check object to see when the player is close to or on the ground
    public LayerMask groundMask; // Defines a layer mask that will be an identifier for ground objects

    public Transform wallL; // Creates a transformation that can be defined as an in game object used for checking for a wall on the left side of the player
    public Transform wallR; // Creates a transformation that can be defined as an in game object used for checking for a wall on the right side of the player
    public float wallDistance = 0.4f; // Defines a radius around the ground check object to see when the player is close to or on a wall
    public LayerMask wallMask; // Defines a layer mask that will be an identifier for wall objects

    Vector3 velocity; // Defines a vector movement that connects to the velocity of the player
    bool onFloor; // Defines a boolean that indicates if the player is touching the ground
    bool onWallA; // Defines a boolean that indicates if the player is touching the wall
    bool onWallB; // Defines a boolean that indicates if the player is touching the wall

    public bool crouching;
    bool crouchAnimation;
    float standingHeight = 2f;
    float crouchingHeight = 0.25f;
    Vector3 standingCentre = new Vector3(0, 0, 0);
    Vector3 crouchingCentre = new Vector3(0, 0.5f, 0);
    float timeToCrouch = 0.25f;
    float timeToCrouchSpeed = 4f;

    // Update is called once per frame
    void Update()
    {

        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player

        if (!crouching)
        {
            onFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Creates a 'Check Sphere' around an empty game object to detect the floor when within range of the player;
        }
        else onFloor = Physics.CheckSphere(crouchingGroundCheck.position, groundDistance, groundMask)  ; // Creates a 'Check Sphere' around an empty game object to detect the floor when within range of the player

        if(onWallA & !onFloor & !Input.GetButtonDown("Jump")) // If the player is on a left hand wall, and their velocity is greater than 0
        {
            velocity.y = -1f; // Minimises player gravity when wall running to allow the player to stay on the wall
            jumpsRemaining = 2; // Resets the player jump counter for when coming off the wall
        }

        if (onWallB & !onFloor & !Input.GetButtonDown("Jump")) // If the player is on a right hand wall, and their velocity is greater than 0
        {
            velocity.y = -1f; // Minimises player gravity when wall running to allow the player to stay on the wall
            jumpsRemaining = 2; // Resets the player jump counter for when coming off the wall
        }

        if (onFloor && velocity.y < 0) // If the player is on the floor, and their velocity is 0
        {
            velocity.y = -2f; // Applies a slight downward force to the player to keep them correctly grounded
        }

        float xMove = Input.GetAxis("Horizontal"); // Movement across the X axis defined by the horizontal movement input
        float zMove = Input.GetAxis("Vertical"); // Movement across the Z axis defined by the vertical movement input

        Vector3 move = transform.right * xMove + transform.forward * zMove; // Converts the movement input into a 3D vector named 'Move'

        controller.Move(move * speed * Time.deltaTime); // Applies the movement vector to the character controller, multiplying by speed to determine how fast to move

        if (Input.GetButtonDown("Jump") && (jumpsRemaining > 0) && !crouching) // If 'Space' is pressed, and there are more than 0 jumps remaining
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // A math calculation to calculate the jump height of the player based on preset values
            jumpsRemaining -= 1; // Removes 1 from jumps remaining
        }

        if(onFloor) 
        {
            jumpsRemaining = 1; // Resets the number of jumps remaining for the player when they touch the ground again
        }

        if (Input.GetButtonDown("Crouch") && !crouchAnimation)
        {
            StartCoroutine(toCrouch());
        }
        
        if (Input.GetButtonDown("Crouch") && !crouchAnimation)
        {
            StartCoroutine(toCrouchSpeed());
        }

        velocity.y += gravity * Time.deltaTime; // Calculation to determine acceleration under the current gravity over time

        controller.Move(velocity * Time.deltaTime); // Calculation for moving the character controller based on velocity, using delta time to prevent different FPS affecting speed

    }

    private IEnumerator toCrouch()
    {
        crouchAnimation = true;

        float timeSince = 0;
        float targetHeight = crouching ? standingHeight : crouchingHeight;
        float currentHeight = controller.height;
        Vector3 targetCentre = crouching ? standingCentre : crouchingCentre;
        Vector3 currentCentre = controller.center;
        float targetSpeed = crouching ? defaultSpeed : crouchingSpeed;

        while (timeSince < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeSince / timeToCrouch);
            controller.center = Vector3.Lerp(currentCentre, targetCentre, timeSince / timeToCrouch);
            timeSince += Time.deltaTime;
            yield return null;
        }

        controller.center = targetCentre;
        controller.height = targetHeight;

        crouching = !crouching;

        crouchAnimation = false;
    }
    
    private IEnumerator toCrouchSpeed()
    {
        float targetSpeed = crouching ? defaultSpeed : crouchingSpeed;
        float currentSpeed;
        if (!crouching)
        {
            currentSpeed = (defaultSpeed + 10);
        }
        else currentSpeed = crouchingSpeed;

        while (timeSince < timeToCrouchSpeed)
        {
            speed = Mathf.Lerp((defaultSpeed + 5), targetSpeed, timeSince / timeToCrouchSpeed);
            timeSince += Time.deltaTime;
            yield return null;
        }

        speed = targetSpeed; 
    }
