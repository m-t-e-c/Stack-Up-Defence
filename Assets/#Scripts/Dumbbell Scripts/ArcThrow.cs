using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcThrow : MonoBehaviour
{
    /*
    This script is used for throwing objects in a perfect arc.
    Attach this script to the object that you want to throw.
    Attach ArcThrowTrajectory to trajectoryPrefab if you want to visualize the arc.
    Propell force is dependent of the object's mass.
    Throw objects by calling Throw() function.
    */

    Rigidbody rb;
    GameObject trajectoryObject;
    ArcThrowTrajectory throwTrajectory;

    public GameObject trajectoryPrefab;
    public float propellForce = 750f;
    float forceMultiplier = 2f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 force)
    {
        if(rb==null) return;
        rb.AddForce(new Vector3(force.x, force.y, force.y)*forceMultiplier);
        Vector3 forceInit = force;
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y))*forceMultiplier;
        if(trajectoryPrefab != null) throwTrajectory.UpdateTrajectory(forceV,rb,transform.position);
    }
}
