using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Mouse_Look : MonoBehaviour 
{
    // Defines a float value that will be used
    // as a multiplier for mouse sesnsitivity.
    public float mouseSensitivity = 1000f; 

    // Defines a Transform which can be used
    // to reference the position stats of the player itself.
    public Transform playerBody;

    // Defines a set of floats which will be
    // responsible for storing the rotation values for the camera.
    float xRotation = 0f; 
    float zRotation = 0f;
    
    // Defining Transforms for where the wall run checks will be anchored and
    // distance from which the player will check for a wall.
    // A layer mask is also created to determine what surfaces to look for.
    public Transform wallL;
    public Transform wallR;
    public float wallDistance = 0.5f;
    public LayerMask wallMask; 

    // Creates a set of booleans that can be referred to
    // for the validation of the player's state.
    bool onWallA;
    bool onWallB;
    
    // A set if variables used to validate the
    // rotation of the camera when wallrunning.
    float maxRotationL = -10f;
    float maxRotationR = 10f; 
    float rotationIncrament = 0.0625f;
    bool onWall;

    // Start is called before the first frame
    void Start()
    {
        // Locks the player's mouse so that it is invisible and stays anchored to the game window.
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        // Defines mouse input for Left and Right, and the speed at which it has moved
        // as well as mouse input for Up and Down, and the speed at which it has moved
        // Time.deltaTime is a function that calls the change in time since the last frame.
        // Multiplying by this stops players with a higher framerate being able to turn faster
        // despite no change in sensitivity.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; 
                                                                                                                                                                     
        // -= mouseY to give correct rotation (Not inverted)
        xRotation -= mouseY;

        // Locks the camera movement on the Y axis to a 180 degree span
        // so the camera always stays the right way up. 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);      

        // Transforms the rotation of the camera based on the movment vectors defined above
        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation); 
        playerBody.Rotate(Vector3.up * mouseX);
        
        onWallA = Physics.CheckSphere(wallL.position, wallDistance, wallMask);
        onWallB = Physics.CheckSphere(wallR.position, wallDistance, wallMask);

        // If wallrunning (on a left wall or a right wall)
        if (onWallA | onWallB)
        {
            onWall = true; 
        }
        else
        {
            onWall = false;
        }

        // If on a left wall,
        // rotate the camera to the left until at max rotation.
        if (onWallA) 
        {
            while(zRotation != maxRotationL) 
            {
                zRotation -= (rotationIncrament);
            }
        }

        // If on a right wall,
        // rotate the camera to the left until at max rotation.
        if (onWallB) 
        {
            while (zRotation != maxRotationR) 
            {
                zRotation += rotationIncrament; 
            }
        }

        // If no longer on a wall, rotate the camera back to it's
        // default Z rotation, moving in the direction that returns
        // it to normal the fastest.
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
    }
}
