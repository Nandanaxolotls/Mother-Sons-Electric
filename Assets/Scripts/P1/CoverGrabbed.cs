
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

[RequireComponent(typeof(XRGrabInteractable))]
public class CoverGrabbed : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("This event will fire when this object is grabbed.")]
    public UnityEvent onGrabbed;

    [Tooltip("This event will fire when this object is released.")]
    public UnityEvent onReleased;

    private XRGrabInteractable grabInteractable;

    private void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
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
        onGrabbed?.Invoke(); // Fire the inspector event
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        onReleased?.Invoke(); // Fire release event
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}
