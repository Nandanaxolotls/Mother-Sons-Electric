using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerDoorTest : MonoBehaviour
{
    [Header("References")]
    public Transform object1; // will move on X
    public Transform object2; // will rotate on X
    public StepManagerP2T stepManagerP2;
    public GameObject Tooltip1;

    [Header("Target Values")]
    public float object1TargetX = -1.3412f;
    public float object2TargetXRot = 180f;

    [Header("Input")]
    public InputActionProperty selectAction;

    [Header("Animation Settings")]
    public float moveDuration = 1f;
    public float rotateDuration = 1f;

    public event System.Action onReachedOriginal;
    public event System.Action onReachedDesired;

    private bool isHovered = false;
    private bool isInDesiredPosition = false;
    private bool isLocked = false;
    private bool isPermanantlyLocked = false;

    private Vector3 object1OriginalPos;
    private Quaternion object2OriginalRot;

    private Coroutine activeRoutine;

    void Start()
    {
        if (object1 != null) object1OriginalPos = object1.localPosition;
        if (object2 != null) object2OriginalRot = object2.localRotation;
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
        if (Tooltip1 != null)
        {
            Tooltip1.SetActive(false);
        }
        if (isPermanantlyLocked) return;
        if (isInDesiredPosition && isLocked) return;

        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        if (!isInDesiredPosition)
        {
            activeRoutine = StartCoroutine(MoveThenRotate(
                new Vector3(object1TargetX, object1OriginalPos.y, object1OriginalPos.z),
                Quaternion.Euler(object2TargetXRot, 0f, 0f),
                true
            ));
        }
        else
        {
            activeRoutine = StartCoroutine(MoveThenRotate(
                object1OriginalPos,
                object2OriginalRot,
                false
            ));
        }

        isInDesiredPosition = !isInDesiredPosition;
    }

    private IEnumerator MoveThenRotate(Vector3 targetPos1, Quaternion targetRot2, bool goingToDesired)
    {
        // --- Step 1: Move object1 ---
        Vector3 startPos1 = object1.localPosition;
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            object1.localPosition = Vector3.Lerp(startPos1, targetPos1, t);
            yield return null;
        }
        object1.localPosition = targetPos1;

        // --- Step 2: Rotate object2 ---
        Quaternion startRot2 = object2.localRotation;
        elapsed = 0f;
        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotateDuration);
            object2.localRotation = Quaternion.Slerp(startRot2, targetRot2, t);
            yield return null;
        }
        object2.localRotation = targetRot2;

        // --- Step 3: Events + Lock ---
        if (goingToDesired)
        {
            isLocked = true;
            onReachedDesired?.Invoke();
            stepManagerP2.DoorClosed();
        }
        else
        {
            onReachedOriginal?.Invoke();
            // stepManagerP2.DoorClosed();
        }
    }

    public void Unlock()
    {
        isLocked = false;

    }

    public void PermanantlyLock()
    {
        isPermanantlyLocked = true;
    }
}
