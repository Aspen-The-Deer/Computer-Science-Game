using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Player_Movement : MonoBehaviour // Creating a public class 'Player_Movement' which will be applied to the player controller
{
    public CharacterController controller; // Creating a public variable that defines which game object is the character controller

    public float speed = 15f; // Creating a public float that defines the speed of the player character
    public float gravity = -9.81f; // Creating a public float that defines the strength of gravity on the player character
    public float jumpHeight = 2f; // Creating a public float that defines the height at which the player can jump
    public int jumpsRemaining = 1; // Creating a public integer that defines the number of times the player is able to jump before returning to the ground 

    public Transform groundCheck; // Creates a transformation that can be defined as an in game object used for ground checking
    public float groundDistance = 0.5f; // Defines a radius around the ground check object to see when the player is close to or on the ground
    public LayerMask groundMask; // Defines a layer mask that will be an identifier for ground objects

    public Transform wallL; // Creates a transformation that can be defined as an in game object used for checking for a wall on the left side of the player
    public Transform wallR; // Creates a transformation that can be defined as an in game object used for checking for a wall on the right side of the player
    public float wallDistance = 0.5f; // Defines a radius around the ground check object to see when the player is close to or on a wall
    public LayerMask wallMask; // Defines a layer mask that will be an identifier for wall objects

    Vector3 velocity; // Defines a vector movement that connects to the velocity of the player
    bool onFloor; // Defines a boolean that indicates if the player is touching the ground
    bool onWallA; // Defines a boolean that indicates if the player is touching the wall
    bool onWallB; // Defines a boolean that indicates if the player is touching the wall

    // Update is called once per frame
    void Update()
    {
        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player
        onFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Creates a 'Check Sphere' around an empty game object to detect the floor when within range of the player

        if(onWallA && velocity.x < 0) // If the player is on a left hand wall, and their velocity is greater than 0
        {
            velocity.y = -1f; // Minimises player gravity when wall running to allow the player to stay on the wall
            jumpsRemaining = 2; // Resets the player jump counter for when coming off the wall
        }

        if (onWallB && velocity.x < 0) // If the player is on a right hand wall, and their velocity is greater than 0
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

        if(Input.GetButtonDown("Jump") && (jumpsRemaining > 0)) // If 'Space' is pressed, and there are more than 0 jumps remaining
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // A math calculation to calculate the jump height of the player based on preset values
            jumpsRemaining -= 1; // Removes 1 from jumps remaining
        }

        if(onFloor) 
        {
            jumpsRemaining = 1; // Resets the number of jumps remaining for the player when they touch the ground again
        }

        velocity.y += gravity * Time.deltaTime; // Calculation to determine acceleration under the current gravity over time

        controller.Move(velocity * Time.deltaTime); // Calculation for moving the character controller based on current

    }
}
