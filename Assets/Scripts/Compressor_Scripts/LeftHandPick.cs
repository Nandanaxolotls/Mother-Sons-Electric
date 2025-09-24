using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandOnlyPickup : XRGrabInteractable
{
    [Header("Interaction Settings")]
    [Tooltip("Only interactors with this tag can pick up this object.")]
    public string allowedInteractorTag = "Left Hand";
    public EMarkCheckManager EMarkCheckManager;
    public GameObject Snappoint;
    public GameObject Arrow1;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag(allowedInteractorTag))
        {
            base.OnSelectEntering(args);

            if (EMarkCheckManager != null)
            {
                EMarkCheckManager.EnableEMarkCheck();
            }
            if (Snappoint != null)
            {
                Snappoint.SetActive(true);
            }
            if(Arrow1 != null)
            {
                Arrow1.SetActive(false); 
            }
        }
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        if (EMarkCheckManager != null)
        {
            EMarkCheckManager.DisableEMarkCheck(); // Stop tracking
        }
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        return interactor.transform.CompareTag(allowedInteractorTag) && base.IsSelectableBy(interactor);
    }
}
