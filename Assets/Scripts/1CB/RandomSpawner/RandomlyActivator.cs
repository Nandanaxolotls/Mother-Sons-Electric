using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActivationManager : MonoBehaviour
{
    [System.Serializable]
    public class PositionGroup
    {
        public List<GameObject> objects;
    }

    [Header("Group Setup")]
    public List<PositionGroup> positions = new List<PositionGroup>();


    [Header("Timing")]
    public float delayBetweenBatches = 2f;  // Delay before activating next batch


    private List<GameObject> currentActiveObjects = new List<GameObject>();
    private HashSet<GameObject> pickedObjects = new HashSet<GameObject>();

    void Start()
    {
        // Disable all objects at start
        foreach (var group in positions)
        {
            foreach (var obj in group.objects)
            {
                obj.SetActive(false);
            }
        }

       
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

        foreach (var group in positions)
        {
            List<GameObject> unpicked = group.objects.FindAll(obj => !pickedObjects.Contains(obj));

            if (unpicked.Count > 0)
            {
                GameObject randomObj = unpicked[Random.Range(0, unpicked.Count)];
                randomObj.SetActive(true);
                currentActiveObjects.Add(randomObj);

                Debug.Log("Activated: " + randomObj.name);
            }
        }

        if (currentActiveObjects.Count == 0)
        {
            Debug.Log(" All objects picked. Task complete.");
        }
    }
}
