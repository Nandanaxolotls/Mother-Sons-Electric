using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class StepManagerCompTut : MonoBehaviour
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

    [Header("Highlight")]
    public GameObject RotorSphere;
    public StepWiseHighlighter Marker;
    public StepWiseHighlighter SyringePick;
    public GameObject SyringePickSphere;
    public GameObject DropZoneArrow;

    [Space]
    public TMP_Text subTitletxt;
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        levelName = GameManager.Instance.levelDatas[5].LevelName;
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
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(5, 0, subTitletxt); // welcome to the game 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(5, 1, subTitletxt, 3f)); // Pick the Compressor which is on the table and Start checking for defects

        }
        //pick the compressor and check for defect in compressor
    }

    public void CheckingCompleted()
    {
        emarkchecked = true;
        rotateChecking.enabled = true;
        Checkboth();
        if (RotorSphere != null)
        {
            RotorSphere.SetActive(true);
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(5, 2, subTitletxt); // Check the Rotor by rotating it
        }
        // check the rotor for defect
    }

    public void RotorCheckCompleted()
    {
        rotorchecked = true;
        MarkPoints.SetActive(true);
        Checkboth();
        if(Marker != null)
        {
            Marker.Highlight();
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(5, 3, subTitletxt); // Pick up the marker and follow the highlighted guides to mark the correct positions
        }
        //start marking on the places 
    }

    public void AllMarkingDone()
    {
        Syringe.enabled = true;
        SyringeChecker.SetActive(true);
        if(SyringePick != null)
        {
            SyringePick.Highlight();
        }
        SyringePickSphere.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(5, 4, subTitletxt); // Now pick the Syringe then take it near the highlighted point and wait for injecting to complete
        }
        //inject syringe 
    }


    public void SyringeInjectionDone()
    {
        dropZone.enabled = true;
        DropZoneArrow.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(5, 5, subTitletxt); // Pick compressor and place it inside bin
        }

        // place it in the bin 
    }


    public void CompressorInBin()
    {
        compressordestroy = true;
        DropZoneArrow.SetActive(false);
        Checkboth();
    }

    public void Checkboth()
    {
        if (emarkchecked && rotorchecked && compressordestroy)
        {
            LevelCompleted();
            if (GameManager.Instance.isTutorial)
            {
                SoundManager.instance.PlayVoiceOver(5, 6, subTitletxt); //Congratulations Level Completed , now go near screen and press next button
            }        
        
        }

    }


    public void LevelCompleted()
    {
        if (NextButton != null)
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

