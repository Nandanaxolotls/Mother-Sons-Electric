using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

[RequireComponent(typeof(XRGrabInteractable))]
public class HighlightOnHover : MonoBehaviour
{
    [Header("Highlight Settings")]
    public Material highlightMaterial;
    public float maxDistance = 3.0f;  // max distance to allow highlight

    private List<Renderer> renderers = new List<Renderer>();
    private List<Material[]> originalMaterials = new List<Material[]>();

    private bool isGrabbed = false;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in allRenderers)
        {
            Material[] matsCopy = new Material[rend.materials.Length];
            for (int i = 0; i < rend.materials.Length; i++)
            {
                matsCopy[i] = rend.materials[i];
            }

            renderers.Add(rend);
            originalMaterials.Add(matsCopy);
        }

        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    //private void OnDestroy()
    //{
    //    grabInteractable.selectEntered.RemoveListener(OnGrab);
    //    grabInteractable.selectExited.RemoveListener(OnRelease);
    //}

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        RestoreOriginalMaterials();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    public void OnHoverEnter(XRBaseInteractor interactor)
    {
        if (isGrabbed) return;

        if (IsInteractorInRange(interactor))
        {
            SetHighlightMaterials();
        }
    }

    public void OnHoverExit(XRBaseInteractor interactor)
    {
        if (isGrabbed) return;

        if (IsInteractorInRange(interactor))
        {
            RestoreOriginalMaterials();
        }
    }

    private bool IsInteractorInRange(XRBaseInteractor interactor)
    {
        float distance = Vector3.Distance(interactor.transform.position, transform.position);
        return distance <= maxDistance;
    }

    private void SetHighlightMaterials()
    {
        foreach (Renderer rend in renderers)
        {
            Material[] highlightMats = new Material[rend.materials.Length];
            for (int i = 0; i < highlightMats.Length; i++)
            {
                highlightMats[i] = highlightMaterial;
            }
            rend.materials = highlightMats;
        }
    }

    private void RestoreOriginalMaterials()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }
}
