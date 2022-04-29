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

    private void Update()
    {

        enemyInRange = Physics.CheckSphere(enemyCheck.position, enemyDistance, enemyMask);
        if (enemyInRange)
        {
            StartCoroutine(enemyDamage());
        }

    }

    private IEnumerator enemyDamage()
    {
        while (enemyInRange)
        {
            playerHealth -= 5;
            yield return new WaitForSeconds(100);
        }
    }

}
