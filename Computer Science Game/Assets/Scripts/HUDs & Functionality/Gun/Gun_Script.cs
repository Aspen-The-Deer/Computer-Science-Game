using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Script : MonoBehaviour
{


    public GameObject projectile;
    public Transform gunTip;
    public new Camera camera;
    public float projForce, upForce;
    public float fireRate = 0.1f;
    public float ammo = 10;
    public bool readyToShoot = true;
    public bool reloading = false;
    public bool allowInvoke = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && readyToShoot && ammo > 0 && !reloading)
        {
            fireGun();
        }

        if(Input.GetButtonDown("Reload") && !reloading)
        {
            StartCoroutine(reload());
        }

        if (ammo <= 0 && readyToShoot)
        {
            StartCoroutine(reload());
        }
    }

    void fireGun()
    {
        ammo--;
        readyToShoot = false;

        RaycastHit hit;
        Vector3 target;
        Ray aim = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(aim, out hit))
        {
            target = hit.point;
        }
        else
        {
            target = aim.GetPoint(100);
        }

        Vector3 direction = target - gunTip.position;
        GameObject currentProjectile = Instantiate(projectile, gunTip.position, Quaternion.identity);
        currentProjectile.transform.forward = direction.normalized;

        currentProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * projForce, ForceMode.Impulse);

        if (allowInvoke)
        {
            Invoke("FireRate", fireRate);
            allowInvoke = false;
        }
    }

    private void FireRate()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private IEnumerator reload()
    {
        readyToShoot = false;
        reloading = true;
        float reloadTime = 4;
        float timeSince = 0;

        while (timeSince < reloadTime) // While the time passed is less than the allocated
        {
            ammo = Mathf.Lerp(ammo, 10, timeSince / reloadTime);  // Over the allocated time, translate the current ammo count from current to target
            timeSince += Time.deltaTime;  // Incraments the time passed by how much time has passed
            yield return null;
        }
        readyToShoot = true;
        reloading = false;
    }
}
