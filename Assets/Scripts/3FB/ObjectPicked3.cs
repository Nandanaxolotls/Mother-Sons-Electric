using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickableObject3 : MonoBehaviour
{
    public RandomActivationManager3 manager;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Grabbed: " + gameObject.name);
        manager?.NotifyPicked(this.gameObject);
    }
}
