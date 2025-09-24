using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingTutorial : MonoBehaviour
{
    public GameObject Sphere;
    public event System.Action defectChecked;
    public bool defectcheckeddone = false;
    private void OnTriggerEnter(Collider other)
    {
        GrabActivateObject grabActivate = other.GetComponent<GrabActivateObject>();
        if (grabActivate != null && !grabActivate.enabled)
        {
            grabActivate.enabled = true;
            if (Sphere != null)
            {
                Sphere.SetActive(false);
            }
            if (defectChecked != null && !defectcheckeddone)
            {
                defectChecked.Invoke();
                defectcheckeddone = true;
            }
            Debug.Log("Activated GrabActivateObject on: " + other.name);
        }
    }
}
