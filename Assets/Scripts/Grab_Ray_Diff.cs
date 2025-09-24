using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grab_And_RAY_Difference : MonoBehaviour
{
    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public XRDirectInteractor LeftDirectinteractor;
    public XRDirectInteractor RightDirectinteractor;

    // Update is called once per frame
    void Update()
    {
        leftGrabRay.SetActive(LeftDirectinteractor.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(RightDirectinteractor.interactablesSelected.Count == 0);
    }
}
