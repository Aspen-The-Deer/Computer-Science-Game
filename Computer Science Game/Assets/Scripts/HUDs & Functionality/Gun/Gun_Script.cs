using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Gun_Script : MonoBehaviour
{

    // Creates a game object to be used
    // as reference to created projectiles.
    public GameObject projectile;

    // Creates a reference to
    // the camera and gunTip game objects.
    public Transform gunTip; 
    public new Camera camera;

    // Defines a float which can be referenced
    // as the velocity given to a fired projectile.
    public float projForce;

    // Defines a float which determines the
    // amount of time between each new projectile fired.
    public float fireRate = 0.5f;

    // Defines a float which determines how many
    // bullets can be shot before needing to reload.
    public float ammo = 10;

    // The booleans below define shooting
    // states of the player's weapon.
    public bool readyToShoot = true;
    public bool reloading = false;
    public bool allowInvoke = true;

    // Creating a reference to the
    // Ammo count HUD elelment.
    public Ammo_Count ammoCount;

    // Update is called once per frame
    void Update()
    {

        // If left mouse button is pressed and the player is not grappling, call the Fire Gun function.
        if (Input.GetButton("Fire1") && readyToShoot && ammo > 0 && !reloading && !Input.GetButton("Fire2"))
        {
            FireGun();
        }

        // When the reload button is pressed call the Reload coroutine.
        if(Input.GetButtonDown("Reload") && !reloading)
        {
            StartCoroutine(Reload());
        }

        //If the player is out of ammo and they're ready to fire again,
        //call the reload function.
        if (ammo <= 0 && readyToShoot)
        {
            StartCoroutine(Reload());
        }

        // Sets the ammo counter on the hud
        // to equal the ammo count rounded
        // to the nearest int.
        ammoCount.setAmmo((int)ammo);
    }

    // Start Fire Gun Function
    void FireGun()
    {

        // Take away 1 ammo and take away
        // the ability to fire while already firing.
        ammo--;
        readyToShoot = false;

        // Creates a new physics raycast from the centre of the player's vision.
        Vector3 target;
        Ray aim = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // If the raycast hits a valid point, the target is equal to the hit point.
        if (Physics.Raycast(aim, out RaycastHit hit))
        {
            target = hit.point;
        }
        // Otherwise the target point is set at a default of 100 units away.
        else
        {
            target = aim.GetPoint(100);
        }

        // A new Vector3 is created which aims the projectile at the target.
         Vector3 direction = target - gunTip.position;

        // A new instance of the projectile prefab is created at the tip of the gun.
        GameObject currentProjectile = Instantiate(projectile, gunTip.position, Quaternion.identity);

        // The rotation of the projectile is set to the Vector3 direction.
        currentProjectile.transform.forward = direction.normalized;

        // Takes the current projectile and sets its velocity in the direction of the target to be equal to the projectile force.
        currentProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projForce, ForceMode.Impulse);

        // If continuous fire is allowed
        if (allowInvoke)
        {
            // Call the Fire Rate function for a duration
            // equal to the fire rate.
            // And allow invoke is set to false to ensure
            // it is only called once.
            Invoke(nameof(FireRate), fireRate);
            allowInvoke = false;
        }

    }

    // Start Fire Rate Function
    private void FireRate()
    {
        // Allow the next shot to be fired
        // and the invoke function to be called again.
        readyToShoot = true;
        allowInvoke = true;
    }

    // Creates the coroutine responsible for
    // reloading the player's weapon
    private IEnumerator Reload()
    {
        readyToShoot = false;
        reloading = true;

        // Defines the time it will take to reload
        float reloadTime = 2;

        //  Defines a blank float used for counting how much time has passed.
        float timeSince = 0;

        // Over a set amount of time, the ammo count will increase back to 10
        // and the timer will be incremented by the time since the last frame.
        while (timeSince < reloadTime)
        {
            ammo = Mathf.Lerp(ammo, 10, timeSince / reloadTime);
            timeSince += Time.deltaTime;
            ammoCount.setAmmo((int)ammo);
            yield return null;
        }
        readyToShoot = true;
        reloading = false;
    }
}
