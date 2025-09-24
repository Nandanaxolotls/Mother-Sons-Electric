using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TagRestrictedSnapZone : XRSocketInteractor
{
    public List<string> acceptedTags;
    [Header("Object to Disable After Snap")]
    public GameObject objectToDisable; // Assign in Inspector
    public event System.Action Startmachine;
    public bool firstMachineStarted = false;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Disable the assigned object instead of the snapped one
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
        if (Startmachine != null && !firstMachineStarted)
        { 
            Startmachine?.Invoke();
            firstMachineStarted = true;
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return acceptedTags.Contains(interactable.transform.tag) && base.CanSelect(interactable);
    }
}
