using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Enemy_Script : MonoBehaviour
{

    // Defines a transform that can be used
    // to refer to the player's position.
    public Transform player;

    // Defines a rigidbody that can be used
    // to refer to the enemy's rigidbody controller
    // and itself as a gameobject.
    private Rigidbody enemy;
    GameObject self;

    // Defines the speed at which the enemy can move.
    public float enemySpeed = 5f;

    // Defining Transforms for where the player check will be anchored and
    // distance from which the enemy will check for a player.
    // A layer mask is also created to determine what surfaces to look for.
    public Transform playerCheck; 
    public float playerDistance = 10f; 
    public LayerMask playerMask; 
    public bool playerInRange;

    // Defining Transforms for where the projectile checks will be anchored and
    // distance from which the enemy will check for a projectile.
    // A layer mask is also created to determine what surfaces to look for.
    public Transform projectileCheck;
    public bool projectileInRange;
    public float projectileDetectDistance = 2f;
    public LayerMask projectileMask;

    // Defines variables for use in calculating damage
    // taken and health.
    public float health = 100;
    public bool damageTaken = true;
    

    // Start is called before the first frame update
    void Start()
    {
        
        // Defines the enemy rigidbody as the current game object.
        enemy = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // Ensures the enemy is accelerated towards the ground under correct force of gravity.
        enemy.AddForce(Physics.gravity, ForceMode.Acceleration);

        // Checks for a player within a specified range.
        playerInRange = Physics.CheckSphere(playerCheck.position, playerDistance, playerMask);

        // If there is a player in range, the enemy will rotate to face the player
        // and it's velocity is set to move towards the player.
        if (playerInRange)
        {
            enemy.transform.LookAt(player);
            enemy.velocity = transform.forward * enemySpeed;
        }

        // Checks for a projectile within a specified range.
        projectileInRange = Physics.CheckSphere(projectileCheck.position, projectileDetectDistance, projectileMask);

        // If there is no nearby projectile, the enemy is able to take damage.
        if (!projectileInRange)
        {
            damageTaken = false;
        }

        // If there is a projectile in range,
        // and the enemy has not yet taken damage.
        // The enemy will take a random amount of damage
        // between 15 and 20 HP.
        if (projectileInRange && !damageTaken)
        {
            health -= Random.Range(15,20);
            damageTaken = true;
        }
    }

    // After each frame update,
    // Check to see if health is <= 0
    private void LateUpdate()
    {
        // If the enemy health is less than or
        // equal to 0, Destroy the enemy.
        if (health <= 0)
        {
            self = this.gameObject;
            Destroy(self);
        }
    }
}
