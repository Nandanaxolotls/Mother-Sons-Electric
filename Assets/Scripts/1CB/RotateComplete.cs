using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class SnapCompleteGrabChecker : MonoBehaviour, ISnapProcessReceiver
{
    private bool processCompleted = false;
    private XRGrabInteractable grab;
    private Rigidbody rb;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Subscribe to release event instead of grab
        grab.selectExited.AddListener(OnReleased);
    }

    public void OnSnapProcessComplete()
    {
        processCompleted = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (processCompleted && rb != null)
        {
            rb.isKinematic = false;
            Destroy(this); // Destroy this component after task is done
        }
    }

    private void OnDestroy()
    {
        grab.selectExited.RemoveListener(OnReleased);
    }
}
