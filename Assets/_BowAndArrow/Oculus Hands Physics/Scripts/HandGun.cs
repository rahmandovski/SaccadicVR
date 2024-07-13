using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandGun: MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    public Animator handAnimator;
    public ArcheryGameManager gameManager;

    [Space]
    public float grip;

    [Space]
    public Transform gunDirection;
    public LineRenderer lineRenderer;
    public float lineDistance;
    public LayerMask targetLayer;

    [Space]
    public AudioSource hitSound;

    public bool shoot;

    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            if(triggerValue > 0.5)
            {
                if (!shoot)
                {
                    shoot = true;
                }
            }
        }

        handAnimator.SetFloat("Trigger", 0);
        handAnimator.SetFloat("Grip", grip);
        Vector3 forwardDir = gunDirection.forward * lineDistance;
        lineRenderer.SetPosition(0, gunDirection.position);
        lineRenderer.SetPosition(1, gunDirection.position + forwardDir);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }

        if (shoot)
        {
            if(Physics.Raycast(gunDirection.position, gunDirection.forward, out RaycastHit hit, 1000, targetLayer))
            {
                hitSound.Play();

                gameManager.TargetShooted(hit.point);
                gameManager.NextTarget(hit.transform.parent.gameObject);
                hitSound.Play();
            }
            shoot = false;
        }
    }

}
