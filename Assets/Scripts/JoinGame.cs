using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Collections;


public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    private NetworkManager networkManager;

    void Start ()
    {
        networkManager = NetworkManager.singleton;

        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }


    public void RefreshRoomList()
    {
        ClearRoomList();

        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        Debug.Log("try to list matches");
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);  
        status.text = "Loading ... ";
    }


    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        //status.text = "";

        if (!success || matchList == null)
        {
            status.text = "Couldn't get room list";
            return;
        }

        if (success)
        //Debug.Log(" success");
        if (matchList != null)
        //Debug.Log("matchlist success");

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);
            //
            _roomListItemGO.transform.localScale =new Vector3(1, 1, 1);
            _roomListItemGO.transform.localRotation = Quaternion.identity;
            _roomListItemGO.transform.localPosition = new Vector3(154.5f, -12.5f, 0);
            //
            RoomListItem _roomListItem = _roomListItemGO.GetComponent< RoomListItem>();
            if(_roomListItem != null)
            {
                Debug.Log("room list item is created");
                _roomListItem.Setup(match, JoinRoom);
            }
            //set up the name of the room/ no of players in it
            //as well as a call back function taht will join the game
            roomList.Add(_roomListItemGO);
            Debug.Log("room list item is displayed");
        }

        if (roomList.Count == 0)
        {
            status.text = "No rooms at the moment";
        }
    }

	
    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }


    public void JoinRoom(MatchInfoSnapshot _match)
    {
        Debug.Log(" Joining " + _match.name);
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());

    }

    IEnumerator WaitForJoin()
    {
        ClearRoomList();

        int countdown = 10;
        while (countdown > 0)
        {
            status.text = "JOINING... (" + countdown + ")";

            yield return new WaitForSeconds(0.8f);

            countdown--;
        }

        // Failed to connect
        status.text = "Failed to connect.";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null)
        {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }

        RefreshRoomList();

    }

}
