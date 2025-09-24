using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMAnager : MonoBehaviour
{
    public GameObject LevelSelectionPanel;
    public GameObject LevelSelectionPanel2;

    public void toggleTutorialState(bool _state)
    {
        GameManager.Instance.isTutorial = _state;
    }

    public void LevelSelection()
    {
        LevelSelectionPanel2.SetActive(false);
        LevelSelectionPanel.SetActive(true);
    }

    public void BackTo1And2()
    {
        LevelSelectionPanel2.SetActive(true);
        LevelSelectionPanel.SetActive(false);
    }

    public void ToggleScene(string name)
    {
        SceneManager.LoadScene(name);

    }


}
