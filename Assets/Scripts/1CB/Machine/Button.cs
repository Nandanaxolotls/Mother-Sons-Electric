using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MachineTriggerZone : MonoBehaviour
{
    public MachineSequence machineSequence;
    public InputActionProperty selectAction;
    public bool isHovered = false;

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovered = true;
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        isHovered = false;
    }

    void Update()
    {
        Debug.Log("ispressed : " + selectAction.action.IsPressed());
        if (isHovered && selectAction.action.IsPressed())
        {
            machineSequence.StartSequence();
        }
    }

   
}
