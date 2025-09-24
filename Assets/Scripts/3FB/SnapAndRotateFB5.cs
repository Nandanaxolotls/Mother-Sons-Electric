using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SnapAndRotateFB5 : MonoBehaviour
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


    public GameObject Point4GreenButton;
    public GameObject Point4BlueButton;
    public GameObject Point5GreenButton;


    [Header("Input")]
    public InputActionProperty selectAction;
    private bool hasActivated = false;
    private bool isHoveredButton1 = false;

    private HashSet<GameObject> processedObjects = new HashSet<GameObject>();
    private HashSet<GameObject> completedObjects = new HashSet<GameObject>();
    private GameObject currentObject;
    private XRGrabInteractable currentGrab;
    private int blueButtonPressCount = 0;

    [Header("Highlighter")]
    public bool Istutorial = false;
    public GameObject SphereFive;
    public bool DoneTutorial = false;


    public event System.Action SnappedToJig5;
    public event System.Action FB5done;


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

    private void Update()
    {
        if (selectAction.action.WasPressedThisFrame())
        {
            if (isHoveredButton1 && !hasActivated)
            {
                GreenButtonPressed();
                hasActivated = true;

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (acceptedTags.Contains(other.tag) &&
            !processedObjects.Contains(other.gameObject) &&
            !completedObjects.Contains(other.gameObject))
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

        Point4GreenButton.SetActive(false);
        Point4BlueButton.SetActive(false);
        Point5GreenButton.SetActive(true);

        if (SphereFive != null && Istutorial && !DoneTutorial)
        {
            SphereFive.SetActive(false);
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
            SnappedToJig5?.Invoke();
        }
    }

    private void UpdateUIValues()
    {
        int[] sliderValues = { 0, 0, 0, 0, 0,50 };
        int[] upperTextValues = { 15, 25, 35, 45, 55,00 };
        string[] lowerTextValues = { "00.000", "00.0000", "00.000", "00.000", "00.041","78.0046" };

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

 
    private IEnumerator ShowStatusTextForDuration(float duration)
    {
        statusText.SetActive(true);
        yield return new WaitForSeconds(duration);
        statusText.SetActive(false);
        ResetUIAndReactivateObject();
      

    }


    private void ResetUIAndReactivateObject()
    {
        statusText.SetActive(false);

        if (currentGrab != null)
        {
            currentGrab.enabled = true;

            // Re-register the interactable
            IXRInteractable interactable = currentGrab as IXRInteractable;
            if (interactable != null && currentGrab.interactionManager != null)
            {
                currentGrab.interactionManager.UnregisterInteractable(interactable);
                currentGrab.interactionManager.RegisterInteractable(interactable);
            }

            // Re-enable physics
            Rigidbody rb = currentGrab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (currentObject != null)
        {
            completedObjects.Add(currentObject); // Optional tracking
         //   processedObjects.Remove(currentObject); // Allow it to be processed again if needed
            currentObject = null;
            currentGrab = null;
        }

        hasActivated = false;
        if (Istutorial && !DoneTutorial)
        {
            FB5done?.Invoke();
            DoneTutorial = true;
        }
    }

}
