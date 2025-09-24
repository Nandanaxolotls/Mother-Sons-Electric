using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabActivateObject : MonoBehaviour
{
    public GameObject targetObjectToToggle; // Object to activate/deactivate
    private XRGrabInteractable grabInteractable;

    private void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
        targetObjectToToggle.SetActive(true);
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }


    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (targetObjectToToggle != null)
            targetObjectToToggle.SetActive(true);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (targetObjectToToggle != null)
            targetObjectToToggle.SetActive(false);
    }

    private void OnDestroy()
    {
        // Unregister events to avoid memory leaks
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}
