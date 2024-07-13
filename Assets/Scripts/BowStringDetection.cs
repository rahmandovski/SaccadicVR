using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringDetection : MonoBehaviour
{
    public Transform bowPivot;
    public BowControll bowControl;
    public bool isHandInside;

    private void Update()
    {
        transform.SetPositionAndRotation(bowPivot.position, bowPivot.rotation);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<XRDirectInteractor>() && bowControl)
        {
            isHandInside = true;
        }
    }
}
