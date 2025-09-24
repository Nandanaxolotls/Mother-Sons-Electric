using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class VideoPlaybackManagerRTline : MonoBehaviour
{
    [Header("Video Components")]
    public VideoPlayer videoPlayer;
    public GameObject videoScreen;


    [Header("Countdown UI")]
    public TextMeshProUGUI countdownText;

    [Header("Slide-In Settings")]
    public Transform videoScreenTransform;    // Assign the VideoScreen's transform here
    public float slideInDuration = 1f;
    public Vector3 slideOffset = new Vector3(0, -1.0f, 0); // How far below it starts

    [Header("After Video Actions")]
    public XRGrabInteractable interactor;
    public Rigidbody targetRigidbody;

    public event System.Action RTVideoPlayed;

    public void Start()
    {
        if (interactor != null)
        {
            interactor.enabled = false;
        }
        if (videoScreen != null)
            videoScreen.SetActive(false);
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
        if (targetRigidbody != null)
            targetRigidbody.isKinematic = true; // Keep it kinematic at star

    }

    public void StartVideoWithCountdown()
    {
        StartCoroutine(CountdownAndPlayVideo());
    }

    private IEnumerator CountdownAndPlayVideo()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "3";
            yield return new WaitForSeconds(1f);
            countdownText.text = "2";
            yield return new WaitForSeconds(1f);
            countdownText.text = "1";
            yield return new WaitForSeconds(1f);
            countdownText.gameObject.SetActive(false);
        }

        // Activate and Slide in
        if (videoScreen != null)
        {
            videoScreen.SetActive(true);
            if (videoScreenTransform != null)
                yield return StartCoroutine(SlideInFromBelow());
        }

        yield return new WaitForSeconds(2f);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(HandleVideoEnd());
    }

    private IEnumerator HandleVideoEnd()
    {
        yield return new WaitForSeconds(2f);

        if (videoScreen != null)
            videoScreen.SetActive(false);

        if (interactor != null)
        {
            interactor.enabled = true;
        }
        if (targetRigidbody != null)
            targetRigidbody.isKinematic = false; // Allow physics after video
        RTVideoPlayed?.Invoke();

    }
    private IEnumerator SlideInFromBelow()
    {
        Vector3 endPosition = videoScreenTransform.position;
        Vector3 startPosition = endPosition + slideOffset;
        float elapsed = 0f;

        videoScreenTransform.position = startPosition;

        while (elapsed < slideInDuration)
        {
            float t = elapsed / slideInDuration;
            videoScreenTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        videoScreenTransform.position = endPosition;
    }

}
