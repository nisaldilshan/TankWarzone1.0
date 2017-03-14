using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    public Text killCount;
    public Text deathCount;

    void Start ()
    {
        if (UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.GetData(OnReceivedData);
        else
            NotLoggedIn();

    }
	
    void OnReceivedData(string data)
    {
        //Debug.Log(data);

        if (killCount == null || deathCount == null)
            return;

        killCount.text = DataTranslator.DataToKills(data).ToString() + " Kills";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " Deaths";
    }

    void NotLoggedIn()
    {
        if (killCount == null || deathCount == null)
            return;

        killCount.text = "You are not logged In";
        deathCount.text = "Check your Internet Connection";
    }


}
