using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Player_Health : MonoBehaviour
{
    // Set the float value for the player's health to 100.
    public float playerHealth = 100;

    // Defining Transforms for where the enemyCheck will be anchored and
    // distance from which the player will check for an enemy.
    // A layer mask is also created to determine what surfaces to look for.
    public Transform enemyCheck;
    public float enemyDistance = 1f;
    public LayerMask enemyMask; 
    public bool enemyInRange;

    // Defining Transforms for where the trapChecks will be anchored and
    // distance from which the player will check for a trap.
    // A layer mask is also created to determine what surfaces to look for.
    public LayerMask trapMask;
    public Transform trapCheck;
    public float trapDistance = 0.1f;
    public bool trapInRange;

    // Creating a reference to the Health Bar HUD element.
    public Health_Bar healthBar;

    // Creating a reference to itself as a gameobject.
    GameObject self;

    // Update is called once per frame
    private void Update()
    {
        
        // There is an enemy in range if a surface of the correct type (layer mask)
        // in range of the bottom of the player.
        enemyInRange = Physics.CheckSphere(enemyCheck.position, enemyDistance, enemyMask);

        // There is an trap in range if a surface of the correct type (layer mask)
        // in range of the bottom of the player.
        trapInRange = Physics.CheckSphere(trapCheck.position, trapDistance, trapMask);

        // If enemy is in range,
        // call the enemyDamage function.
        if (enemyInRange)
        {
            enemyDamage();
        }

        // If player health reaches 0,
        // call the playerDeath function.
        if (playerHealth <= 0)
        {
            playerDeath();
        }


        // If trap is in range,
        // call the trapDamage function.
        if (trapInRange)
        {
            trapDamage();
        }
    }

    // Start trapDamage function.
    private void trapDamage()
    {
        // Take the value of time since last frame,
        // and multiply by the damage to get how much health should be taken away.
        // Then call the updateBar HUD function.
        float healthReduction = Time.deltaTime;
        playerHealth -= (healthReduction * 50);
        updateBar();
    }

    // Start enemyDamage function.
    private void enemyDamage()
    {
        // Take the value of time since last frame,
        // and multiply by the damage to get how much health should be taken away.
        // Then call the updateBar HUD function.
        float healthReduction = Time.deltaTime;
        playerHealth -= (healthReduction*15);
        updateBar();
    }

    // Start playerDeath function.
    private void playerDeath()
    {
        // Set the game object self
        // to the current object and destroy it.
        self = this.gameObject;
        Destroy(self);
    }

    // Start updateBar function.
    void updateBar()
    {
        // Set the value of the health bar to the player's
        // current health rounded to the nearest int.
        float roundedHealth = Mathf.Round(playerHealth);
        healthBar.SetHealth((int)(roundedHealth));
    }

}
