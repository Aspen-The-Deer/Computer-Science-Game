using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
 
    public float playerHealth = 100;
    public Transform enemyCheck;
    public float enemyDistance = 1f;
    public LayerMask enemyMask; 
    public bool enemyInRange;

    public LayerMask trapMask;
    public Transform trapCheck;
    public float trapDistance = 0.1f;
    public bool trapInRange;

    public Health_Bar healthBar;

    GameObject self;

    private void Update()
    {
   
        enemyInRange = Physics.CheckSphere(enemyCheck.position, enemyDistance, enemyMask);
        trapInRange = Physics.CheckSphere(trapCheck.position, trapDistance, trapMask);
        if (enemyInRange)
        {
            enemyDamage();
        }

        if (playerHealth <= 0)
        {
            playerDeath();
        }

        if (trapInRange)
        {
            trapDamage();
        }
    }


    private void trapDamage()
    {
        float healthReduction = Time.deltaTime;
        playerHealth -= (healthReduction * 50);
        updateBar();
    }
    private void enemyDamage()
    {
        float healthReduction = Time.deltaTime;
        playerHealth -= (healthReduction*15);
        updateBar();
    }

    private void playerDeath()
    {
        self = this.gameObject;
        Destroy(self);
    }

    void updateBar()
    {
        float roundedHealth = Mathf.Round(playerHealth);
        healthBar.SetHealth((int)(roundedHealth));
    }

}
