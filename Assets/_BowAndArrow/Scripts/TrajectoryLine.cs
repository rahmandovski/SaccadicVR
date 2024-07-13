using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public float velocity;
    public float step;

    public LineRenderer lineRenderer;
    public Vector3[] trajactoryPositions;

    private void Start()
    {
        trajactoryPositions = new Vector3[lineRenderer.positionCount];
    }

    public void SetVelocity(float v)
    {
        velocity = v;

        lineRenderer.enabled = velocity > 0;
        if (velocity < 0) return;
        Vector3 speed = transform.forward * velocity;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            trajactoryPositions[i] = transform.position + (speed * (step * i) - (-Physics.gravity) * Mathf.Pow((step * i), 2) / 2);
            lineRenderer.SetPosition(i, trajactoryPositions[i]);
        }
    }
}
