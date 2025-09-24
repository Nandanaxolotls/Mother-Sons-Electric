using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerRBTutorial : MonoBehaviour
{
    public StepWiseHighlighter RBpartTut;
    public SnapAndRotateRB SnapAndrotateRB;
    public SnapAndRotateRB2 SnapAndrotateRB2;
    public MagnifyingTutorial Magnifyingtutorial;
    public TutorialDestroyed tutorialDestroyed;

    public GameObject Spheretwo;
    public GameObject MagnifyingSphere;
    public GameObject RandomActivator;
    public AllObjectDestroy allObjectDestroy;

    [Space]
    public TMP_Text subTitletxt;
    public string levelName;
    public GameObject NextButton;

    public GameObject Arrow;
    void Start()
    {
        levelName = GameManager.Instance.levelDatas[3].LevelName;
        if (RBpartTut != null)
        {
            RBpartTut.Highlight();
        }
        //jig1
        SnapAndrotateRB.StartTutorial();
        SnapAndrotateRB.SnappedToJig1 += Snappedto1;
        SnapAndrotateRB.PressBlueButton += Pressblue1;
        //jig2
        SnapAndrotateRB.RB1done += StartRB2;
        SnapAndrotateRB2.SnappedToJig2 += Snappedto2;
        SnapAndrotateRB2.PressBlueButton += Pressblue2;

        SnapAndrotateRB2.RB2done += StartMagnifying;
        Magnifyingtutorial.defectChecked += MagnifyingCheckDone;
        tutorialDestroyed.TutorialObjectChecked += Tutorialcompleted;
        allObjectDestroy.AllObjectChecked += LevelCompleted;
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 0, subTitletxt); // welcome to the game 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(3, 1, subTitletxt, 3f)); // pick the RB part from box and place it on the highlighted part on jig 1

        }
    }

    public void Snappedto1()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 2, subTitletxt); // Now press green button using left hand to record the data
        }
    }

    public void Pressblue1()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 3, subTitletxt); // After data recorded then press blue button twice to change the display of AMM.
        }
    }

    public void StartRB2()
    {
        Spheretwo.SetActive(true);
        SnapAndrotateRB2.StartTutorial();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 4, subTitletxt); // Pick the part from jig 1 using right hand and place it on jig 2
        }
    }

    public void Snappedto2()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 5, subTitletxt); //Now press green button using left hand to record the data
            StartCoroutine(SoundManager.instance.PlayDelayedSound(3, 6, subTitletxt, 4.5f)); // then part will now rotate clockwise 90 degree to check concave Y direction 
        }
    }

    public void Pressblue2()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 7, subTitletxt); // After data recorded then press blue button to change the display of AMM.
        }
    }

    public void StartMagnifying()
    {
        MagnifyingSphere.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 8, subTitletxt); //Pick the object from Jig 2 and check for defect 
        }
    }

    public void MagnifyingCheckDone()
    {
        Debug.Log(" Magnifying check done");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 9, subTitletxt); // place the defected part in defected bin and good part in good bin
        }
        if(Arrow !=  null)
        {
            Arrow.SetActive(true);
        }
    }

    public void Tutorialcompleted()
    {
        RandomActivator.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 10, subTitletxt); // Do this same process with all other parts
        }
        if(Arrow != null)
        {
            Arrow.SetActive(false);
        }
    }
    public void LevelCompleted()
    {
        Debug.Log(" Level Completed ");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(3, 11, subTitletxt); // Congratulations Level Completed , now go near screen and press next button
        }
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
