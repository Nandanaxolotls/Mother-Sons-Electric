using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerFBTutorial : MonoBehaviour
{
    public SnapAndRotateFB snapAndRotateFB;
    public SnapAndRotateFB2 snapAndRotateFB2;
    public SnapAndRotateFB3 snapAndRotateFB3;
    public SnapAndRotateFB4 snapAndRotateFB4;
    public SnapAndRotateFB5 snapAndRotateFB5;
    public MagnifyingTutorial magnifyingtutorial;
    public AllObjectDestroy allObjectDestroy;

    [Header("Highlighter")]
    public StepWiseHighlighter FBpart;
    public GameObject FB2Sphere;
    public GameObject FB3Sphere;
    public GameObject FB4Sphere;
    public GameObject FB5Sphere;
    public GameObject MagnifyingSphere;
    public TutorialDestroyed tutorialDestroyed;
    public GameObject RandomActivator;
    [Space]
    public TMP_Text subTitletxt;
    public string levelName;
    public GameObject NextButton;

    public GameObject Arrow1;
    public GameObject Arrow2;
    void Start()
    {
        levelName = GameManager.Instance.levelDatas[2].LevelName;
        if (FBpart != null)
        {
            FBpart.Highlight();
        }
        //jig1
        snapAndRotateFB.StartTutorial();
        snapAndRotateFB.SnappedToJig1 += Snappedto1;
        snapAndRotateFB.PressBlueButton += PressBlue1;
        //jig2
        snapAndRotateFB.FB1done += StartFB2;
        snapAndRotateFB2.SnappedToJig2 += Snappedto2;
        snapAndRotateFB2.PressBlueButton += PressBlue2;
        //jig3
        snapAndRotateFB2.FB2done += StartFB3;
        snapAndRotateFB3.SnappedToJig3 += Snappedto3;
        snapAndRotateFB3.PressBlueButton += PressBlue3;
        //jig4
        snapAndRotateFB3.FB3done += StartFB4;
        snapAndRotateFB4.SnappedToJig4 += Snappedto4;
        snapAndRotateFB4.PressBlueButton += PressBlue4;
        //jig5
        snapAndRotateFB4.FB4done += StartFB5;
        snapAndRotateFB5.SnappedToJig5 += Snappedto5;

        snapAndRotateFB5.FB5done += Magnifyingcheck;
        magnifyingtutorial.defectChecked += MagnifyingcheckDone;
        tutorialDestroyed.TutorialObjectChecked += TutorialCompleted;
        allObjectDestroy.AllObjectChecked += LevelCompleted;
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 0, subTitletxt); // welcome to the game 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(2, 1, subTitletxt, 3f)); // pick the FB part from box and place it on the highlighted part on jig 1

        }
    }
    //jig1
    public void Snappedto1()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 2, subTitletxt); // Now press green button using left hand to record the data
        }
    }

    public void PressBlue1()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 3, subTitletxt); // After data recorded then press blue button twice to change the display of AMM.
        }
    }

    //jig2
    public void StartFB2()
    {
        FB2Sphere.SetActive(true);
        snapAndRotateFB2.StartTutorial();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 4, subTitletxt); // Pick the part from jig 1 using right hand and place it on jig 2
        }
    }

    public void Snappedto2()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 5, subTitletxt); //Now press green button using left hand to record the data
            StartCoroutine(SoundManager.instance.PlayDelayedSound(2, 6, subTitletxt, 4.5f)); // then part will now rotate clockwise 70 degree to check concave Y direction 
        }
    }

    public void PressBlue2()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 7, subTitletxt); // After data recorded then press blue button to change the display of AMM.
        }
    }


    //jig3
    public void StartFB3()
    {
        FB3Sphere.SetActive(true);
        snapAndRotateFB3.StartTutorial();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 8, subTitletxt); // Pick the part from jig 2 using right hand place it on jig 3
        }
    }

    public void Snappedto3()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 9, subTitletxt); //Now press green button using left hand to record the data 
        }
    }

    public void PressBlue3()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 10, subTitletxt); // After data recorded then press blue button to change the display of AMM.
        }
    }


    //jig4
    public void StartFB4()
    {
        FB4Sphere.SetActive(true);
        snapAndRotateFB4.StartTutorial();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 11, subTitletxt); // Pick the part from jig 3 using right hand and place it on jig 4
        }
    }
    public void Snappedto4()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 12, subTitletxt); //Now press green button using left hand to record the data 
        }
    }
    public void PressBlue4()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 13, subTitletxt); // After data recorded then press blue button to change the display of AMM.
        }
    }



    //jig5
    public void StartFB5()
    {
        FB5Sphere.SetActive(true);
        snapAndRotateFB5.StartTutorial();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 14, subTitletxt); // Pick the part from jig 4 using right hand and place it on jig 5
        }
    }
    public void Snappedto5()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 15, subTitletxt); //Now press green button using left hand to record the data 
        }
    }



    public void Magnifyingcheck()
    {
        MagnifyingSphere.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 16, subTitletxt); //Pick the object from Jig 5 and check for defect 
        }
    }

    public void MagnifyingcheckDone()
    {
        Debug.Log("Magnifying check done");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 17, subTitletxt); // place the defected part in defected bin and good part in good bin
        }
        if (Arrow1 != null)
        {
            Arrow1.SetActive(true);
        }
        if (Arrow2 != null)
        {
            Arrow2.SetActive(true);
        }
    }

    public void TutorialCompleted()
    {
        RandomActivator.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 18, subTitletxt); // Do this same process with all other parts
        }
        if (Arrow1 != null)
        {
            Arrow1.SetActive(false);
        }
        if (Arrow2 != null)
        {
            Arrow2.SetActive(false);
        }

    }

    public void LevelCompleted()
    {
        Debug.Log(" Level Completed ");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(2, 19, subTitletxt); // Congratulations Level Completed , now go near screen and press next button
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
