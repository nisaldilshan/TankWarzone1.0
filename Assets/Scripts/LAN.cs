using UnityEngine;
using UnityEngine.Networking;

public class LAN : MonoBehaviour {

    public static bool IsLANOn = false;

    //Sounds
    public AudioSource m_MenuAudio;
    public AudioClip m_Click;

    private string IPAddress;

    private NetworkManager networkManager;

    void Start ()
    {
        networkManager = NetworkManager.singleton;
    }
	
    public void HostGame()
    {
        //Sound
        ClickAudio();
        
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

    private void ClickAudio(){
        m_MenuAudio.clip = m_Click;
        m_MenuAudio.Play();
    }
}
