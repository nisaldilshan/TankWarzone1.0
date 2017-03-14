using UnityEngine;
using UnityEngine.Networking;

public class LAN : MonoBehaviour {

    public static bool IsLANOn = false;

    private string IPAddress;

    private NetworkManager networkManager;

    void Start ()
    {
        networkManager = NetworkManager.singleton;
    }
	
    public void HostGame()
    {
        networkManager.StartHost();
        IsLANOn = true;
    }


    public void SetIP(string _ip)
    {
        IPAddress = _ip;
        networkManager.networkAddress = IPAddress;
    }

    public void StartClient()
    {
        networkManager.StartClient();
        IsLANOn = true;
    }
}
