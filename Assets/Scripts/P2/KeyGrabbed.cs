using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

[RequireComponent(typeof(XRGrabInteractable))]
public class KeyGrabbed : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("This event will fire when this object is grabbed.")]
    public UnityEvent onGrabbed;

    [Tooltip("This event will fire when this object is released.")]
    public UnityEvent onReleased;

    [Header("References")]
    [Tooltip("Optional: Assign the mesh renderer that should be enabled on grab. If left empty, will auto-detect.")]
    public MeshRenderer targetMesh;

    private XRGrabInteractable grabInteractable;

    private void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        // Auto-detect mesh if not assigned
        if (targetMesh == null)
            targetMesh = GetComponent<MeshRenderer>();
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
    
        if (targetMesh != null)
            targetMesh.enabled = true;
        onGrabbed?.Invoke();
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
