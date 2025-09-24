using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActivationManager3 : MonoBehaviour
{
    [Header("Objects to Activate Randomly")]
    public List<GameObject> objects = new List<GameObject>(); // ? All objects in one group

    [Header("Timing")]
    public float delayBetweenBatches = 2f;

    private List<GameObject> currentActiveObjects = new List<GameObject>();
    private HashSet<GameObject> pickedObjects = new HashSet<GameObject>();

    void Start()
    {
        // Disable all objects at the start
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
        StartCoroutine(ActivateNextSetWithDelay());
    }

    public void StartActivating()
    {
        StartCoroutine(ActivateNextSetWithDelay());
    }

    public void NotifyPicked(GameObject picked)
    {
        if (currentActiveObjects.Contains(picked))
        {
            Debug.Log("Picked: " + picked.name);
            currentActiveObjects.Remove(picked);
            pickedObjects.Add(picked);

            if (currentActiveObjects.Count == 0)
            {
                Debug.Log("All active picked. Activating next set...");
                StartCoroutine(ActivateNextSetWithDelay());
            }
        }
    }

    public IEnumerator ActivateNextSetWithDelay()
    {
        yield return new WaitForSeconds(delayBetweenBatches);

        currentActiveObjects.Clear();

        List<GameObject> unpicked = objects.FindAll(obj => !pickedObjects.Contains(obj));

        if (unpicked.Count > 0)
        {
            GameObject randomObj = unpicked[Random.Range(0, unpicked.Count)];
            randomObj.SetActive(true);
            currentActiveObjects.Add(randomObj);

            Debug.Log("Activated: " + randomObj.name);
        }
        else
        {
            Debug.Log("All objects picked. Task complete.");
        }
    }
}
