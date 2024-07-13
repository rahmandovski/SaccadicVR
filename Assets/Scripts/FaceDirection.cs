using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    public Transform forward;
    public float threshold;
    public GameObject warning;

    Vector3 currentForward;

    private void Update()
    {
        currentForward = transform.forward;
        warning.SetActive(Vector3.Distance(forward.forward, currentForward) > threshold);
    }
}
