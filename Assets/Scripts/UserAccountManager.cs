using UnityEngine;
using System.Collections;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {

    public static UserAccountManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public static string LoggedIn_Username { get; protected set; } //stores username once logged in
    private static string LoggedIn_Password = "";//stores password once logged in

    public static string nickname = "";//stores password once logged in

    public static string LoggedIn_Data { get; protected set; }

    public static bool IsLoggedIn { get; protected set; }
    public static bool IsnicknameEnabled { get; protected set; }

    public string loggedInSceneName = "Lobby";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void OnDataRecievedCallback(string data);

    public void LogOut()
    {
        LoggedIn_Username = "";
        LoggedIn_Password = "";

        IsLoggedIn = false;

        Debug.Log("user logged out...!");

        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void LogIn(string username, string password)
    {
        LoggedIn_Username = username;
        LoggedIn_Password = password;

        IsLoggedIn = true;

        Debug.Log("Logged in as :" + username);

        SceneManager.LoadScene(loggedInSceneName);
    }

    public void LoginBypass()
    {
        //LoggedIn_Username = username;
        //LoggedIn_Password = password;

        IsLoggedIn = false;
        IsnicknameEnabled = false;

        Debug.Log("Login Bypassed");

        SceneManager.LoadScene(loggedInSceneName);
    }

    public void NickName_ON()
    {
        IsnicknameEnabled = true;
        Debug.Log("NickName ON");
    }
    public void NickName_OFF()
    {
        IsnicknameEnabled = false;
        Debug.Log("NickName OFF");
    }
    public void SetNickName(string _nickname)
    {
        nickname = _nickname;
        //Debug.Log("NickName:" + nickname);
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void SendData(string data)
    { 
        if (IsLoggedIn)
        {
            //ready to send request
            StartCoroutine(sendSendDataRequest(LoggedIn_Username, LoggedIn_Password,data)); //calls function to send: send data request
        }
    }

    IEnumerator sendSendDataRequest(string username, string password, string data)
    {
        IEnumerator eee = DCF.SetUserData(username, password, data);
        while (eee.MoveNext())
        {
            yield return eee.Current;
        }
        //WWW returneddd = eee.Current as WWW;
        string response = eee.Current as string; // << The returned string from the request
        if (response == "ContainsUnsupportedSymbol")
        {
            //One of the parameters contained a - symbol
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        if (response == "Error")
        {
            //Error occurred. For more information of the error, DC.Login could
            //be used with the same username and password
            Debug.Log("Data Upload Error: Contains Unsupported Symbol '-'");
        }
    }

    public void GetData(OnDataRecievedCallback onDataRecieved)
    {
        if (IsLoggedIn)
        {
            //ready to send request
            StartCoroutine(sendGetDataRequest(LoggedIn_Username, LoggedIn_Password, onDataRecieved)); //calls function to send get data request
        }
    }

    IEnumerator sendGetDataRequest(string username, string password, OnDataRecievedCallback onDataRecieved)
    {
        string data = "ERROR";

        IEnumerator eeee = DCF.GetUserData(username, password);
        while (eeee.MoveNext())
        {
            yield return eeee.Current;
        }

        //WWW returnedddd = eeee.Current as WWW;
        string response = eeee.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //Error occurred. For more information of the error, DC.Login could
            //be used with the same username and password
            Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
        else
        {
            if (response == "ContainsUnsupportedSymbol")
            {
                //One of the parameters contained a - symbol
                Debug.Log("Get Data Error: Contains Unsupported Symbol '-'");
            }
            else
            {
                //Data received in returned.text variable
                string DataRecieved = response;
                data = DataRecieved;
            }
        }

        //LoggedIn_Data = data;
        if(onDataRecieved != null)
            onDataRecieved.Invoke(data);
    }
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    //IEnumerator GetData()
    //{
    //    IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
    //    while (e.MoveNext())
    //    {
    //        yield return e.Current;
    //    }
    //    string response = e.Current as string; // << The returned string from the request

    //    if (response == "Error")
    //    {
    //        //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            
    //        playerUsername = "";
    //        playerPassword = "";
    //        loginParent.gameObject.SetActive(true);
    //        loadingParent.gameObject.SetActive(false);
    //        Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
    //    }
    //    else
    //    {
    //        //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
    //        loadingParent.gameObject.SetActive(false);
    //        loggedInParent.gameObject.SetActive(true);
    //        LoggedIn_DataOutputField.text = response;
    //    }
    //}
    //IEnumerator SetData(string data)
    //{
    //    IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
    //    while (e.MoveNext())
    //    {
    //        yield return e.Current;
    //    }
    //    string response = e.Current as string; // << The returned string from the request

    //    if (response == "Success")
    //    {
    //        //The data string was set correctly. Goes back to LoggedIn UI
    //        loadingParent.gameObject.SetActive(false);
    //        loggedInParent.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            
    //        playerUsername = "";
    //        playerPassword = "";
    //        loginParent.gameObject.SetActive(true);
    //        loadingParent.gameObject.SetActive(false);
    //        Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
    //    }
    //}
    //public void LoggedIn_SaveDataButtonPressed()
    //{
    //    StartCoroutine(SetData(LoggedIn_DataInputField.text));
    //}
    //public void LoggedIn_LoadDataButtonPressed()
    //{
    //    StartCoroutine(GetData());
    //}
}
