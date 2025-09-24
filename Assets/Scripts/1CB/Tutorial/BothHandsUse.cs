using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BothHandsPickup : XRGrabInteractable
{
    [Header("Interaction Settings")]
    public GameObject Snappoint;
    public GameObject Arrow;
    public bool SphereActivated = false;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        
        if (Snappoint != null && !SphereActivated)
        {
            Snappoint.SetActive(true);
            SphereActivated = true;
        }
        if (Arrow != null)
        { 
            Arrow.SetActive(false);
        }
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

       
    }
}
