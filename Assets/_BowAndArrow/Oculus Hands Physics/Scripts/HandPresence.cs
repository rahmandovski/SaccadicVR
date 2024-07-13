using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;    
    private InputDevice targetDevice;
    public Animator handAnimator;

    [Space]
    public float trigger;
    public float grip;
    public bool primary;
    public bool secondary;

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
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
            trigger = triggerValue;
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
            trigger = 0;
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
            grip = gripValue;
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
            grip = 0;
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue))
        {
            primary = primaryValue;
        }
        else
        {
            primary = false;
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryValue))
        {
            secondary = secondaryValue;
        }
        else
        {
            secondary = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }
    }
}
