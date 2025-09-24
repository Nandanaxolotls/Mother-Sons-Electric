//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
//using System.Collections;

//public class DropZone : MonoBehaviour
//{
//    public string acceptedTag;
//    public int rewardPoints = 1;
//    public int penaltyPoints = -1;
//    public float delayBeforeCheck = 0.2f;
//    public float feedbackDuration = 1.0f;

//    [Header("Visual Feedback")]
//    public Renderer zoneRenderer; // assign the Renderer of the drop box
//    public Color correctColor = Color.green;
//    public Color incorrectColor = Color.red;
//    private Color originalColor;

//    [Header("Floating +1 Text (VR)")]
//    public GameObject floatingTextPrefabPositive;           // Assign the prefab
//    public GameObject floatingTextPrefabNegative;
//    public Transform playerCamera;                  // Assign the VR Camera (e.g. XR Origin ? Camera Offset ? Main Camera)
//                                                    //  public Transform worldTarget;                   // Optional: where it floats toward (like a score panel or UI board)
//    public Transform spawnPointTransform;

//    private void Start()
//    {
//        if (zoneRenderer == null)
//            zoneRenderer = GetComponent<Renderer>();

//        if (zoneRenderer != null)
//            originalColor = zoneRenderer.material.color;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Defected") || other.CompareTag("Good"))
//        {
//            StartCoroutine(HandleDropObject(other.gameObject));
//        }
//    }

//    private IEnumerator HandleDropObject(GameObject obj)
//    {
//        yield return new WaitForSeconds(delayBeforeCheck);

//        if (obj == null)
//            yield break;

//        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
//        if (grabInteractable != null && grabInteractable.isSelected)
//            yield break; // Still held

//        string objectTag = obj.tag;

//        if (objectTag == acceptedTag)
//        {
//            ScoreManager.instance.AddPoints(rewardPoints);
//            Debug.Log("Correct drop! +" + rewardPoints);
//            StartCoroutine(FlashZoneColor(correctColor));
//            ShowFloatingPositive();
//        }
//        else
//        {
//            ScoreManager.instance.AddPoints(penaltyPoints);
//            Debug.Log("Incorrect drop! " + penaltyPoints);
//            StartCoroutine(FlashZoneColor(incorrectColor));
//            ShowFloatingNegative();
//        }

//        Destroy(obj);
//    }

//    private IEnumerator FlashZoneColor(Color color)
//    {
//        if (zoneRenderer != null)
//        {
//            zoneRenderer.material.color = color;
//            yield return new WaitForSeconds(feedbackDuration);
//            zoneRenderer.material.color = originalColor;
//        }
//    }

//    public void ShowFloatingPositive()
//    {
//        StartCoroutine(FloatingTextAnimationPositive());
//    }

//    private IEnumerator FloatingTextAnimationPositive()
//    {
//        Vector3 spawnPosition = spawnPointTransform.position;

//        GameObject floatingText = Instantiate(floatingTextPrefabPositive, spawnPosition, Quaternion.identity);

//        // Make it face the player
//        floatingText.transform.LookAt(playerCamera);
//        floatingText.transform.Rotate(0, 180, 0);

//        Vector3 start = spawnPosition;
//        Vector3 end = start + Vector3.up * 0.5f;

//        float duration = 2f;
//        float elapsed = 0f;

//        while (elapsed < duration)
//        {
//            elapsed += Time.deltaTime;
//            float t = elapsed / duration;
//            floatingText.transform.position = Vector3.Lerp(start, end, t);
//            yield return null;
//        }

//        Destroy(floatingText);
//    }


//    public void ShowFloatingNegative()
//    {
//        StartCoroutine(FloatingTextAnimationNegative());
//    }

//    private IEnumerator FloatingTextAnimationNegative()
//    {
//        Vector3 spawnPosition = spawnPointTransform.position;

//        GameObject floatingText = Instantiate(floatingTextPrefabNegative, spawnPosition, Quaternion.identity);

//        // Make it face the player
//        floatingText.transform.LookAt(playerCamera);
//        floatingText.transform.Rotate(0, 180, 0);

//        Vector3 start = spawnPosition;
//        Vector3 end = start + Vector3.up * 0.5f;

//        float duration = 2f;
//        float elapsed = 0f;

//        while (elapsed < duration)
//        {
//            elapsed += Time.deltaTime;
//            float t = elapsed / duration;
//            floatingText.transform.position = Vector3.Lerp(start, end, t);
//            yield return null;
//        }

//        Destroy(floatingText);
//    }

//}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

public class DropZone : MonoBehaviour
{
    [Header("Accepted Tags")]
    public List<string> acceptedTags;  // ? List instead of single string

    public int rewardPoints = 1;
    public int penaltyPoints = -1;
    public float delayBeforeCheck = 0.2f;
    public float feedbackDuration = 1.0f;

    [Header("Visual Feedback")]
    public Renderer zoneRenderer;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    private Color originalColor;

    [Header("Floating +1 Text (VR)")]
    public GameObject floatingTextPrefabPositive;
    public GameObject floatingTextPrefabNegative;
    public Transform playerCamera;
    public Transform spawnPointTransform;

    private void Start()
    {
        if (zoneRenderer == null)
            zoneRenderer = GetComponent<Renderer>();

        if (zoneRenderer != null)
            originalColor = zoneRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (acceptedTags.Contains(other.tag)) // only start coroutine if tag is one of the expected ones
        {
            StartCoroutine(HandleDropObject(other.gameObject));
        }
        else if (other.CompareTag("Defected") || other.CompareTag("NotGood") || other.CompareTag("Good")) // fallback in case
        {
            StartCoroutine(HandleDropObject(other.gameObject));
        }
    }

    private IEnumerator HandleDropObject(GameObject obj)
    {
        yield return new WaitForSeconds(delayBeforeCheck);

        if (obj == null)
            yield break;

        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
            yield break;

        string objectTag = obj.tag;

        if (acceptedTags.Contains(objectTag))
        {
            ScoreManagerGame.instance.AddPoints(rewardPoints);
            Debug.Log("Correct drop! +" + rewardPoints);
            StartCoroutine(FlashZoneColor(correctColor));
            ShowFloatingPositive();
        }
        else
        {
            ScoreManagerGame.instance.AddPoints(penaltyPoints);
            Debug.Log("Incorrect drop! " + penaltyPoints);
            StartCoroutine(FlashZoneColor(incorrectColor));
            ShowFloatingNegative();
        }

        Destroy(obj);
    }

    private IEnumerator FlashZoneColor(Color color)
    {
        if (zoneRenderer != null)
        {
            zoneRenderer.material.color = color;
            yield return new WaitForSeconds(feedbackDuration);
            zoneRenderer.material.color = originalColor;
        }
    }

    public void ShowFloatingPositive()
    {
        StartCoroutine(FloatingTextAnimation(floatingTextPrefabPositive));
    }

    public void ShowFloatingNegative()
    {
        StartCoroutine(FloatingTextAnimation(floatingTextPrefabNegative));
    }

    private IEnumerator FloatingTextAnimation(GameObject prefab)
    {
        Vector3 spawnPosition = spawnPointTransform.position;
        GameObject floatingText = Instantiate(prefab, spawnPosition, Quaternion.identity);

        floatingText.transform.LookAt(playerCamera);
        floatingText.transform.Rotate(0, 180, 0);

        Vector3 start = spawnPosition;
        Vector3 end = start + Vector3.up * 0.5f;

        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            floatingText.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        Destroy(floatingText);
    }
}
