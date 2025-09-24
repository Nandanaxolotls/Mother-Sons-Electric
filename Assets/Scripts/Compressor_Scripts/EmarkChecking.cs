using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EMarkCheckManager : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointUI
    {
        public Transform checkpoint;              // The actual checkpoint position + direction
        public Canvas canvas;                     // World-space canvas
        public TextMeshProUGUI countdownText;     // Text showing countdown
        public GameObject tickImage;              // shown after completion
        public Image progressRing;
        public GameObject StartCanvas;
    }
    [Header("Checkpoint Setup")]
    public List<CheckpointUI> checkpointUIs;     // Assign front, back, final checkpoints in order
    public float facingAngleThreshold = 30f;
    public float requiredLookTime = 2f;

    [Header("UI and Activation")]
    public GameObject blowerObject;
    public GameObject messageUI;

    [Header("Debug")]
    public bool showDebugLogs = true;

    [Header("Additional GameObjects to Activate")]
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    private bool[] checkpointStatus;
    private float[] checkpointTimers;

    private int currentIndex = -1;
    private Camera vrCamera;
    private bool isBeingHeld = false;
    private bool allCheckedAnnounced = false;

    public event System.Action OnEMarkComplete;

    void Start()
    {
        vrCamera = Camera.main;
        int count = checkpointUIs.Count;

        checkpointStatus = new bool[count];
        checkpointTimers = new float[count];

        // Init all UIs hidden
        foreach (var ui in checkpointUIs)
        {
            if (ui.canvas != null) ui.canvas.enabled = false;
            if (ui.tickImage != null) ui.tickImage.SetActive(false);
            if (ui.countdownText != null) ui.countdownText.text = "";
            if (ui.progressRing != null) ui.progressRing.gameObject.SetActive(false);
            // if (ui.StartCanvas != null) ui.StartCanvas.SetActive(true);
        }

        // if (blowerObject != null) blowerObject.SetActive(false);
        if (messageUI != null) messageUI.SetActive(false);
    }

    void Update()
    {
        if (!isBeingHeld) return;

        int lookedAtIndex = GetCheckpointLookingAt();

        if (lookedAtIndex != -1 && !checkpointStatus[lookedAtIndex])
        {
            if (currentIndex == lookedAtIndex)
            {
                checkpointTimers[lookedAtIndex] += Time.deltaTime;

                float timeLeft = requiredLookTime - checkpointTimers[lookedAtIndex];
                int secondsLeft = Mathf.CeilToInt(timeLeft);

                var ui = checkpointUIs[lookedAtIndex];

                if (ui.canvas != null) ui.canvas.enabled = true;

                if (ui.countdownText != null)
                    ui.countdownText.text = secondsLeft.ToString();
                if (ui.StartCanvas != null) ui.StartCanvas.SetActive(false);

                if (ui.progressRing != null)
                    ui.progressRing.fillAmount = Mathf.Clamp01(timeLeft / requiredLookTime);

                if (checkpointTimers[lookedAtIndex] >= requiredLookTime)
                {
                    checkpointStatus[lookedAtIndex] = true;
                    if (showDebugLogs) Debug.Log(" Checkpoint {lookedAtIndex + 1} checked!");

                    if (ui.StartCanvas != null) ui.StartCanvas.SetActive(false);
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
            if (ui.StartCanvas != null) ui.StartCanvas.SetActive(true);
        }

        if (messageUI != null) messageUI.SetActive(false);
        // if (blowerObject != null) blowerObject.SetActive(false);
    }

    public void DisableEMarkCheck()
    {
        isBeingHeld = false;
        // ResetEMarkCheck();
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
        if (ui.StartCanvas != null) ui.StartCanvas.SetActive(true);
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
        if (showDebugLogs) Debug.Log(" All E-Marking checkpoints complete.");

        if (blowerObject != null) blowerObject.SetActive(true);
        if (messageUI != null) messageUI.SetActive(true);

        OnEMarkComplete?.Invoke(); // Notify StepManager
    }

    // Called by grab interactable
    public void EnableEMarkCheck()
    {
        isBeingHeld = true;

        for (int i = 0; i < checkpointUIs.Count; i++)
        {
            if (!checkpointStatus[i]) // Only for incomplete checkpoints
            {
                var ui = checkpointUIs[i];
                if (ui.StartCanvas != null)
                    ui.StartCanvas.SetActive(true);
            }
        }
    }


    // public void DisableEMarkCheck() => isBeingHeld = false;
}
