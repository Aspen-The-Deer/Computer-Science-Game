using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Mouse_Look : MonoBehaviour // Creating a public class 'Mouse_Look' which will be applied to the player controller
{
    public float mouseSensitivity = 1000f;  // Defines a public variable which represents the sensitivity of the mouse

    public Transform playerBody; // Defines 'playerBody' to be a transformation linked to the unity character controller

    float xRotation = 0f; // Sets the default orientation of the x camera axis  to 0
    float zRotation = 0f;

    public Transform wallL; // Creates a transformation that can be defined as an in game object used for checking for a wall on the left side of the player
    public Transform wallR; // Creates a transformation that can be defined as an in game object used for checking for a wall on the right side of the player
    public float wallDistance = 0.5f; // Defines a radius around the ground check object to see when the player is close to or on a wall
    public LayerMask wallMask; // Defines a layer mask that will be an identifier for wall objects

    bool onWallA; // Defines a boolean that indicates if the player is touching the wall
    bool onWallB; // Defines a boolean that indicates if the player is touching the wall

    float maxRotationL = -10f; // Defines the max rotation angle when on a left handed wallrun
    float maxRotationR = 10f; // Defines the max rotation angle when on a left handed wallrun
    float rotationIncrament = 0.0625f; // Defines the incrament value at which the camera will rotate
    bool onWall;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the player's mouse so that it is invisible and stays anchored to the game window
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Defines mouse input for Left and Right, and the speed at which it has moved
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Defines mouse input for Up and Down, and the speed at which it has moved
                                                                                     // Time.deltaTime is a function that calls the change in time since the last frame.
                                                                                     // Multiplying by this stops players with a higher framerate being able to turn faster despite no change in sensitivity

        xRotation -= mouseY; // -= mouseY to give correct rotation (Not inverted)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Locks the camera movement on the Y axis to a 180 degree span

        xRotation -= mouseY; // -= mouseY to give correct rotation (Not inverted)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Locks the camera movement on the Y axis to a 180 degree span        

        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation); // Mathmatic function to calculate player rotation on the X axis
        playerBody.Rotate(Vector3.up * mouseX); // Mathmatic function to call in mouse movement calculations for conversion into a player rotation
        
        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask); // Creates a 'Check Sphere' around an empty game object to detect a wall within range of the player

        if(onWallA | onWallB) // If wallrunning (on a left wall or a right wall)
        {
            onWall = true; 
        }
        else
        {
            onWall = false;
        }

        if(onWallA) // If on a left wall
        {
            while(zRotation != maxRotationL) // Loop until the z Rotation of the camera matches the max tilt angle
            {
                zRotation -= (rotationIncrament); // Incraments the camera z tilt by the specified ammount
            }
        }

        if (onWallB) // If on a right wall
        {
            while (zRotation != maxRotationR) // Loop until the x Rotation of the camera matches the max tilt angle
            {
                zRotation += rotationIncrament; // Incraments the camera z tilt by the specified ammount
            }
        }

        if(onWall == false) // If not wallrunning
        {
            if(zRotation < 0) // If z rotation is negative
            {
                zRotation += rotationIncrament; // Incrament the z camera Rotation up until it is 0
            }
            else if(zRotation > 0) // If the z rotation is positive
            {
                zRotation -= rotationIncrament; // Incrament the z camera Rotation down until it is 0
            }
        }
    }
}
