using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StepManagerCBline : MonoBehaviour
{

    public RandomActivationManager randomActivationManager;

    public AllObjectDestroy allObjectDestroy;

    public GameObject NextButton;

    void Start()
    {
        allObjectDestroy.AllObjectChecked += LevelCompleted;
        randomActivationManager.StartActivating();

    }

    public void LevelCompleted()
    {
        Debug.Log("Level completed");
        NextButton.SetActive(true);
    }

    public void RBlevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
