using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

    [SerializeField]
    GameObject ScoreboardItem_Prefab;

    [SerializeField]
    Transform ScoreboardItem_Holder;

    void OnEnable()
    {
        //Get anarray of players
        PlayerObject[] players = GameManagerCar.GetAllPlayers();

        //Loop through andsetup listitem foreach one
        foreach ( PlayerObject player in players)
        {
            Debug.Log(player.username + "|" + player.Kills + "|" + player.Deaths);
            GameObject itemGO = (GameObject)Instantiate(ScoreboardItem_Prefab, ScoreboardItem_Holder);

            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();

            if (item != null)
            {
                item.Setup(player.username, player.Kills, player.Deaths);
            }
        }
    }

    void OnDisable()
    {
        //clean up the list items
        foreach (Transform child in ScoreboardItem_Holder)
        {
            Destroy(child.gameObject);
        }
    }
}
