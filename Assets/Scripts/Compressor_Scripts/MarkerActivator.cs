using UnityEngine;
using System.Collections.Generic;

public class MarkerActivator : MonoBehaviour
{
    [Header("Objects to Activate")]
    public List<GameObject> objectsToActivate = new List<GameObject>();

    [Header("Required Marker Tag")]
    public string markerTag = "Marker";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(markerTag))
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                  
                    Debug.Log($"Activated: {obj.name}");
                }
            }
            ScoreManagerGame.instance.AddPoints(1);
            // Destroy this trigger object after activation
            Destroy(gameObject);
        }

    }
}
