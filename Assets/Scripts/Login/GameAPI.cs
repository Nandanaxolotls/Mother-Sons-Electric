

using UnityEngine;



public class GameAPIs : MonoBehaviour

{

    public static string baseURL = "https://motherson.axolotlsapps.in/api/";

    public static string loginAPi = baseURL + "auth/employee_login";
    public static string submitScoreAPi = baseURL + "score/save";
    public static string totalScoreAPi = baseURL + "score/total_save";


    public static readonly string user_Phone = "User_phone";

    public static readonly string user_Password = "User_password";

    public static readonly string user_id = "User_Id";

    public static string employee_id = "";

    public static string login_Done = "login_done";


    public static string employee_id_key = "User_Name"; // Key name for PlayerPrefs



    public static void SetEmployeeId(string user)

    {

        PlayerPrefs.SetString(employee_id, user);
        PlayerPrefs.Save();
    }


    // Start is called before the first frame update

    void Start()

    {

    }

    // Update is called once per frame

    void Update()

    {

    }

}

