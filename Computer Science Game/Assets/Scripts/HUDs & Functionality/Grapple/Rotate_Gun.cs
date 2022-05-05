using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Rotate_Gun : MonoBehaviour
{

    // Creates a public reference to the grappling script.
    public Grappling_Hook Grappling;

    // Defines a rotation vector for referencing
    // the desired gun rotation.
    // And the speed at which the gun rotates.
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {

        // If not grappling, the desired rotation is the default position.
        if (!Grappling.IsGrappling())
        {
            desiredRotation = transform.parent.rotation;
        }
        // Otherwise the desired rotation is the angle from the current rotation that faces the grapple point.
        else
        {
            desiredRotation = Quaternion.LookRotation(Grappling.GetGrapplePoint() - transform.position);
        }

        // Using a lerp function the gun rotation is slowly changed to match the desired rotation.
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }


}
