using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Projectile_Destroy : MonoBehaviour
{
    // Creates a game object that
    // can be used as a reference to the projectile.
    GameObject self;

    // If the projectile collides with any solid surface.
    private void OnTriggerEnter(Collider other)
    {
        // Set the game object self to
        // the current object and destroy it.
        self = this.gameObject;
        Destroy(self);
    }

    // Update is called once per frame
    private void Update()
    {
        // Set the game object self
        // to the current object and destroy it
        // after two seconds.
        self = this.gameObject;
        Destroy(self, 2f);
    }
}
