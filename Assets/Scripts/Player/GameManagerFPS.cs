using UnityEngine;
using System.Collections.Generic;

public class GameManagerFPS : MonoBehaviour {

    public static GameManagerFPS instance;

    public MatchSettingsFPS matchsettings;

    [SerializeField]
    private GameObject sceneCamera;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one GameManager in the scene.");
        }
        else
        {
            instance = this;
        }
    }

    public void SetSceneCameraActive( bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(isActive);
    }

    #region Player Tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnregisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(400, 10, 200, 500));
        GUILayout.BeginVertical();

        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    #endregion
}
