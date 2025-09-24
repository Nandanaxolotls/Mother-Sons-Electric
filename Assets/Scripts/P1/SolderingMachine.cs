using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SolderingMachine : MonoBehaviour
{
    [Header("References")]
    public Transform object1; // moves on Z
    public Transform object2; // moves on X/Y
    public GameManager gameManager;
    public ParticleSystem myParticles;
    public GameObject Tooltip5;

    [Header("Target Positions")]
    public float object1ZTarget = 2f;   // target Z movement for object1
    public float object2YTarget = 2f;   // first Y target
    public float object2XTarget = 1f;   // X target
    public float object2YSecondTarget = 3f; // second Y target

    [Header("Animation Settings")]
    public float moveDuration = 1f;     // time for each move
    public float waitBetweenSteps = 1f; // pause between steps

    [Header("Input")]
    public InputActionProperty selectAction;

    public event System.Action onProcessComplete;

    private bool isHovered = false;
    private Vector3 obj1OriginalPos;
    private Vector3 obj2OriginalPos;
    private Coroutine processCoroutine;

    void Start()
    {
        if (object1 != null) obj1OriginalPos = object1.localPosition;
        if (object2 != null) obj2OriginalPos = object2.localPosition;
    }

    public void OnHoverEntered(HoverEnterEventArgs args) => isHovered = true;
    public void OnHoverExited(HoverExitEventArgs args) => isHovered = false;

    void Update()
    {
        if (isHovered && selectAction.action.WasPressedThisFrame())
        {
            Call();
        }
    }

    public void Call()
    {
        Tooltip5.SetActive(false);
        if (processCoroutine != null) StopCoroutine(processCoroutine);
        processCoroutine = StartCoroutine(ProcessSequence(2)); // run 2 times
    }

    private IEnumerator ProcessSequence(int repeatCount)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            // Step 1: Object1 move on Z
            yield return StartCoroutine(MoveTo(object1, new Vector3(obj1OriginalPos.x, obj1OriginalPos.y, object1ZTarget)));

            // Step 2: Object2 move up on Y
            yield return StartCoroutine(MoveTo(object2, new Vector3(obj2OriginalPos.x, object2YTarget, obj2OriginalPos.z)));

            yield return new WaitForSeconds(waitBetweenSteps);

            // Step 3: Object2 return to original Y
            yield return StartCoroutine(MoveTo(object2, obj2OriginalPos));

            // Step 4: Object2 move on X
            yield return StartCoroutine(MoveTo(object2, new Vector3(object2XTarget, obj2OriginalPos.y, obj2OriginalPos.z)));

            // Step 5: Object2 move to second Y target
            yield return StartCoroutine(MoveTo(object2, new Vector3(object2XTarget, object2YSecondTarget, obj2OriginalPos.z)));

            if (myParticles != null)
            {
                myParticles.Play();
                yield return new WaitForSeconds(1f); // let it run for 1 second
                myParticles.Stop();
            }

            yield return new WaitForSeconds(1);

            // Step 6: Object2 return to original Y (keeping X offset)
            yield return StartCoroutine(MoveTo(object2, new Vector3(object2XTarget, obj2OriginalPos.y, obj2OriginalPos.z)));

            yield return new WaitForSeconds(waitBetweenSteps);

            // Step 7: Return Object2 fully to original position
            yield return StartCoroutine(MoveTo(object2, obj2OriginalPos));

            // Step 8: Return Object1 to original Z
            yield return StartCoroutine(MoveTo(object1, obj1OriginalPos));
        }

        // ? Final event called after finishing all repeats
        onProcessComplete?.Invoke();
    }

    private IEnumerator MoveTo(Transform obj, Vector3 targetPos)
    {
        if (obj == null) yield break;
        Vector3 start = obj.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            obj.localPosition = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }
        obj.localPosition = targetPos;
    }
}
