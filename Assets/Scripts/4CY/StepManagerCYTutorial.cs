using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerCYTutorial : MonoBehaviour
{
    public AllObjectDestroy allObjectDestroy;

    public List<StepWiseHighlighter> CYparts; // Now a list
    public MagnifyingTutorial magnifyingTutorial;

    public TutorialDestroyed TutorialDestroyed;
    public TMP_Text subTitletxt;
    public string levelName;
    public GameObject NextButton;

    [Space]
    public GameObject Arrow2;
    // Start is called before the first frame update
    void Start()
    {
        levelName = GameManager.Instance.levelDatas[1].LevelName;
        if (CYparts != null && CYparts.Count > 0)
        {
            foreach (var part in CYparts)
            {
                if (part != null)
                    part.Highlight();
            }
        }
        TutorialDestroyed.TutorialObjectChecked += StartForOther;
        allObjectDestroy.AllObjectChecked += LevelCompleted;
        magnifyingTutorial.defectChecked += MagnifyingChecked;
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(1, 0, subTitletxt); // welcome to the game 
            StartCoroutine(SoundManager.instance.PlayDelayedSound(1, 1, subTitletxt, 3f)); // pick the part from box and check for the defect

        }
    }

    public void MagnifyingChecked()
    {
        Debug.Log(" Magnifying Checked ");
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(1, 2, subTitletxt); // place the defected part in defected bin and good part in good bin

        }
        if (Arrow2 != null)
        { 
            Arrow2.SetActive(true);
        }


    }

    public void StartForOther()
    {
        if (GameManager.Instance.isTutorial)
        {
            SoundManager.instance.PlayVoiceOver(1, 3, subTitletxt); // do this same process with all other parts 

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
            SoundManager.instance.PlayVoiceOver(1, 4, subTitletxt); //  Congratulations Level Completed , now go near screen and press next button

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
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
