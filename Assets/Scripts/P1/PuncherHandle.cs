using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PuncherHandle : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;   // The object to rotate
    public Transform targetObject2;  // The second object to move
    public GameManager gameManager;
    public GameObject Tooltip1;
    public GameObject Tooltip2;

    [Header("Target Rotation Values (for targetObject)")]
    public float targetZ = 90f;   // starting Z (e.g., 90)
    public float oppositeZ = -90f; // opposite Z (e.g., -90)

    [Header("Target Movement Values (for targetObject2)")]
    public float moveYStart = 0f;   // original Y position
    public float moveYEnd = 2f;     // target Y position

    [Header("Input")]
    public InputActionProperty selectAction;

    [Header("Animation Settings")]
    public float halfRotationDuration = 0.5f; // time for each half-step (90?0 or 0?-90)
    public float moveDuration = 1f;           // time for Y-axis movement

    public event System.Action onReachedDesired;
    public event System.Action onReachedOriginal;

    private bool isHovered = false;
    private bool goingToOpposite = true; // state toggle
    private Coroutine rotationCoroutine;
    private Coroutine moveCoroutine;

    // ?? After returning to original once, lock further toggles
    public bool locked = false;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.localEulerAngles = new Vector3(0f, -90f, targetZ);
        }

        if (targetObject2 != null)
        {
            Vector3 pos = targetObject2.localPosition;
            targetObject2.localPosition = new Vector3(pos.x, moveYStart, pos.z);
        }
    }

    public void OnHoverEntered(HoverEnterEventArgs args) => isHovered = true;
    public void OnHoverExited(HoverExitEventArgs args) => isHovered = false;

    void Update()
    {
        if (locked) return; // ?? Prevent any movement when locked

        if (isHovered && selectAction.action.WasPressedThisFrame())
        {
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);

            if (goingToOpposite)
            {
                // Go to desired
                rotationCoroutine = StartCoroutine(RotateStepwise(targetZ, oppositeZ, true));
                moveCoroutine = StartCoroutine(MoveY(targetObject2, moveYStart, moveYEnd));
                if (Tooltip1 != null)
                {
                    Tooltip1.SetActive(false);
                }
                if (Tooltip2 != null)
                {
                    Tooltip2.SetActive(true);
                }
            }
            else
            {
                // Return to original
                if (Tooltip2 != null)
                {
                    Tooltip2.SetActive(false);
                }
                rotationCoroutine = StartCoroutine(RotateStepwise(oppositeZ, targetZ, false));
                moveCoroutine = StartCoroutine(MoveY(targetObject2, moveYEnd, moveYStart));
                
            }

            goingToOpposite = !goingToOpposite;
        }
    }

    private IEnumerator RotateStepwise(float fromZ, float toZ, bool goingToDesired)
    {
        // Step 1: rotate from current to 0
        yield return StartCoroutine(SmoothRotate(new Vector3(0f, -90f, 0f)));

        // Step 2: rotate from 0 to destination
        yield return StartCoroutine(SmoothRotate(new Vector3(0f, -90f, toZ)));

        // ? Invoke after finishing
        if (goingToDesired)
        {
            onReachedDesired?.Invoke();
        }
        else
        {
            onReachedOriginal?.Invoke();
            locked = true; // ?? Lock only after returning to original
            Debug.Log("Handle locked after returning to original.");
        }
    }

    private IEnumerator SmoothRotate(Vector3 targetRotation)
    {
        Quaternion startRotation = targetObject.localRotation;
        Quaternion endRotation = Quaternion.Euler(targetRotation);
        float elapsed = 0f;

        while (elapsed < halfRotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfRotationDuration);
            targetObject.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        targetObject.localRotation = endRotation;
    }

    private IEnumerator MoveY(Transform obj, float fromY, float toY)
    {
        if (obj == null) yield break;

        Vector3 startPos = obj.localPosition;
        Vector3 endPos = new Vector3(startPos.x, toY, startPos.z);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            obj.localPosition = Vector3.Lerp(new Vector3(startPos.x, fromY, startPos.z), endPos, t);
            yield return null;
        }

        obj.localPosition = endPos;
    }

    // ? Call this from another script to unlock
    public void UnlockHandle()
    {
        locked = false;
        Debug.Log("Handle unlocked by external script.");
    }
}
