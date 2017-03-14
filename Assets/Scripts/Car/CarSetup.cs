using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerObject))]
[RequireComponent(typeof(CarController))]
public class CarSetup : NetworkBehaviour
{

    public BoxCollider[] BodyColliders = new BoxCollider[2];

    public Canvas NamePlate;

    PlayerUI ui;

    [SerializeField]
    Behaviour[] ComponentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string DontDrawLayerName = "DontDraw";

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    //
    [SyncVar]
    private int currentmapnumber;
    //

    [SerializeField]
    Behaviour[] options_behavior;
    [SerializeField]
    GameObject[] options_objects;

    void Start()
    {
        if (MapSelect.instance.HOST())
        {
            CmdGetMapNumber();
        }

        if (!isLocalPlayer)  // if not the local palyer
        {
            DisableComponents();
            AssignRemoteLayer();
            AssignRemoteLayer_collider();
        }

        else //if local player
        {
            if (!MapSelect.instance.HOST())
            {
                CmdGetMapNumber();
            }

            //create player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            //configure player UI
            ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component on PlayerUI Prefab");
            }
            ui.SetControllerType(2);
            ui.SetCar(GetComponent<PlayerObject>());

            //this part was moved here to fix the bug
            GetComponent<PlayerObject>().Setup();

            //setting up user name
            string _username = "Loading ...";
            if (UserAccountManager.IsLoggedIn)
            {
                _username = UserAccountManager.LoggedIn_Username;
            }
            else if(UserAccountManager.IsnicknameEnabled)
            {
                _username = UserAccountManager.nickname;
            }
            else
            {
                _username = transform.name;
            }

            CmdSetUserName(transform.name, _username);

            AssignDontDrawLayer();
        }

    }

    void Update()
    {
        Set_OptionsMenu(ui);
    }

    [Command]
    void CmdSetUserName(string playerID, string username)
    {
        PlayerObject player = GameManagerCar.GetPlayer(playerID);
        if (player != null)
        {
            Debug.Log(username + " has joined.");
            player.username = username;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerObject _player = GetComponent<PlayerObject>();

        GameManagerCar.RegisterPlayer(_netID, _player);
        RegisterCollider();

    }

    void RegisterCollider()
    {
        string _colID0 = "Player " + GetComponent<NetworkIdentity>().netId + BodyColliders[0].name;
        BodyColliders[0].name = _colID0;
        string _colID1 = "Player " + GetComponent<NetworkIdentity>().netId + BodyColliders[1].name;
        BodyColliders[1].name = _colID1;
    }

    void Set_OptionsMenu(PlayerUI ui)
    {
        options_behavior[0].enabled = ui.ssao;
        options_behavior[1].enabled = ui.motionblur;
        options_behavior[2].enabled = ui.bloom;
        options_behavior[3].enabled = ui.sunshafts;
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void AssignDontDrawLayer()
    {
        NamePlate.gameObject.layer = LayerMask.NameToLayer(DontDrawLayerName);
    }

    void AssignRemoteLayer_collider()
    {
        BodyColliders[0].gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        BodyColliders[1].gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < ComponentsToDisable.Length; i++)
        {
            ComponentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        Destroy(playerUIInstance);

        if (isLocalPlayer)
            GameManagerCar.instance.SetSceneCameraActive(true);
        
        GameManagerCar.UnregisterPlayer(transform.name);
    }

    [Command]
    void CmdGetMapNumber()
    {
        currentmapnumber = MapSelect.instance.GetMapNumber();
        RpcSetMapNumber();
    }

    [ClientRpc]
    public void RpcSetMapNumber()
    {
        MapSelect.instance.SetMapNumber(currentmapnumber);
        GameManagerCar.instance.SetArenaholder();
    }
}
