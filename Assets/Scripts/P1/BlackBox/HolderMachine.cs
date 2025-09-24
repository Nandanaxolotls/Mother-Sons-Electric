using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HolderMachine : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject1;
    public Transform targetObject2;
    public GameManager gameManager;

    [Header("Target Rotation Values (Object 1)")]
    public int YValue1;
    public int ZValue1;

    [Header("Target Rotation Values (Object 2)")]
    public int YValue2;
    public int ZValue2;

    [Header("Input")]
    public InputActionProperty selectAction;

    [Header("Animation Settings")]
    public float rotationDuration = 1f;

    public enum RotationDirection { Clockwise, Anticlockwise }
    [Tooltip("Choose rotation direction for both objects")]
    public RotationDirection rotationDirection = RotationDirection.Clockwise;

    public event System.Action onReachedOriginal;
    public event System.Action onReachedDesired;

    private bool isHovered = false;
    private bool isInDesiredPosition = false;
    private bool isLocked = false;
    private bool isPermanantlyLocked = false;

    private Vector3 originalRotation1;
    private Vector3 originalRotation2;

    private Coroutine rotationCoroutine;

    void Start()
    {
        if (targetObject1 != null)
            originalRotation1 = targetObject1.localEulerAngles;

        if (targetObject2 != null)
            originalRotation2 = targetObject2.localEulerAngles;
    }

    public void OnHoverEntered(HoverEnterEventArgs args) => isHovered = true;
    public void OnHoverExited(HoverExitEventArgs args) => isHovered = false;

    void Update()
    {
        if (isHovered && selectAction.action.WasPressedThisFrame())
        {
            TogglePosition();
        }
    }

    private void TogglePosition()
    {
        if (targetObject1 == null && targetObject2 == null) return;
        if (isPermanantlyLocked) return;
        if (isInDesiredPosition && isLocked) return;

        // Stop old coroutine
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(SmoothRotate(!isInDesiredPosition));

        isInDesiredPosition = !isInDesiredPosition;
    }

    private IEnumerator SmoothRotate(bool goingToDesired)
    {
        Quaternion startRot1 = targetObject1 != null ? targetObject1.localRotation : Quaternion.identity;
        Quaternion startRot2 = targetObject2 != null ? targetObject2.localRotation : Quaternion.identity;

        Quaternion endRot1;
        Quaternion endRot2;

        if (goingToDesired)
        {
            // Move to desired rotations
            if (rotationDirection == RotationDirection.Clockwise)
            {
                endRot1 = Quaternion.Euler(new Vector3(0f, YValue1, ZValue1));
                endRot2 = Quaternion.Euler(new Vector3(0f, YValue2, ZValue2));
            }
            else
            {
                endRot1 = Quaternion.Euler(new Vector3(0f, -YValue1, -ZValue1));
                endRot2 = Quaternion.Euler(new Vector3(0f, -YValue2, -ZValue2));
            }
        }
        else
        {
            // Back to original
            endRot1 = Quaternion.Euler(originalRotation1);
            endRot2 = Quaternion.Euler(originalRotation2);
        }

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);

            if (targetObject1 != null)
                targetObject1.localRotation = Quaternion.Slerp(startRot1, endRot1, t);

            if (targetObject2 != null)
                targetObject2.localRotation = Quaternion.Slerp(startRot2, endRot2, t);

            yield return null;
        }

        if (targetObject1 != null) targetObject1.localRotation = endRot1;
        if (targetObject2 != null) targetObject2.localRotation = endRot2;

        if (goingToDesired)
        {
            onReachedDesired?.Invoke();
            isLocked = true;
        }
        else
        {
            onReachedOriginal?.Invoke();
        }
    }

    public void Unlock() => isLocked = false;
    public void PermanantlyLock() => isPermanantlyLocked = true;
}
