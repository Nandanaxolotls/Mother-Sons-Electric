using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

public class StepWiseHighlighter : MonoBehaviour
{
    public Material highlightMaterial;
    public float blinkInterval = 0.5f; // Time between highlight on/off

    private List<Renderer> renderers = new List<Renderer>();
    private List<Material[]> originalMaterials = new List<Material[]>(); // store arrays
    private XRGrabInteractable grabInteractable;
    private Coroutine blinkCoroutine;

    void Awake()
    {
        // Get all renderers in this object and its children
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        foreach (Renderer r in renderers)
        {
            // Save full material array for each renderer
            originalMaterials.Add(r.materials);
        }

        // If grab component is attached, register to unhighlight on grab
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    public void Highlight()
    {
        if (highlightMaterial == null) return;

        // Start blinking
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkHighlight());
    }

    public void Unhighlight()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        // Restore original materials
        for (int i = 0; i < renderers.Count; i++)
        {
            if (renderers[i] != null && originalMaterials[i] != null)
            {
                renderers[i].materials = originalMaterials[i]; // restore full array
            }
        }
    }

    private IEnumerator BlinkHighlight()
    {
        bool showingHighlight = true;

        while (true)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                if (renderers[i] != null)
                {
                    if (showingHighlight)
                    {
                        // Replace all slots with highlight
                        Material[] highlightArray = new Material[renderers[i].materials.Length];
                        for (int j = 0; j < highlightArray.Length; j++)
                            highlightArray[j] = highlightMaterial;

                        renderers[i].materials = highlightArray;
                    }
                    else
                    {
                        // Restore original
                        renderers[i].materials = originalMaterials[i];
                    }
                }
            }

            showingHighlight = !showingHighlight;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Unhighlight();
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }
}
