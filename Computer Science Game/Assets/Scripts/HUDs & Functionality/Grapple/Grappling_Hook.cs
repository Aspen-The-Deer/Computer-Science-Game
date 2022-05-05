using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ^ Default unity functionality requirements

public class Grappling_Hook : MonoBehaviour
{

    // Creates a reference to the line renderer
    // used for drawing a rope from the grapple gun tip
    // to the grapple point.
    private LineRenderer lr;

    // Creates a Vector3 to be referenced as
    // the attatchment point for the grapple gun.
    private Vector3 grapplePoint;

    // Defines a layermask of what surfaces
    // can be grappled to.
    public LayerMask whatCanGrapple;

    // Creates transforms to be referenced as
    // positioning of gameobjects.
    public Transform gunTip, player;
    public new Transform camera;

    // Defines the max distance of the 
    // grapple hook attachment.
    public float maxDistance = 100;

    // Creates a reference to the spring joint
    // used to attatch the player to a grapple point.
    // The springjoint component is what allows the rigidbody
    // to swing from a singular point.
    private SpringJoint joint;

    // Upon start of the script
    void Awake()
    {
        // Set the lr variable to the Line Renderer component.
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // If Right Mouse Button Pressed,
        // call the start grapple function.
        if (Input.GetButtonDown("Fire2"))
        {
            StartGrapple();
        }
        // When RMB released,
        // call the stop grapple function.
        else if (Input.GetButtonUp("Fire2"))
        {
            StopGrapple();
        }
    }

    // After each frame update,
    // call the draw rope function.
    void LateUpdate()
    {
        DrawRope();
    }

    // Start Grapple function
    void StartGrapple()
    {

        // If a physics raycast returns a valid hit on a valid surface
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxDistance, whatCanGrapple))
        {
            
            // Set the grapple point to the hit position of the raycast
            // and calculate the distance of the player from the grapple point.
            grapplePoint = hit.point;
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // Defining the properties of the springjoint and adding it as a component.
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            // Set the count of attatcment positions
            lr.positionCount = 2;
        }
    }

    // Start Draw Rope function
    void DrawRope()
    {
        // If there is no active joint, do nothing.
        if (!joint) return;

        // Otherwise set one end  of the line render
        // to the tip of the grapple gun, and the other
        // to the grapple point.
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    // Start Stop Grapple function
    void StopGrapple()
    {

        // Set the count of attatchment positions
        lr.positionCount = 0;

        // Destroy the springjoint.
        Destroy(joint);
    }

    // Creates a bool that returns the
    // state of the springjoint game object.
    public bool IsGrappling()
    {
        return joint != null;
    }

    // Creates a Vector3 that returns the
    // grapplePoint Vector3.
    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
