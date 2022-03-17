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

    float maxRotationL = -10f;
    float maxRotationR = 10f;
    float rotationIncrament = 0.0625f;
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

        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask);
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask);

        if(onWallA | onWallB)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        if(onWallA)
        {
            while(zRotation != maxRotationL)
            {
                zRotation -= rotationIncrament;
            }
        }

        if (onWallB)
        {
            while (zRotation != maxRotationR)
            {
                zRotation += rotationIncrament;
            }
        }

        if(onWall == false)
        {
            if(zRotation < 0)
            {
                zRotation += rotationIncrament;
            }
            else if(zRotation > 0)
            {
                zRotation -= rotationIncrament;
            }
        }

        xRotation -= mouseY; // -= mouseY to give correct rotation (Not inverted)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Locks the camera movement on the Y axis to a 180 degree span        

        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation); // Mathmatic function to calculate player rotation on the X axis
        playerBody.Rotate(Vector3.up * mouseX); // Mathmatic function to call in mouse movement calculations for conversion into a player rotation

    }
}