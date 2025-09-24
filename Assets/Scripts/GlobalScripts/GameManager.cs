using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ScoreManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<LevelData> levelDatas;
    public ScoreManager scoreManager;

    public bool isEnglish;

    public bool isTutorial;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteKey("login_Done");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LoadSceneBtn(int _Scene)
    {
        SceneManager.LoadScene(_Scene);
    }

    public void ToggleLanguage(bool state)
    {

    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteKey("login_Done");
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            ClearPlayerPrefs();
        }
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            ClearPlayerPrefs();
        }
    }
    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("login_Done");
        PlayerPrefs.DeleteKey("user_id");
        PlayerPrefs.DeleteKey("user_Phone");
        PlayerPrefs.DeleteKey("user_Password");
        PlayerPrefs.Save();
    }


}
