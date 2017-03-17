using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManagerCar : MonoBehaviour {

    public static GameManagerCar instance;

    public MatchSettingsCAR matchsettings;

    //mini map values
    float current_x = 10f;
    float current_z = 10f;
    float rot_y = 90f;

    [SerializeField]
    private GameObject sceneCamera;

    [SerializeField]
    public GameObject MinimapCamera;

    [SerializeField]
    public GameObject DustParticles;

    [SerializeField]
    private GameObject loading_Screen;

    [SerializeField]
    private Transform arenaHolder;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the scene.");
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if (MinimapCamera == null || MinimapCamera.activeSelf == false)
            return;

        MinimapCamera.transform.position = new Vector3(current_x, 0f, current_z);
        MinimapCamera.transform.localEulerAngles = new Vector3(0f, rot_y, 0f);
    }


    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(isActive);
    }

    public void SetminimapActive(bool isActive)
    {
        if (MinimapCamera == null)
            return;

        MinimapCamera.SetActive(isActive);
    }

    public void getminimap_Values(float _x, float _z, float _roty)
    {
        current_x = _x;
        current_z = _z;
        rot_y = _roty;      
    }

    public void SetLoading_screen(bool value)
    {
        loading_Screen.SetActive(value);
    }

    public void SetArenaholder()
    {
        MapSelect.instance.Map(arenaHolder);
    }


    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, PlayerObject> players = new Dictionary<string, PlayerObject>();

    public static void RegisterPlayer (string _netID, PlayerObject _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnregisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static PlayerObject GetPlayer(string _playerID)
    {
        return players[_playerID];
    }


    public static PlayerObject[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }


    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(400, 10, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach (string _playerID in players.Keys)
    //    {
    //        //GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
    //        GUILayout.Label(players[_playerID].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    #endregion

}
