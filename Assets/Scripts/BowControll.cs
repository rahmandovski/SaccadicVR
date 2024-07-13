using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Dweiss;


public class BowControll : MonoBehaviour
{
    public InputDeviceCharacteristics leftControllerCharacteristics;
    private InputDevice leftTargetDevice;


    public InputDeviceCharacteristics rightControllerCharacteristics;
    private InputDevice rightTargetDevice;

    [Space]
    [Space]
    public Transform leftHand;
    public Transform rightHand;

    [Space]
    [Space]
    public Transform midStringTrack;
    public Transform bowPivot;
    public Transform midString;
    public ArrowControl hoveredArrow;
    public AudioSource shootSound;
    public AudioSource ropeSound;
    public float arrowSpeedMultiplier;
    public bool isSelected;
    public bool isReadyToShoot;

    public List<ArrowControl> arrows;

    [Space]
    public Vector3[] trajectoryPositions;

    public float lastRope;
    Transform firstParent;
    Vector3 firstPosition;
    Quaternion firstRotation;

    public void SetSelected(bool cond)
    {
        isSelected = cond;
    }

    private void Start()
    {
        trajectoryPositions = new Vector3[100];
        TryInitialize();

        firstParent = transform.parent;
        firstPosition = transform.position;
        firstRotation = transform.rotation;

        ShowNextArrow();
    }

    public void ShowNextArrow()
    {
        hoveredArrow = arrows[0].GetComponent<ArrowControl>();
        arrows[0].GetComponent<ArrowControl>().AttachToBow(bowPivot);
        arrows[0].gameObject.SetActive(true);
    }

    void TryInitialize()
    {
        if(!leftTargetDevice.isValid)
        {
            List<InputDevice> leftDevices = new List<InputDevice>();

            InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, leftDevices);
            if (leftDevices.Count > 0)
            {
                leftTargetDevice = leftDevices[0];
            }
        }

        if(!rightTargetDevice.isValid)
        {
            List<InputDevice> rightDevices = new List<InputDevice>();

            InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, rightDevices);
            if (rightDevices.Count > 0)
            {
                rightTargetDevice = rightDevices[0];
            }
        }

    }

    void UpdateBow()
    {
        if (hoveredArrow)
        {
            bool isLefttrigger = false;
            if(leftTargetDevice.TryGetFeatureValue(CommonUsages.grip, out float leftTriggerValue))
            {
                isLefttrigger = (leftTriggerValue > 0.5);
            }

            bool isRighttrigger = false;
            if (rightTargetDevice.TryGetFeatureValue(CommonUsages.grip, out float rightTriggerValue))
            {
                isRighttrigger = (rightTriggerValue > 0.5);
            }


            bool isLeftHandTrigger = false;
            if (leftTargetDevice.TryGetFeatureValue(CommonUsages.trigger, out float leftHandTriggerValue))
            {
                isLeftHandTrigger = (leftHandTriggerValue > 0.5);
            }
            bool isRightHandTrigger = false;
            if (rightTargetDevice.TryGetFeatureValue(CommonUsages.trigger, out float rightHandTriggerValue))
            {
                isRightHandTrigger = (rightHandTriggerValue > 0.5);
            }

            if (isLefttrigger && isRighttrigger)
            {
                Transform target = null;
                if(Vector3.Distance(midString.position, rightHand.position) < 0.1)
                {
                    target = rightHand;
                }
                else if (Vector3.Distance(midString.position, leftHand.position) < 0.1)
                {
                    target = leftHand;
                }

                if(target)
                {
                    Vector3 localPosition = midStringTrack.InverseTransformPoint(target.position);
                    localPosition.x = 0;
                    localPosition.y = 0;
                    localPosition.z = Mathf.Clamp(localPosition.z, -0.5f, 0);
                    Vector3 worldPosition = midStringTrack.TransformPoint(localPosition);
                    midString.position = worldPosition;

                    float v = Mathf.Abs(localPosition.z / 0.5f);

                    if(v > lastRope)
                    {
                        if(!ropeSound.isPlaying)
                        ropeSound.Play();
                    }

                    lastRope = v;

                    isReadyToShoot = v > 0.1;
                    if (isReadyToShoot)
                    {
                        hoveredArrow.trajectoryRenderer.SetVelocity(v * arrowSpeedMultiplier);
                    }
                    else hoveredArrow.trajectoryRenderer.SetVelocity(0);

                }
                else
                {
                    isReadyToShoot = false;
                    midString.position = midStringTrack.position;
                    hoveredArrow.trajectoryRenderer.SetVelocity(0);
                }
            }
            else if (isLefttrigger && isLeftHandTrigger && isSelected)
            {
                isReadyToShoot = true;
                hoveredArrow.trajectoryRenderer.SetVelocity(arrowSpeedMultiplier);
            }
            else if (isRighttrigger && isRightHandTrigger && isSelected)
            {
                isReadyToShoot = true;
                hoveredArrow.trajectoryRenderer.SetVelocity(arrowSpeedMultiplier);
            }
            else
            {
                if (isReadyToShoot)
                {
                    shootSound.Play();
                    hoveredArrow.Shoot();
                    arrows.Remove(hoveredArrow);
                    hoveredArrow = null;
                    StartCoroutine(WaitToNextArrow());
                }
                else
                {
                    hoveredArrow.trajectoryRenderer.SetVelocity(0);
                }

                isReadyToShoot = false;
                midString.position = midStringTrack.position;
            }
        }
        else
        {
            isReadyToShoot = false;
            midString.position = midStringTrack.position;
        }
    }

    IEnumerator WaitToNextArrow()
    {
        yield return new WaitForSeconds(0.3f);

        ShowNextArrow();
    }

    private void Update()
    {
        TryInitialize();

        UpdateBow();
    }

    public void ResetBow()
    {
        transform.position = firstPosition;
        transform.rotation = firstRotation;
        transform.SetParent(firstParent);
    }

    public void ReadyToLaunch(ArrowControl arrow)
    {
        arrow.gameObject.SetActive(false);
        arrows.Add(arrow);
    }
}
