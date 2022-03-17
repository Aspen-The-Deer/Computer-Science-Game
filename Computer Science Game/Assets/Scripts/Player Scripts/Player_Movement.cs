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
        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask);
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask);
        onFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(onWallA && velocity.y < 0)
        {
            velocity.y = -1f;
            jumpsRemaining = 2;
        }

        if (onWallB && velocity.y < 0)
        {
            velocity.y = -1f;
            jumpsRemaining = 2;
        }

        if (onFloor && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xMove + transform.forward * zMove;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && (jumpsRemaining > 0))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsRemaining -= 1;
        }

        if(onFloor)
        {
            jumpsRemaining = 1;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
