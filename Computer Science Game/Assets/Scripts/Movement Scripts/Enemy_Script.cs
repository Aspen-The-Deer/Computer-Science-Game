using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{

    public Transform player;
    private Rigidbody enemy;
    public float enemySpeed = 5f;

    public Transform playerCheck; // Creates a transformation that can be defined as an in game object used for checking for a wall on the right side of the player
    public float playerDistance = 10f; // Defines a radius around the player check object to see when the enemy is within range of the player
    public LayerMask playerMask; // Defines a layer mask that will be an identifier for player objects
    public bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {

        enemy = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        enemy.AddForce(Physics.gravity, ForceMode.Acceleration);
        playerInRange = Physics.CheckSphere(playerCheck.position, playerDistance, playerMask);

        if (playerInRange)
        {
            enemy.AddForce(Physics.gravity, ForceMode.Acceleration);
            enemy.transform.LookAt(player);
            enemy.velocity = transform.forward * enemySpeed;
        }
        

    }
}
