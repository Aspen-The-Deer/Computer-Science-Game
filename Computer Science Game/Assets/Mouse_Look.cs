using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Look : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Defines a public variable which represents the sensitivity of the mouse

    public Transform playerBody; // Defines 'playerBody' to be a transformation linked to the unity character controller

    float xRotation = 0f; // Sets the default orientation of each camera axis to 0

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

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Mathmatic function to calculate player rotation on the X axis
        playerBody.Rotate(Vector3.up * mouseX); // Mathmatic function to call in mouse movement calculations for conversion into a player rotation
    }
}
