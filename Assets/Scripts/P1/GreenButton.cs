using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GreenButton : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty selectAction;
    public bool isLocked = false;

    public event System.Action ButtonPressed;
   // public event System.Action onReachedDesired;

    private bool isHovered = false;

    void Start()
    {
        
    }

    public void OnHoverEntered(HoverEnterEventArgs args) => isHovered = true;
    public void OnHoverExited(HoverExitEventArgs args) => isHovered = false;

    void Update()
    {
        if (!isLocked && isHovered && selectAction.action.WasPressedThisFrame())
        {
            TogglePosition();
        }
    }

    private void TogglePosition()
    {
        ButtonPressed?.Invoke();
        isLocked = true;
    }

  
}
