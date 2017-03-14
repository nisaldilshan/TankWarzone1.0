using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerObject))]
public class PlayerScore : MonoBehaviour {

    int lastKills = 0;
    int lastDeaths = 0;

    PlayerObject player;
	
	void Start ()
    {
        player = GetComponent<PlayerObject>();
        if (UserAccountManager.IsLoggedIn)
            StartCoroutine(SyncScoreLoop());
	}


    void OnDestroy()
    {
        if (player != null)
        {
            SyncNow();
        }
        
    }


    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            SyncNow();
        }
       
    }

    void SyncNow()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.GetData(OnDataReceived);
        }
    }

    void OnDataReceived(string data)
    {
        if (player.Kills <= lastKills && player.Deaths <= lastDeaths)
            return;

        int killsSinceLast = player.Kills - lastKills;
        int deathsSinceLast = player.Deaths - lastDeaths;

        //

        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);

        int newKills = killsSinceLast + kills;
        int newDeaths = deathsSinceLast + deaths;

        string newData = DataTranslator.ValuesToData( newKills, newDeaths );

        Debug.Log("Syncing" + newData);

        lastKills = player.Kills;
        lastDeaths = player.Deaths;

        UserAccountManager.instance.SendData(newData);
    }


}
