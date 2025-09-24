using System.Collections;

using System.Collections.Generic;

using System.Data;

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Networking;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour

{

    [SerializeField] TMP_InputField inputEmployeeId;

    [SerializeField] TMP_InputField inputPassword;

    [SerializeField] TMP_Text txtMsg;

    [SerializeField] GameObject objLogin;
    [SerializeField] GameObject objLevelSelection;

    [SerializeField] Toggle toggletac;


    private void Start()
    {
        //   inputEmployeeId.readOnly = true;
        //  inputPassword.readOnly = true;

        //if (PlayerPrefs.GetInt(GameAPIs.login_Done)==1)
        //{
        //    objLogin.SetActive(false);
        //    objLevelSelection.SetActive(true);
        //}
    }

    private IEnumerator Authenticate(string mobile, string pass)

    {
        Debug.Log("Authenticate called");
        WWWForm form = new WWWForm();

        form.AddField("employee_id", mobile);

        form.AddField("password", pass);

        using (UnityWebRequest request = UnityWebRequest.Post(GameAPIs.loginAPi, form))

        {

            string auth = "Basic " + System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("phone:password"));

            request.SetRequestHeader("Authorization", auth);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)

            {
                Debug.Log("fail called");

                Debug.LogError("Login Error: " + request.error);

            }

            else

            {
                Debug.Log("Success called");

                string jsonData = request.downloadHandler.text;

                UserData dataUser = JsonUtility.FromJson<UserData>(jsonData);

                if (dataUser.status == "success")

                {

                    PlayerPrefs.SetInt(GameAPIs.login_Done, 1);
                    PlayerPrefs.SetString(GameAPIs.employee_id, inputEmployeeId.text);
                    GameAPIs.employee_id = dataUser.data.employee_id;
                    PlayerPrefs.SetString(GameAPIs.employee_id_key, inputEmployeeId.text);
                    PlayerPrefs.SetInt(GameAPIs.user_id, int.Parse(dataUser.data.id));

                    if (!string.IsNullOrEmpty(inputEmployeeId.text)) PlayerPrefs.SetString(GameAPIs.user_Phone, inputEmployeeId.text);

                    if (!string.IsNullOrEmpty(inputPassword.text)) PlayerPrefs.SetString(GameAPIs.user_Password, inputPassword.text);

                    //  GameAPIs.SetEmployeeId(inputEmployeeId.text);
                    Msg.instance.DisplayMsg("Login Successful");
                    StartCoroutine(DelayLevelSelection());
                }

                else

                {
                    Msg.instance.DisplayMsg(dataUser.message, Color.red);
                    //if (dataUser.status == "success") 

                    //else Msg.instance.DisplayMsg(dataUser.message, Color.red);

                }

            }

        }

    }

    IEnumerator DelayLevelSelection()
    {
        yield return new WaitForSeconds(2f);
        objLogin.SetActive(false);
        objLevelSelection.SetActive(true);
    }
    public void ButtonLogin()

    {

        //if (string.IsNullOrEmpty(inputEmployeeId.text))

        //{

        //    Msg.instance.DisplayMsg("Enter mobile number");

        //    return;

        //}

        //if (string.IsNullOrEmpty(inputPassword.text))

        //{

        //    Msg.instance.DisplayMsg("Enter password");

        //    return;

        //}

        //if (!toggleConfirm.isOn)

        //{

        //    Msg.instance.DisplayMsg("Please confirm your age");

        //    return;

        //}

        //if (!toggletac.isOn)

        //{

        //    Msg.instance.DisplayMsg("Please confirm Terms and Conditions"); 

        //    return;

        //}

        StartCoroutine(Authenticate(inputEmployeeId.text, inputPassword.text));

    }

    public void SceneChange(string scene) 
    {
        SceneManager.LoadScene(scene);
    }

    public void OnButtonPressed()

    {

        Debug.Log("Poke Button Pressed!");

    }


    [System.Serializable]

    public class UserData

    {

        public string status;

        public string message;

        public Data data;

    }

    [System.Serializable]

    public class Data

    {
        public string id;
        public string employee_id;
        public string name;
        public string phone;
        public string email;
        public string department;
        public string designation;
        public string password;
        public string status;
        public string created_at;
        public string last_login;

        public int is_bounded;

    }

}
