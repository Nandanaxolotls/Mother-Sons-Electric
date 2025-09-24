using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SyringeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI countdownText;   // Assign a UI Text in Inspector

    [Header("Required Tag")]
    public string syringeTag = "Syringe";

    [Header("Countdown Settings")]
    public float countdownTime = 5f;


    public GameObject DisableSphere;
    private Coroutine countdownCoroutine;

    public event System.Action SyringeInjected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(syringeTag))
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(StartCountdown());
            }
            DisableSphere.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(syringeTag))
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                if (countdownText != null)
                    countdownText.text = ""; // Clear text
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        float timeRemaining = countdownTime;

        while (timeRemaining > 0)
        {
            if (countdownText != null)
                countdownText.text = Mathf.Ceil(timeRemaining).ToString();

            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        // Clear countdown
        if (countdownText != null)
            countdownText.text = "";

        // Call the function after 3 sec
        OnSyringeHeld();
        countdownCoroutine = null;
    }

    private void OnSyringeHeld()
    {
        Debug.Log("Syringe stayed Function called!");
        SyringeInjected?.Invoke();
    }
}
