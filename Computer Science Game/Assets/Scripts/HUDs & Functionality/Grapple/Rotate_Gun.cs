using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Gun : MonoBehaviour
{
    public Grappling_Hook Grappling;

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    void Update()
    {
        if (!Grappling.IsGrappling())
        {
            desiredRotation = transform.parent.rotation;
        }
        else
        {
            desiredRotation = Quaternion.LookRotation(Grappling.GetGrapplePoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }


}
