using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    Camera cam;
    public Transform target;
    public float lerpSpeed = 0.125f;
    public Vector3 offset = new Vector3(0,-10,15);
    
    float startFOV;

    void Start() 
    {   
        cam = GetComponent<Camera>();
        startFOV = cam.fieldOfView;
    }
    
    void LateUpdate()
    {
        Vector3 lerpPos = Vector3.Lerp(transform.position, target.position-offset, lerpSpeed);
        transform.position = lerpPos;
        transform.LookAt(target.position);
    }
}
