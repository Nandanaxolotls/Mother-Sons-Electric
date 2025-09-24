using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SnapAndRotateFB3 : MonoBehaviour
{
    [Header("Accepted Tags")]
    public List<string> acceptedTags;

    [Header("Snap Target")]
    public Transform snapPoint;

    [Header("Rotation Settings")]
    public float rotationSpeed = 360f;
    public float rotationDuration = 2f;

    [Header("UI Elements")]
    public Slider[] sliders;
    public TextMeshProUGUI[] upTexts;
    public TextMeshProUGUI[] downTexts;
    public GameObject statusText; // Assign Text for "Data Recorded"

    public GameObject Point1GreenButton;
    public GameObject Point1BlueButton;
    public GameObject Point2GreenButton;
    public GameObject Point2BlueButton;
    public GameObject Point3GreenButton;
    public GameObject Point3BlueButton;
    public GameObject Point4GreenButton;
    public GameObject Point4BlueButton;
    public GameObject Point5GreenButton;


    [Header("Input")]
    public InputActionProperty selectAction;
    private bool hasActivated = false;
    private bool isHoveredButton1 = false;
    private bool isHoveredButton2 = false;

    private HashSet<GameObject> processedObjects = new HashSet<GameObject>();
    private GameObject currentObject;
    private XRGrabInteractable currentGrab;
    private int blueButtonPressCount = 0;

    [Header("Highlighter")]
    public bool Istutorial = false;
    public GameObject SphereThree;
    public bool DoneTutorial = false;


    public event System.Action SnappedToJig3;
    public event System.Action PressBlueButton;
    public event System.Action FB3done;


    private void Start()
    {
        statusText.SetActive(false);
    }

    public void StartTutorial()
    {
        Istutorial = true;
    }

    public void OnHoverEnteredButton1(HoverEnterEventArgs args) => isHoveredButton1 = true;
    public void OnHoverExitedButton1(HoverExitEventArgs args) => isHoveredButton1 = false;
    public void OnHoverEnteredButton2(HoverEnterEventArgs args) => isHoveredButton2 = true;
    public void OnHoverExitedButton2(HoverExitEventArgs args) => isHoveredButton2 = false;
    private void Update()
    {
        if (selectAction.action.WasPressedThisFrame())
        {
            if (isHoveredButton1 && !hasActivated)
            {
                GreenButtonPressed();
                hasActivated = true;
               
            }
            else if (isHoveredButton2 && hasActivated)
            {
                BlueButtonPressed();
            }
        }
    }

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
        grab.enabled = false;

        Point1GreenButton.SetActive(false);
        Point1BlueButton.SetActive(false);
        Point2GreenButton.SetActive(false);
        Point2BlueButton.SetActive(false);
        Point3GreenButton.SetActive(true);
        Point3BlueButton.SetActive(true);
        Point4GreenButton.SetActive(false);
        Point4BlueButton.SetActive(false);
        //Point5GreenButton.SetActive(false);
        if (SphereThree != null && Istutorial && !DoneTutorial)
        {
            SphereThree.SetActive(false);
        }
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        obj.transform.position = snapPoint.position;
        obj.transform.rotation = snapPoint.rotation;

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float step = rotationSpeed * Time.deltaTime;
            obj.transform.Rotate(0f, step, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = snapPoint.position;
        obj.transform.rotation = Quaternion.Euler(
            snapPoint.rotation.eulerAngles.x,
            snapPoint.rotation.eulerAngles.y,
            obj.transform.rotation.eulerAngles.z
        );

        ISnapProcessReceiver receiver = obj.GetComponent<ISnapProcessReceiver>();
        if (receiver != null) receiver.OnSnapProcessComplete();

        UpdateUIValues();
        currentObject = obj;
        currentGrab = grab;
        if (Istutorial && !DoneTutorial)
        {
            SnappedToJig3?.Invoke();
        }
    }

    private void UpdateUIValues()
    {
        int[] sliderValues = { 0, 0, 0, 80, 0 };
        int[] upperTextValues = { 14, 24, 34, 44, 54 };
        string[] lowerTextValues = { "00.000", "00.0000", "00.000", "39.004", "00.000" };

        for (int i = 0; i < 6; i++)
        {
            if (i < sliders.Length) sliders[i].value = sliderValues[i];
            if (i < upTexts.Length) upTexts[i].text = upperTextValues[i].ToString();
            if (i < downTexts.Length) downTexts[i].text = lowerTextValues[i];
        }
    }

    private void GreenButtonPressed()
    {
        Debug.Log("Button pressed");
        StartCoroutine(ShowStatusTextForDuration(3f));
    }

    private void BlueButtonPressed()
    {
       

        Debug.Log($"Blue Button Pressed: Count = {blueButtonPressCount}");

            ResetUIAndReactivateObject();
            hasActivated = false;
        
        
    }

    private IEnumerator ShowStatusTextForDuration(float duration)
    {
        statusText.SetActive(true);
        yield return new WaitForSeconds(duration);
        statusText.SetActive(false);
        if (Istutorial && !DoneTutorial)
        {
            PressBlueButton?.Invoke();
        }
    }


    private void ResetUIAndReactivateObject()
    {
        int[] newUpperValues = { 15, 25, 35, 45, 55 };
        string[] resetLowerTexts = { "", "", "", "", "" };

        for (int i = 0; i < 6; i++)
        {
            if (i < sliders.Length) sliders[i].value = 0;
            if (i < upTexts.Length) upTexts[i].text = newUpperValues[i].ToString();
            if (i < downTexts.Length) downTexts[i].text = resetLowerTexts[i];
        }

        statusText.SetActive(false);

        if (currentGrab != null)
        {
            currentGrab.enabled = true;
        }

        if (currentObject != null)
        {
            processedObjects.Remove(currentObject);
            currentObject = null;
            currentGrab = null;
        }

        if (Istutorial && !DoneTutorial)
        {
            FB3done?.Invoke();
            DoneTutorial = true;
        }
    }
}
