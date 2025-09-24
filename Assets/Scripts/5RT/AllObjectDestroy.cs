using UnityEngine;

public class AllObjectDestroy : MonoBehaviour
{
    [Header("Assign objects to track")]
    public GameObject[] objectsToCheck;

    [Header("Check frequency (seconds)")]
    public float checkInterval = 0.5f;

    private bool allDestroyedCalled = false;

    public event System.Action AllObjectChecked;

    private void Start()
    {
        InvokeRepeating(nameof(CheckObjects), checkInterval, checkInterval);
    }

    private void CheckObjects()
    {
        // Skip if function already triggered
        if (allDestroyedCalled) return;

        bool allDestroyed = true;

        foreach (GameObject obj in objectsToCheck)
        {
            if (obj != null) // still exists
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed)
        {
            allDestroyedCalled = true;
            AllDestroyed();
        }
    }

    private void AllDestroyed()
    {
        Debug.Log(" All assigned objects have been destroyed!");
        AllObjectChecked?.Invoke();
    }
}
