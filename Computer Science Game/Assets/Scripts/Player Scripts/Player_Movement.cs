using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Player_Movement : MonoBehaviour // Creating a public class 'Player_Movement' which will be applied to the player controller
{
    public CharacterController controller; // Creating a public variable that defines which game object is the character controller

    public float defaultSpeed = 20f; // Creating a public float that defines the speed of the player character
    public float crouchingSpeed = 15f;
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

    public bool crouching; // Defines a boolean that indicates if the player is crouching
    public bool crouching2; // Defines a boolean that indicates if the player is crouching (alternate)
    public bool crouchAnimation; // Defines a boolean that indicates if the player is in the crouching animation
    float standingHeight = 2f; // Defines the standing height of the player
    float crouchingHeight = 0.25f; // Defines the crouching height of the player
    Vector3 standingCentre = new Vector3(0, 0, 0); // Defines a set of local coordinates which works as the centre of the player model when standing
    Vector3 crouchingCentre = new Vector3(0, 0.5f, 0); // Defines a set of local coordinates which works as the centre of the player model when crouching
    float timeToCrouch = 0.125f; // Defines how long the transition between standing and crouching will take
    public float timeToCrouchSpeed = 1.5f; // Defines how long the transition between walking speed and crouching speed will take

    // Update is called once per frame
    void Update()
    {

        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player

        if (!crouching)
        {
            onFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Creates a 'Check Sphere' around an empty game object to detect the floor when within range of the player;
        }
        else onFloor = Physics.CheckSphere(crouchingGroundCheck.position, groundDistance, groundMask); // Creates a 'Check Sphere' around an empty game object to detect the floor when within range of the player

        if (onWallA & !onFloor & !Input.GetButtonDown("Jump") & !crouching) // If the player is on a left hand wall, and their velocity is greater than 0
        {
            velocity.y = -1f; // Minimises player gravity when wall running to allow the player to stay on the wall
            jumpsRemaining = 2; // Resets the player jump counter for when coming off the wall
        }

        if (onWallB & !onFloor & !Input.GetButtonDown("Jump") & !crouching) // If the player is on a right hand wall, and their velocity is greater than 0
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
        else if (Input.GetButtonDown("Jump") && (jumpsRemaining > 0) && crouching && !crouchAnimation) // If 'Space is pressed, and the player is crouching
        {
            StartCoroutine(toCrouch()); // Calls a routine responsible for toggling the crouch state for the player model
            StartCoroutine(toCrouchSpeed()); // Calls a routine responsible for toggling the crouch state for the player speed
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // A math calculation to calculate the jump height of the player based on preset values
            jumpsRemaining -= 1; // Removes 1 from jumps remaining
        }

            if (onFloor)
        {
            jumpsRemaining = 1; // Resets the number of jumps remaining for the player when they touch the ground again
        }

        if (Input.GetButtonDown("Crouch") && !crouchAnimation && onFloor) // If the 'C' key is pressed, and the player is grounded
        {
            StartCoroutine(toCrouch()); // Calls a routine responsible for toggling the crouch state for the player model
            StartCoroutine(toCrouchSpeed()); // Calls a routine responsible for toggling the crouch state for the player speed
        }

        if (!crouching) // if the player is not crouching
        {
            speed = defaultSpeed; // Set speed to its default
        }

        if (!crouching2) // If the player is not crouching
        {
            timeToCrouchSpeed = 1.5f; // Set the multiplier for crouch speed change to 1.5s
        }
        else timeToCrouchSpeed = 0f; // Set the multiplier for crouch speed change to be instant

        velocity.y += gravity * Time.deltaTime; // Calculation to determine acceleration under the current gravity over time

        controller.Move(velocity * Time.deltaTime); // Calculation for moving the character controller based on velocity, using delta time to prevent different FPS affecting speed

    }

    private IEnumerator toCrouch()
    {
        crouchAnimation = true; // Indecates the beginning of the transition between standing and crouching

        float timeSince = 0; // Defines a float variable used to calculate time passed
        float targetHeight = crouching ? standingHeight : crouchingHeight; // Defines a float for the players target height depending on if they are crouching or not
        float currentHeight = controller.height; // Takes the current height of the controller as a float
        Vector3 targetCentre = crouching ? standingCentre : crouchingCentre; // Defines a float for the players target centre depending on if they are crouching or not
        Vector3 currentCentre = controller.center; // Takes the current centre of the controller as a float

        while (timeSince < timeToCrouch) // While the time passed is less than the allocated
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeSince / timeToCrouch); // Over the allocated time, translate the current height from current to target
            controller.center = Vector3.Lerp(currentCentre, targetCentre, timeSince / timeToCrouch); // Over the allocated time, translate the current centre from current to target
            timeSince += Time.deltaTime; // Incraments the time passed by how much time has passed
            yield return null;
        }

        controller.center = targetCentre; // Ensures the centre is correctly set to the target
        controller.height = targetHeight; // Ensures the height is correctly set to the target

        crouching = !crouching; // Flips the value of the couching bool

        crouchAnimation = false; // Indecates the end of the transition between standing and crouching
    }

    private IEnumerator toCrouchSpeed()
    {
        float timeSince1 = 0; // Defines a float variable used to calculate time passed
        float targetSpeed = crouching ? defaultSpeed : crouchingSpeed; // Defines a float for the players target speed depending on if they are crouching or not
        float currentSpeed; // Creates an empty float value for the players current speed

        if (!crouching2) // If not crouching
        {
            currentSpeed = (defaultSpeed + 10); // Current speed is set to equal the default + a boost
        }
        else currentSpeed = crouchingSpeed; // If crouching, current speed is set to equal the crouching speed

        while (timeSince1 < timeToCrouchSpeed) // While the time passed is less than the allocated
        {
            speed = Mathf.Lerp(currentSpeed, targetSpeed, timeSince1 / timeToCrouchSpeed); // Over the allocated time, translate the current speed from current to target
            timeSince1 += Time.deltaTime; // Incraments the time passed by how much time has passed
            yield return null;
        }

        speed = targetSpeed; // Ensures the speed is correctly set to the target

        crouching2 = !crouching2; // Flips the value of the couching2 bool
    }
}
