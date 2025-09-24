using UnityEngine;
using System.Collections.Generic;

public class CB30onMachine : MonoBehaviour
{
    [Header("Objects to Check")]
    public List<GameObject> objectsToCheck;

    [Header("Optional Settings")]
    public bool checkContinuously = true;

    private bool alreadyTriggered = false;

    public event System.Action CB30PlayVideo;

    void Update()
    {
        if (checkContinuously && !alreadyTriggered)
        {
            CheckAllActive();
        }
    }

    public void CheckAllActive()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj == null || !obj.activeInHierarchy)
            {
                return; // If any object is inactive, exit early
            }
        }

        // All are active
        alreadyTriggered = true;
        OnAllObjectsActive();
    }

    private void OnAllObjectsActive()
    {
        Debug.Log(" All assigned objects are active!");
        CB30PlayVideo?.Invoke();
        // TODO: Call your next logic here
        // Example: GetComponent<MyNextStep>().Begin();
    }

    // Optional: Call this manually if needed
    public void ResetChecker()
    {
        alreadyTriggered = false;
    }
}
