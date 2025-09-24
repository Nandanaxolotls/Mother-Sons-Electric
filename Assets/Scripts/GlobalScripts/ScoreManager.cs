using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static SoundManager;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreTxt;
    public int scoreValue;


    private void Awake()
    {
        if (scoreTxt == null)
        {
            GameObject scoreObj = GameObject.FindGameObjectWithTag("ScoreTxt");
            if (scoreObj != null)
            {
                scoreTxt = scoreObj.GetComponent<TMP_Text>();
            }
        }

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }




    public void AddScore(int _scoreValue, string level, string process)
    {
        Debug.Log("Score added");
        if (scoreTxt == null)
        {
            GameObject scoreObj = GameObject.FindGameObjectWithTag("ScoreTxt");
            if (scoreObj != null)
            {
                scoreTxt = scoreObj.GetComponent<TMP_Text>();
            }
        }
        scoreValue += _scoreValue;
        scoreTxt.text = _scoreValue.ToString();
        SubmitScoreToDB(level, process, _scoreValue.ToString());


    }

    public void DeductScore(int _value, string level, string process, string score, string totalScore)
    {
        scoreValue -= _value;
        scoreTxt.text = _value.ToString();
        SubmitScoreToDB(level, process, score);
    }


    public void SubmitScoreToDB(string level, string process, string score)
    {
        StartCoroutine(SubmitScoreCoroutine(level, process, score));
    }

    private IEnumerator SubmitScoreCoroutine(string level, string process, string score)
    {
        Debug.Log("SubmitScore called");

        WWWForm form = new WWWForm();
        string employeeId = PlayerPrefs.GetString(GameAPIs.employee_id_key, "");
        form.AddField("employee_id", employeeId);
        //form.AddField("employee_id", GameAPIs.employee_id);
        form.AddField("level", level);
        form.AddField("process", process); // keep typo if server expects it
        form.AddField("score", score + " / " + 1);
        form.AddField("status", "Pass");

        using (UnityWebRequest request = UnityWebRequest.Post(GameAPIs.submitScoreAPi, form))
        {
            string auth = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("phone:password"));
            request.SetRequestHeader("Authorization", auth);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Score Submission Error: " + request.error);
                Msg.instance.DisplayMsg("Failed to submit score!", Color.red);
            }
            else
            {
                Debug.Log("Score submission success");
                string jsonData = request.downloadHandler.text;
                ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(jsonData);
                Debug.Log("Response");

                if (response.status == "success")
                {
                    Debug.Log("Score update success: " + response.data.score);
                    Msg.instance.DisplayMsg("Score Submitted Successfully!");
                }
                else
                {
                    Msg.instance.DisplayMsg(response.message, Color.red);
                }
            }
        }
    }

    public void SubmitTotalScoreToDB(string level, string score, string totalscore)
    {
        StartCoroutine(SubmitTotalScoreCoroutine(level, score, totalscore));

    }

    private IEnumerator SubmitTotalScoreCoroutine(string level, string score, string TotalScore)
    {
        Debug.Log("SubmitScore called");

        WWWForm form = new WWWForm();
        string employeeId = PlayerPrefs.GetString(GameAPIs.employee_id_key, "");
        form.AddField("employee_id", employeeId);
        form.AddField("level", level);
        form.AddField("get_score", score);
        form.AddField("total_score", TotalScore);
        // form.AddField("status", "Pass");

        using (UnityWebRequest request = UnityWebRequest.Post(GameAPIs.totalScoreAPi, form))
        {
            string auth = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("phone:password"));
            request.SetRequestHeader("Authorization", auth);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Score Submission Error: " + request.error);
                Msg.instance.DisplayMsg("Failed to submit score!", Color.red);
            }
            else
            {
                Debug.Log("Score submission success");
                string jsonData = request.downloadHandler.text;
                ScoreResponse response = JsonUtility.FromJson<ScoreResponse>(jsonData);
                Debug.Log("Response");

                if (response.status == "success")
                {
                    Debug.Log("Score update success: " + response.data.score);
                    Msg.instance.DisplayMsg("Score Submitted Successfully!");
                }
                else
                {
                    Msg.instance.DisplayMsg(response.message, Color.red);
                }
            }
        }
    }


    [Serializable]
    public class ScoreResponse
    {
        public string status;
        public string message;
        public ScoreData data;
    }

    [Serializable]
    public class ScoreData
    {
        public string employee_id;
        public string level;
        public string proccess;       // Note: Spelling is 'proccess' as per your JSON
        public string score;
        public string total_score;
        public string status;         // 'Pass' or 'Fail', etc.
        public int delete_status;
        public string last_updated;
    }


    [Serializable]
    public class LevelData
    {
        public float TotalScore;
        public string LevelName;
        public List<VoiceOverData> voiceOverDatas;
    }

}
