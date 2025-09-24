using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PunchingMachine : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;   // The object to rotate
    public Transform targetObject2;  // The second object to move
    public GameManager gameManager;
    public GameObject Tooltip1;
   // public GameObject Tooltip2;

    [Header("Target Rotation Values (for targetObject)")]
    public float targetX = 90f;   // starting X
    public float oppositeX = -90f; // opposite X

    [Header("Target Movement Values (for targetObject2)")]
    public float moveYStart = 0f;   // original Y position
    public float moveYEnd = 2f;     // target Y position

    [Header("Input")]
    public InputActionProperty selectAction;

    [Header("Animation Settings")]
    public float halfRotationDuration = 0.5f; // time for each half-step
    public float moveDuration = 1f;           // time for Y-axis movement

    public event System.Action onReachedDesired;
    public event System.Action onReachedOriginal;

    private bool isHovered = false;
    private bool isRunning = false;
    private Coroutine actionCoroutine;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.localEulerAngles = new Vector3(targetX, 90f, -90f);
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
        if (isRunning) return;

        if (isHovered && selectAction.action.WasPressedThisFrame())
        {
            if (actionCoroutine != null) StopCoroutine(actionCoroutine);
            actionCoroutine = StartCoroutine(DoCycle());
        }
    }

    private IEnumerator DoCycle()
    {
        isRunning = true;

        // Forward phase (desired position)
        if (Tooltip1 != null) Tooltip1.SetActive(false);
       // if (Tooltip2 != null) Tooltip2.SetActive(true);

        yield return StartCoroutine(RunSimultaneous(
            RotateStepwise(targetX, oppositeX),
            MoveY(targetObject2, moveYStart, moveYEnd)
        ));

        onReachedDesired?.Invoke();

        // Pause at forward
        yield return new WaitForSeconds(0.5f);

        // Backward phase (return to original)
     //   if (Tooltip2 != null) Tooltip2.SetActive(false);

        yield return StartCoroutine(RunSimultaneous(
            RotateStepwise(oppositeX, targetX),
            MoveY(targetObject2, moveYEnd, moveYStart)
        ));

        onReachedOriginal?.Invoke();

        isRunning = false;
    }
    private IEnumerator RunSimultaneous(IEnumerator a, IEnumerator b)
    {
        bool aDone = false, bDone = false;

        StartCoroutine(Wrap(a, () => aDone = true));
        StartCoroutine(Wrap(b, () => bDone = true));

        while (!aDone || !bDone)
            yield return null;
    }

    private IEnumerator Wrap(IEnumerator routine, System.Action onComplete)
    {
        yield return StartCoroutine(routine);
        onComplete?.Invoke();
    }


    private IEnumerator RotateStepwise(float fromX, float toX)
    {
        // Step 1: rotate to neutral (0, -90, 0)
        yield return StartCoroutine(SmoothRotate(new Vector3(0f, 90f, -90f)));

        // Step 2: rotate to destination
        yield return StartCoroutine(SmoothRotate(new Vector3(toX, 90f, -90f)));
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

        Vector3 startPos = new Vector3(obj.localPosition.x, fromY, obj.localPosition.z);
        Vector3 endPos = new Vector3(obj.localPosition.x, toY, obj.localPosition.z);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            obj.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        obj.localPosition = endPos;
    }
}
