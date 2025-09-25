using System.Collections;
using UnityEngine;

public class LaserMachine : MonoBehaviour
{
    [Header("References")]
    public Transform object1; // Moves on X
    public Transform object2; // Moves on Z

    [Header("Target Movement Values")]
    public float object1XEnd = 2f;  // target local X for object1
    public float object2ZEnd = 2f;  // target local Z for object2

    public float moveDuration = 1f;

    private Vector3 object1StartPos;
    private Vector3 object2StartPos;

    public event System.Action LaserMachineDone;

    private void Start()
    {
        if (object1 != null) object1StartPos = object1.localPosition;
        if (object2 != null) object2StartPos = object2.localPosition;
    }

    // Called from another script
    public void StartProcess()
    {
        StartCoroutine(ProcessSequence());
    }

    private IEnumerator ProcessSequence()
    {
        // Step 1: Move object1 first
        yield return StartCoroutine(
            MoveToPosition(object1, new Vector3(object1XEnd, object1StartPos.y, object1StartPos.z))
        );

        // Step 2: Then move object2
        yield return StartCoroutine(
            MoveToPosition(object2, new Vector3(object2StartPos.x, object2StartPos.y, object2ZEnd))
        );

        // Step 3: Wait while object2 is in place
        yield return new WaitForSeconds(5f);

        // Step 4: Move object2 back
        yield return StartCoroutine(MoveToPosition(object2, object2StartPos));

        // Step 5: Then move object1 back
        yield return StartCoroutine(MoveToPosition(object1, object1StartPos));

        // Step 6: Done
        OnProcessFinished();
    }

    private IEnumerator MoveToPosition(Transform obj, Vector3 target)
    {
        if (obj == null) yield break;

        Vector3 startPos = obj.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            obj.localPosition = Vector3.Lerp(startPos, target, t);
            yield return null;
        }

        obj.localPosition = target;
    }

    private void OnProcessFinished()
    {
        LaserMachineDone?.Invoke();
    }
}
