using UnityEngine;

public class GrabActivatorZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GrabActivateObject grabActivate = other.GetComponent<GrabActivateObject>();
        if (grabActivate != null && !grabActivate.enabled)
        {
            grabActivate.enabled = true;
            Debug.Log("Activated GrabActivateObject on: " + other.name);
        }
    }
}
