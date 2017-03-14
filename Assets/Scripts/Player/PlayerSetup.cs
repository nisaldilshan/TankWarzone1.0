using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ComponentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string dontDrawLayerName = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;


    void Start()
    {

        if (!isLocalPlayer)  // if not the local palyer
        {
            DisableComponents();
            AssignRemoteLayer();
        }

        else //if local player
        {
            
            //disable graphics for local palyer
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //create player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            //configure player UI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if( ui == null )
            {
                Debug.LogError("No PlayerUI component on PlayerUI Prefab");
            }
            ui.SetControllerType(1);
            ui.SetPlayer(GetComponent<Player>());

            //this part was moved here to fix the bug
            GetComponent<Player>().Setup();
        }
        
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManagerFPS.RegisterPlayer(_netID, _player);
    }
    
    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
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

        if(isLocalPlayer)
            GameManagerFPS.instance.SetSceneCameraActive(true);

        GameManagerFPS.UnregisterPlayer(transform.name);
    }

}
