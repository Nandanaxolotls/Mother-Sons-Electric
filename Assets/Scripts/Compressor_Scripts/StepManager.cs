using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class StepManager : MonoBehaviour
{
    //emark check
    public EMarkCheckManager eMarkCheckManager;
    //rotate check
    public RotateChecking rotateChecking;
    //Marking Points Activate
    public GameObject MarkPoints;
    //After marking done
    public MarkActiveChecker markActiveChecker;
    //Activate Syringe
    public XRGrabInteractable Syringe;
    public GameObject SyringeChecker;
    //After Syringe checking done
    public SyringeManager syringeManager;
    //Drop zone activating 
    public DropZone dropZone;
    //After compressor dropped in bin checking
    public AllObjectDestroy allObjectDestroy;
    public bool emarkchecked = false;
    public bool rotorchecked = false;
    public bool compressordestroy = false;

    public GameObject NextButton;
    // Start is called before the first frame update
    void Start()
    {
        rotateChecking.enabled = false;
        eMarkCheckManager.OnEMarkComplete += CheckingCompleted;
        rotateChecking.PivotCheckDone += RotorCheckCompleted;
        MarkPoints.SetActive(false);
        markActiveChecker.AllMarkingDone += AllMarkingDone;
        Syringe.enabled = false;
        SyringeChecker.SetActive(false);
        syringeManager.SyringeInjected += SyringeInjectionDone;
        allObjectDestroy.AllObjectChecked += CompressorInBin;
        dropZone.enabled = false;
    }

    public void CheckingCompleted()
    {
        emarkchecked = true;
        rotateChecking.enabled = true;
        Checkboth();
    }

    public void RotorCheckCompleted()
    {
        rotorchecked = true;
        MarkPoints.SetActive(true);
        Checkboth();
    }

    public void AllMarkingDone()
    {
        Syringe.enabled = true;
        SyringeChecker.SetActive (true);
    }


    public void SyringeInjectionDone()
    {
        dropZone.enabled = true;
    }


    public void CompressorInBin()
    {
        compressordestroy = true;
        Checkboth();
    }

    public void Checkboth()
    {
        if (emarkchecked && rotorchecked && compressordestroy)
        {
            LevelCompleted();
        }

    }


    public void LevelCompleted()
    {
        if(NextButton != null)
        {
            NextButton.SetActive(true);
        }
    }

    public void Nextlevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void RestartGame()
    {
        string CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CurrentScene);
    }
}
