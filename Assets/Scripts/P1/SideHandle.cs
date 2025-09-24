using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SideHandle : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject; // The object to rotate
    public GameManager gameManager;

    [Header("Target Rotation Values")]
    public int YValue;
    public int ZValue;

    [Header("Input")]
    public InputActionProperty selectAction;

    [Header("Animation Settings")]
    public float rotationDuration = 1f; // Time to complete the rotation

    public enum RotationDirection { Clockwise, Anticlockwise }
    [Tooltip("Choose rotation direction for this object")]
    public RotationDirection rotationDirection = RotationDirection.Clockwise;

    public event System.Action onReachedOriginal;
    public event System.Action onReachedDesired;

    private bool isHovered = false;
    private bool isInDesiredPosition = false; // track toggle state
    private bool isLocked = false;            // NEW: prevents going back
    private bool isPermanantlyLocked = false;
    private Vector3 originalRotation;         // store original local rotation
    private Coroutine rotationCoroutine;      // store reference to active coroutine

    void Start()
    {
        if (targetObject != null)
        {
            originalRotation = targetObject.localEulerAngles; // Save starting rotation
        }
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
        if (targetObject == null) return;
        if (isPermanantlyLocked) return;
        // If it's already at desired position and locked ? don't allow going back
        if (isInDesiredPosition && isLocked)
            return;

        // Decide target rotation
        Vector3 targetRotation;
        if (!isInDesiredPosition)
        {
            if (rotationDirection == RotationDirection.Clockwise)
                targetRotation = new Vector3(0f, YValue, ZValue);
            else
                targetRotation = new Vector3(0f, -YValue, -ZValue);
        }
        else
        {
            targetRotation = originalRotation; // back to original
        }

        // Stop existing coroutine if running
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(SmoothRotate(targetRotation, !isInDesiredPosition));

        isInDesiredPosition = !isInDesiredPosition; // flip state
    }

    private IEnumerator SmoothRotate(Vector3 targetRotation, bool goingToDesired)
    {
        Quaternion startRotation = targetObject.localRotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            targetObject.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        targetObject.localRotation = endRotation; // ensure exact final rotation

        if (goingToDesired)
        {
            onReachedDesired?.Invoke();
            isLocked = true;  // Lock when desired position is reached
        }
        else
        {
            onReachedOriginal?.Invoke();
        }
    }

    // Call this externally to unlock
    public void Unlock()
    {
        isLocked = false;
    }

    public void PermanantlyLock()
    {
        isPermanantlyLocked = true;
    }
}
