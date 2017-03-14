using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu instance;

    void Awake()
    {

        instance = this;
    }

    public static bool IsOn = false;
    public static bool Optionmenu_IsOn = false;

    private NetworkManager networkManager;

    [SerializeField]
    GameObject optionsMenu;
    [SerializeField]
    GameObject backButton;
    [SerializeField]
    GameObject[] ComponentsToDisable;


    void Start()
    {
        networkManager = NetworkManager.singleton;
        optionsMenu.SetActive(false);
        backButton.SetActive(false);
        Optionmenu_IsOn = false;
    }

    void Update()
    {

    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;

        if(!LAN.IsLANOn)
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);

        networkManager.StopHost();

        LAN.IsLANOn = false;

        networkManager.networkAddress = "localhost";
    }

    public void Options_ON()
    {
        optionsMenu.SetActive(true);
        Optionmenu_IsOn = optionsMenu.activeSelf;
        backButton.SetActive(Optionmenu_IsOn);
        Disable_Enable_Components(Optionmenu_IsOn);
    }

    public void Options_OFF()
    {
        optionsMenu.SetActive(false);
        Optionmenu_IsOn = optionsMenu.activeSelf;
        backButton.SetActive(Optionmenu_IsOn);
        Disable_Enable_Components(Optionmenu_IsOn);
    }

    void Disable_Enable_Components(bool state)
    {
        if (state)
        {
            for (int i = 0; i < ComponentsToDisable.Length; i++)
            {
                ComponentsToDisable[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < ComponentsToDisable.Length; i++)
            {
                ComponentsToDisable[i].SetActive(true);
            }
        }
    }

}
