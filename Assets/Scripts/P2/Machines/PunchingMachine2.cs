using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PunchingMachine2 : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;    // Object 1: rotates in Z
    public Transform targetObject2;   // Object 2: moves in Y
    public Transform targetObject3;   // Object 3: moves in Z
    public GameManager gameManager;
    public GameObject Tooltip8;

    [Header("Target Values (Object1)")]
    public float targetZ = 90f;   // starting Z
    public float oppositeZ = -90f; // opposite Z

    [Header("Target Values (Object2)")]
    public float moveYStart = 0f;   // original Y
    public float moveYEnd = 2f;     // target Y

    [Header("Target Values (Object3)")]
    public float moveZStart = 0f;   // original Z
    public float moveZEnd = 2f;     // target Z

    [Header("Input")]
    public InputActionProperty selectAction;

    [Header("Animation Settings")]
    public float halfRotationDuration = 0.5f;
    public float moveDuration = 1f;

    public event System.Action onReachedDesired;
    public event System.Action onReachedOriginal;

    private bool isHovered = false;
    private bool isRunning = false;
    private Coroutine actionCoroutine;

    void Start()
    {
        if (targetObject != null)
            targetObject.localEulerAngles = new Vector3(0f, 90f, targetZ);

        if (targetObject2 != null)
        {
            Vector3 pos = targetObject2.localPosition;
            targetObject2.localPosition = new Vector3(pos.x, moveYStart, pos.z);
        }

        if (targetObject3 != null)
        {
            Vector3 pos = targetObject3.localPosition;
            targetObject3.localPosition = new Vector3(pos.x, pos.y, moveZStart);
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

        if (Tooltip8 != null) Tooltip8.SetActive(false);

        // Step 1: Move Object3 forward
        yield return StartCoroutine(MoveZ(targetObject3, moveZStart, moveZEnd));

        // Step 2: Move Object1 + Object2 simultaneously
        yield return StartCoroutine(RunSimultaneous(
            RotateStepwise(targetZ, oppositeZ),
            MoveY(targetObject2, moveYStart, moveYEnd)
        ));

        onReachedDesired?.Invoke();

        yield return new WaitForSeconds(0.5f);

        // Step 3: Return Object1 + Object2 simultaneously
        yield return StartCoroutine(RunSimultaneous(
            RotateStepwise(oppositeZ, targetZ),
            MoveY(targetObject2, moveYEnd, moveYStart)
        ));

        onReachedOriginal?.Invoke();

        // Step 4: Return Object3 to original
        yield return StartCoroutine(MoveZ(targetObject3, moveZEnd, moveZStart));

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

    private IEnumerator RotateStepwise(float fromZ, float toZ)
    {
        // Step 1: rotate to neutral (0, 0, 0)
        yield return StartCoroutine(SmoothRotate(new Vector3(0f, 90f, 180f)));

        // Step 2: rotate to destination
        yield return StartCoroutine(SmoothRotate(new Vector3(0f, 90f, toZ)));
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

    private IEnumerator MoveZ(Transform obj, float fromZ, float toZ)
    {
        if (obj == null) yield break;

        Vector3 startPos = new Vector3(obj.localPosition.x, obj.localPosition.y, fromZ);
        Vector3 endPos = new Vector3(obj.localPosition.x, obj.localPosition.y, toZ);
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
