using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerRTTutorial : MonoBehaviour
{
    public AllObjectDestroy AllObjectDestroy;
    public List<GameObject> RTParts;
    public List<StepWiseHighlighter> RTObjectsHighlight;
    public StepWiseHighlighter FinalMachineObject;
    public GameObject FinalSphere;

    public GameObject RTsnapPoint;
    public RTSnapPoint rTSnapPoint;

    public VideoPlaybackManagerRTline videoPlaybackManagerRTline;

    public GameObject RTfinalSnappoint;

    public RTfinalSnapPoint rTfinalSnapPoint;

    [Header("Highlighter")]
    public List<StepWiseHighlighter> RTparts;
    public MagnifyingTutorial Magnifyingtutorial;
    public TutorialDestroyed tutorialdestroyed;

    public List<StepWiseHighlighter> RTpartsOther;

    [Space]
    public TMP_Text subTitletxt;
    public string levelName;
    public GameObject NextButton;
    [Space]
    public GameObject Arrow;
    public GameObject Arrow2;
    public GameObject Arrow3;

    void Start()
    {
        levelName = GameManager.Instance.levelDatas[4].LevelName;
        if (RTparts != null && RTparts.Count > 0)
        {
            foreach (var part in RTparts)
            {
                if (part != null)
                    part.Highlight();
            }
        }
        Magnifyingtutorial.defectChecked += MagnifyingChecked;
        tutorialdestroyed.TutorialObjectChecked += CheckOtherParts;
        AllObjectDestroy.AllObjectChecked += Allchecked;
        RTsnapPoint.SetActive(false);
        foreach (var obj in RTParts)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        rTSnapPoint.RTsnapped += Playvideo;
        videoPlaybackManagerRTline.RTVideoPlayed += VideoPlayed;
        RTfinalSnappoint.SetActive(false);
        rTfinalSnapPoint.RTFinalsnapped += LevelCompleted;
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 0, subTitletxt); // welcome to the game 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(4, 1, subTitletxt, 3f)); // pick the RT part from box and check for defect

        }
    }

    public void MagnifyingChecked()
    {
        Debug.Log("Magnifying checked");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 2, subTitletxt); //place the defected part in defected bin and good part in good bin
        }
        if (Arrow != null)
        { 
            Arrow.SetActive(true);
        }
                                                                    
    }

    public void CheckOtherParts()
    {
        if (RTpartsOther != null && RTpartsOther.Count > 0)
        {
            foreach (var part in RTpartsOther)
            {
                if (part != null)
                    part.Highlight();
            }
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 3, subTitletxt); // Check defects for other parts also and sort them accordingly
        }
        if (Arrow != null)
        {
            Arrow.SetActive(false);
        }
    }

    public void Allchecked()
    {
        foreach (var obj in RTParts)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        if (RTObjectsHighlight != null && RTObjectsHighlight.Count > 0)
        {
            foreach (var part in RTObjectsHighlight)
            {
                if (part != null)
                    part.Highlight();
            }
        }

        RTsnapPoint.SetActive(true);
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 4, subTitletxt); // Go near Highlighted other tray and pick one RT from tray and place it on the highlighted jig
        }
        if(Arrow2 != null)
        {
            Arrow2.SetActive(true);
        }
    }

    public void Playvideo()
    {
        if (videoPlaybackManagerRTline != null)
        {
            videoPlaybackManagerRTline.StartVideoWithCountdown();
        }
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 5, subTitletxt); // wait for the machine process to complete
        }
        
    }

    public void VideoPlayed()
    {
        RTfinalSnappoint.SetActive(true);
        FinalMachineObject.Highlight();
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 6, subTitletxt); // Pick RT part from jig and place it on the highlighted tray
        }
        if (Arrow3 != null)
        {
            Arrow3.SetActive(true);
        }
    }

    public void LevelCompleted()
    {
        FinalSphere.SetActive(false);
        Debug.Log("RT line Level completed");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(4, 7, subTitletxt); // Congratulations Level Completed , now go near screen and press next button
        }
        if (NextButton != null)
        {
            NextButton.SetActive(true);
        }
        if (Arrow3 != null)
        {
            Arrow3.SetActive(false);
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
