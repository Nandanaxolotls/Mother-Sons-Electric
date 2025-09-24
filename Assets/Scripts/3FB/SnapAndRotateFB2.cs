using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SnapAndRotateFB2 : MonoBehaviour
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
    private HashSet<GameObject> completedObjects = new HashSet<GameObject>();

    private GameObject currentObject;
    private XRGrabInteractable currentGrab;
    private int blueButtonPressCount = 0;

    [Header("Highlighter")]
    public bool Istutorial = false;
    public GameObject SphereTwo;
    public bool DoneTutorial = false;


    public event System.Action SnappedToJig2;
    public event System.Action PressBlueButton;
    public event System.Action FB2done;


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
                blueButtonPressCount = 0; // Reset count when green is pressed
            }
            else if (isHoveredButton2 && hasActivated)
            {
                BlueButtonPressed();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (acceptedTags.Contains(other.tag) && !processedObjects.Contains(other.gameObject) && !completedObjects.Contains(other.gameObject))
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
        Point2GreenButton.SetActive(true);
        Point2BlueButton.SetActive(true);
        Point3GreenButton.SetActive(false);
        Point3BlueButton.SetActive(false);
        Point4GreenButton.SetActive(false);
        Point4BlueButton.SetActive(false);
        // Point5GreenButton.SetActive(false);
        if (SphereTwo != null && Istutorial && !DoneTutorial)
        {
            SphereTwo.SetActive(false);
        }
       
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // rb.isKinematic = true;
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
            SnappedToJig2?.Invoke();
        }
    }

    private void UpdateUIValues()
    {
        int[] sliderValues = { 25, 65, 70, 50, 30 };
        int[] upperTextValues = { 13, 23, 33, 43, 53};
        string[] lowerTextValues = { "00.003", "00.091", "00.069", "00.040", "00.003"};

        for (int i = 0; i < 6; i++)
        {
            if (i < sliders.Length) sliders[i].value = sliderValues[i];
            if (i < upTexts.Length) upTexts[i].text = upperTextValues[i].ToString();
            if (i < downTexts.Length) downTexts[i].text = lowerTextValues[i];
        }
    }
    private void GreenButtonPressed()
    {
        Debug.Log("Green Button pressed");
        StartCoroutine(GreenButtonSequence());
    }

    private void BlueButtonPressed()
    {
        ResetUIAndReactivateObject();
        hasActivated = false;
    }


    private IEnumerator GreenButtonSequence()
    {
        // Show "Data Recorded" for 3 seconds
        statusText.SetActive(true);
        yield return new WaitForSeconds(3f);
        statusText.SetActive(false);

        // Rotate the snapped object 70 degrees on Y axis
        if (currentObject != null)
        {
            Quaternion startRotation = currentObject.transform.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0, 70f, 0);
            float rotateTime = 1f;
            float elapsed = 0f;

            while (elapsed < rotateTime)
            {
                currentObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / rotateTime);
                elapsed += Time.deltaTime;
                yield return null;
            }

          
            currentObject.transform.rotation = endRotation;
        }

        // Show "Data Recorded" again
        statusText.SetActive(true);
        yield return new WaitForSeconds(3f);
        statusText.SetActive(false);
        if (Istutorial && !DoneTutorial)
        {
            PressBlueButton?.Invoke();
        }

        hasActivated = true;
    }

    private void ResetUIAndReactivateObject()
    {
        int[] sliderValues = { 00, 00, 00, 00, 00, 00 };
        int[] newUpperValues = { 00, 00, 00, 00, 00, 00 };
        string[] resetLowerTexts = { "", "", "", "", "", "" };

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

            IXRInteractable interactable = currentGrab as IXRInteractable;
            if (interactable != null && currentGrab.interactionManager != null)
            {
                currentGrab.interactionManager.UnregisterInteractable(interactable);
                currentGrab.interactionManager.RegisterInteractable(interactable);
            }

            // Re-enable physics
            Rigidbody rb = currentGrab.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;
        }


        if (currentObject != null)
        {
            completedObjects.Add(currentObject); // ? Add this
            processedObjects.Remove(currentObject);
            currentObject = null;
            currentGrab = null;
        }

        if (Istutorial && !DoneTutorial)
        {
            FB2done?.Invoke();
            DoneTutorial = true;
        }
    }

}
