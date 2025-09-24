using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ComputerDisplay : MonoBehaviour
{
    public Door1 door1;
    public GameObject Canvas;
    public GameObject Tooltip;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public TMP_Text countdownText;  // Assign your UI text (TextMeshProUGUI)
    public GameObject ToolTip2;

    [Header("Settings")]
    public int countdownTime = 5;   // Adjustable countdown duration


    [Header("Input")]
    public InputActionProperty selectAction;
    private bool isHovered = false;
    private bool doorclosed = false;


    void Start()
    {
        door1.Door1ReachedDesired += DoorclosingDone;
       // Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
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
    public void DoorclosingDone()
    {
        Debug.Log("Done");
        doorclosed = true;
        Canvas.SetActive(true);
      
    }
 
    public IEnumerator Displaying()
    {
        if (Tooltip != null)
        {
            Tooltip.SetActive(false);
        }
        Button1.SetActive(false);
        Button2.SetActive(true);

        // Countdown loop
        int timeLeft = countdownTime;
        while (timeLeft >= 0)
        {
            if (countdownText != null)
                countdownText.text = timeLeft.ToString();

            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        // Optionally clear text
        if (countdownText != null)
            countdownText.text = "";
        // After countdown finishes
        Button1.SetActive(true);
        Button2.SetActive(false);
        Button3.SetActive(true);
        door1.Unlock();
        if (ToolTip2 != null)
        {
            ToolTip2.SetActive(true);
        }
       
    }
}
