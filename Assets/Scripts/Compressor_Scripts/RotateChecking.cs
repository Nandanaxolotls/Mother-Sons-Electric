using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RotateChecking : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointUI
    {
        public Transform checkpoint;
        public Canvas canvas;
        public TextMeshProUGUI countdownText;
        public GameObject tickImage;
        public Image progressRing;
    }

    [Header("Checkpoint Setup")]
    public List<CheckpointUI> checkpointUIs;
    public float facingAngleThreshold = 30f;
    public float requiredLookTime = 2f;
    public TextMeshProUGUI completionText;
    //[Header("UI and Activation")]
    //public GameObject blowerObject;
    //public GameObject messageUI;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private bool[] checkpointStatus;
    private float[] checkpointTimers;

    private int currentIndex = -1;
    private Camera vrCamera;
    private bool isBeingHeld = false;
    private bool allCheckedAnnounced = false;

    [Header("Rotation Object")]
    public Transform objectToRotate;
    public float rotationSpeed = 180f; // degrees per second

    public GameObject SphereOFF;

    private bool isRotating = false;


    public event System.Action PivotCheckDone;
    void Start()
    {
        StartPivotCheck();
      
        vrCamera = Camera.main;
        int count = checkpointUIs.Count;

        checkpointStatus = new bool[count];
        checkpointTimers = new float[count];

        foreach (var ui in checkpointUIs)
        {
            if (ui.canvas != null) ui.canvas.enabled = false;
            if (ui.tickImage != null) ui.tickImage.SetActive(false);
            if (ui.countdownText != null) ui.countdownText.text = "";
            if (ui.progressRing != null) ui.progressRing.gameObject.SetActive(false);
        }

        //if (messageUI != null) messageUI.SetActive(false);
    }

    void Update()
    {
        if (!isBeingHeld) return;

        int lookedAtIndex = GetCheckpointLookingAt();

        if (lookedAtIndex != -1 && !checkpointStatus[lookedAtIndex])
        {
            Debug.Log("Loooking at pivot");
            if (completionText != null)
            {
                StartCoroutine(ShowTemporaryMessage("Looking at pivot", 3f));
            }
            if (SphereOFF != null)
            {
                SphereOFF.SetActive(false);
            }
            StartCoroutine(RotateObjectTwice(objectToRotate.transform));
            if (currentIndex == lookedAtIndex)
            {
                checkpointTimers[lookedAtIndex] += Time.deltaTime;

                float timeLeft = requiredLookTime - checkpointTimers[lookedAtIndex];
                int secondsLeft = Mathf.CeilToInt(timeLeft);

                var ui = checkpointUIs[lookedAtIndex];

                if (ui.canvas != null) ui.canvas.enabled = true;

                if (ui.countdownText != null)
                    ui.countdownText.text = secondsLeft.ToString();

                if (ui.progressRing != null)
                    ui.progressRing.fillAmount = Mathf.Clamp01(timeLeft / requiredLookTime);

                if (checkpointTimers[lookedAtIndex] >= requiredLookTime)
                {
                    checkpointStatus[lookedAtIndex] = true;
                    if (showDebugLogs) Debug.Log("Checkpoint " + (lookedAtIndex + 1) + " checked!");

                    if (ui.countdownText != null) ui.countdownText.text = "";
                    if (ui.tickImage != null) ui.tickImage.SetActive(true);

                    currentIndex = -1;
                    StartCoroutine(HideCanvasAfter(ui.canvas, 1.5f));
                }
            }
            else
            {
                ResetCurrentUI();
                currentIndex = lookedAtIndex;
                checkpointTimers[lookedAtIndex] = 0f;

                var ui = checkpointUIs[lookedAtIndex];
                if (ui.tickImage != null) ui.tickImage.SetActive(false);
                if (ui.canvas != null) ui.canvas.enabled = true;
                if (ui.progressRing != null) ui.progressRing.gameObject.SetActive(true);
            }
        }
        else
        {
            ResetCurrentUI();
            currentIndex = -1;
        }

        if (!allCheckedAnnounced && AllCheckpointsChecked())
        {
            allCheckedAnnounced = true;

            Invoke(nameof(ShowCompletion), 1f);
        }
    }
    private IEnumerator RotateObjectTwice(Transform target)
    {
        float speed = 1f; // degrees per second
        float duration = 2f; // rotate for 2 seconds
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.Rotate(0f, 0f, speed * Time.deltaTime); // Rotate on Z axis
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ShowTemporaryMessage(string message, float duration)
    {
        if (completionText != null)
        {
            completionText.text = message;
            yield return new WaitForSeconds(duration);
            completionText.text = "";
        }
    }
    public void ResetEMarkCheck()
    {
        allCheckedAnnounced = false;
        currentIndex = -1;

        for (int i = 0; i < checkpointStatus.Length; i++)
        {
            checkpointStatus[i] = false;
            checkpointTimers[i] = 0f;

            var ui = checkpointUIs[i];
            if (ui.canvas != null) ui.canvas.enabled = false;
            if (ui.countdownText != null) ui.countdownText.text = "";
            if (ui.progressRing != null) ui.progressRing.fillAmount = 1f;
            if (ui.progressRing != null) ui.progressRing.gameObject.SetActive(false);
            if (ui.tickImage != null) ui.tickImage.SetActive(false);
        }

        //if (messageUI != null) messageUI.SetActive(false);
    }

    public void DisableEMarkCheck()
    {
        isBeingHeld = false;
    }

    int GetCheckpointLookingAt()
    {
        for (int i = 0; i < checkpointUIs.Count; i++)
        {
            if (checkpointStatus[i]) continue;

            Transform checkpoint = checkpointUIs[i].checkpoint;
            Vector3 toCamera = (vrCamera.transform.position - checkpoint.position).normalized;
            Vector3 checkpointForward = checkpoint.forward;
            float angle = Vector3.Angle(checkpointForward, toCamera);

            if (angle < facingAngleThreshold)
            {
                return i;
            }
        }
        return -1;
    }

    void ResetCurrentUI()
    {
        if (currentIndex == -1) return;

        var ui = checkpointUIs[currentIndex];
        if (ui.canvas != null) ui.canvas.enabled = false;
        if (ui.countdownText != null) ui.countdownText.text = "";
        if (ui.progressRing != null) ui.progressRing.fillAmount = 1f;
        if (ui.tickImage != null) ui.tickImage.SetActive(false);
    }

    IEnumerator HideCanvasAfter(Canvas canvas, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (canvas != null) canvas.enabled = false;
    }

    bool AllCheckpointsChecked()
    {
        foreach (bool c in checkpointStatus)
        {
            if (!c) return false;
        }
        return true;
    }

    void ShowCompletion()
    {
        if (showDebugLogs) Debug.Log("All E-Marking checkpoints complete.");


        PivotCheckDone?.Invoke();
        //if (blowerObject != null) blowerObject.SetActive(true);
        //if (messageUI != null) messageUI.SetActive(true);
    }

    public void StartPivotCheck()
    {
        isBeingHeld = true;
    }
}
