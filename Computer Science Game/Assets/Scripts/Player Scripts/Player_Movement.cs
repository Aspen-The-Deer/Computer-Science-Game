using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool onFloor;

    // Update is called once per frame
    void Update()
    {

        onFloor = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(onFloor && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xMove + transform.forward * zMove;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && onFloor)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
