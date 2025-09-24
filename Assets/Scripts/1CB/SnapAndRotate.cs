using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TaggedObjectSnapAndRotate : MonoBehaviour
{
    [Header("Accepted Tags")]
    public List<string> acceptedTags;

    [Header("Snap Target")]
    public Transform snapPoint;

    [Header("Rotation Settings")]
    public float rotationSpeed = 360f;       // Degrees per second
    public float rotationDuration = 2f;      // Total rotation time

    private HashSet<GameObject> processedObjects = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (acceptedTags.Contains(other.tag) && !processedObjects.Contains(other.gameObject))
        {
            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                processedObjects.Add(other.gameObject);
                StartCoroutine(SnapAndRotate(other.gameObject, grab));
            }
        }
    }

    private IEnumerator SnapAndRotate(GameObject obj, XRGrabInteractable grab)
    {
        // Disable grab
        grab.enabled = false;

        // Disable physics
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Snap to target position and rotation
        obj.transform.position = snapPoint.position;
        obj.transform.rotation = snapPoint.rotation;

        // Rotate around Z axis for set duration
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float step = rotationSpeed * Time.deltaTime;
            obj.transform.Rotate(0f, 0f, step);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fix final position/rotation after rotation
        obj.transform.position = snapPoint.position;
        obj.transform.rotation = Quaternion.Euler(
            snapPoint.rotation.eulerAngles.x,
            snapPoint.rotation.eulerAngles.y,
            obj.transform.rotation.eulerAngles.z
        );

        // Notify the object
        ISnapProcessReceiver receiver = obj.GetComponent<ISnapProcessReceiver>();
        if (receiver != null)
        {
            receiver.OnSnapProcessComplete();
        }

        // Re-enable grab
        grab.enabled = true;
    }
}
