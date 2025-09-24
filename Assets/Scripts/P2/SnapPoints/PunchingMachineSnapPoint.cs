using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PunchingMachineSnapPoint : MonoBehaviour
{
    [Tooltip("Tag of the object that should snap here")]
    public string targetTag = "Pickable";

    [Tooltip("If true, snapped object will match rotation too")]
    public bool snapRotation = true;

    [Tooltip("Offset from snap zone position (optional)")]
    public Vector3 positionOffset;

    [Tooltip("Offset from snap zone rotation (optional)")]
    public Vector3 rotationOffset;

    public GameObject objectToActivateAfterSnap;

    [Header("Snap Options")]
    [Tooltip("If enabled, the snapped object cannot be picked up again after snapping.")]
    public bool makeSnappedObjectUngrabable = true;

    private XRSocketInteractor socketInteractor;
    private XRGrabInteractable candidateInteractable;

    public event System.Action CoversnappedToPunching;
    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            candidateInteractable = interactable;
            SnapObject(); // Snap immediately
        }
    }

    private void OnTriggerExit(Collider other)
    {
        XRGrabInteractable exited = other.GetComponent<XRGrabInteractable>();
        if (exited != null && exited == candidateInteractable)
        {
            candidateInteractable = null;
        }
    }

    private void SnapObject()
    {
        if (candidateInteractable == null) return;

        Vector3 snapPosition = transform.position + positionOffset;
        Quaternion snapRotationQuat = snapRotation
            ? transform.rotation * Quaternion.Euler(rotationOffset)
            : candidateInteractable.transform.rotation;

        Rigidbody rb = candidateInteractable.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Destroy(candidateInteractable.gameObject);

        if (objectToActivateAfterSnap != null)
        {
            // objectToActivateAfterSnap.transform.position = snapPosition;
            // objectToActivateAfterSnap.transform.rotation = snapRotationQuat;
            objectToActivateAfterSnap.SetActive(true);
            CoversnappedToPunching?.Invoke(); // Notify StepManager
                                   // ? Find and start the sequential snap process

        }

        if (socketInteractor != null)
            socketInteractor.enabled = false;

        Debug.Log("Snapped object destroyed, replacement activated.");

        gameObject.SetActive(false);
    }




    private void OnObjectGrabbedAgain(SelectEnterEventArgs args)
    {
        if (socketInteractor != null && !socketInteractor.enabled)
        {
            socketInteractor.enabled = true;
            Debug.Log("Socket re-enabled after object was grabbed again.");
        }

        if (candidateInteractable != null)
        {
            candidateInteractable.selectEntered.RemoveListener(OnObjectGrabbedAgain);
        }
    }
}

