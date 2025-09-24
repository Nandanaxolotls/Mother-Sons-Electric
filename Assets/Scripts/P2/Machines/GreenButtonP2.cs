using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GreenButtonP2 : MonoBehaviour
{
    public FrontCoverOnAssembly frontCoverOnAssembly;
    public GameObject Canvas;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Tooltip7;
    [Header("Input")]
    public InputActionProperty selectAction;
    private bool isHovered = false;
    private bool doorclosed = false;
    public event System.Action CameraChecked;

    void Start()
    {
        frontCoverOnAssembly.FrontOnMachineSnapped += FrontCoverSnapped;
    }

    public void OnHoverEntered(HoverEnterEventArgs args) => isHovered = true;
    public void OnHoverExited(HoverExitEventArgs args) => isHovered = false;

    void Update()
    {
        if (doorclosed && isHovered && selectAction.action.WasPressedThisFrame())
        {
            StartCoroutine(Displaying());
        }
    }
    public void FrontCoverSnapped()
    {
        Debug.Log("Done");
        doorclosed = true;
        Canvas.SetActive(true);

    }

    public IEnumerator Displaying()
    {
        Tooltip7.SetActive(false);
        Button1.SetActive(true);
        yield return new WaitForSeconds(3f);
        Button1.SetActive(true);
        Button2 .SetActive(true);
        CameraChecked?.Invoke();
    }
}
