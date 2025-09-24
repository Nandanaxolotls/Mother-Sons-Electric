using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    [Header("Input Bindings")]
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    [Header("Animator")]
    public Animator handAnimator;

    [Header("Custom Animation Settings")]
    [Tooltip("List of tags that trigger special animation")]
    public List<string> specialTags; // <-- Multiple special tags

    public string specialAnimBool = "NearSpecial";  // Animator bool parameter

    private int touchingSpecialCount = 0;

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsSpecialTag(other.tag))
        {
            touchingSpecialCount++;
            handAnimator.SetBool(specialAnimBool, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsSpecialTag(other.tag))
        {
            touchingSpecialCount = Mathf.Max(0, touchingSpecialCount - 1);
            if (touchingSpecialCount == 0)
                handAnimator.SetBool(specialAnimBool, false);
        }
    }

    private bool IsSpecialTag(string tag)
    {
        return specialTags.Contains(tag);
    }
}
