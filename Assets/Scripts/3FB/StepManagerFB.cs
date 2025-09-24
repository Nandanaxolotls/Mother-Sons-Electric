using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerFB : MonoBehaviour
{
    public AllObjectDestroy allObjectDestroy;
    public GameObject NextButton;

    void Start()
    {
        allObjectDestroy.AllObjectChecked += LevelCompleted;
        if(NextButton != null )
        {
            NextButton.SetActive( false );
        }
    }

    public void LevelCompleted()
    {
        Debug.Log(" Level Completed ");
        if (NextButton != null)
        {
            NextButton.SetActive(true);
        }
    }

    public void levelChange(string name)
    {
        SceneManager.LoadScene(name);
    }
}
