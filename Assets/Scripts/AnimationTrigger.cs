using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MonoBehaviour
{
    public UnityEvent trigger;

    public void Trigger()
    {
        trigger.Invoke();
    }
}
