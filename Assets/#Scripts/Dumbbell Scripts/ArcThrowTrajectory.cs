using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcThrowTrajectory : MonoBehaviour
{
    [Header("Trajectory Variables")]
    LineRenderer trajectoryRenderer;
    [SerializeField] int trajectoryPOICount = 20;
    List<Vector3> trajectoryPoints = new List<Vector3>();

    void Start()
    {
        trajectoryRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {
        Vector3 velocity = (forceVector/rigidbody.mass)*Time.fixedDeltaTime;
        float flightDuration = (2*velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration/ trajectoryPOICount;
        trajectoryPoints.Clear();
        RaycastHit hit;
        for(int i = 0; i< trajectoryPOICount; i++)
        {
            float stepTimePassed = stepTime * i;
            Vector3 MovementVector = new Vector3( velocity.x * stepTimePassed,
                                                  velocity.y * stepTimePassed - 0.5f * Physics.gravity.y*stepTimePassed*stepTimePassed,
                                                  velocity.z * stepTimePassed);
            trajectoryPoints.Add(-MovementVector + startingPoint);
        }
        trajectoryRenderer.positionCount = trajectoryPoints.Count;
        trajectoryRenderer.SetPositions(trajectoryPoints.ToArray());
    }
}
