using UnityEngine;
using UnityEngine.UI;

public class UserAccount_Lobby : MonoBehaviour {

    public Text usernameText;

    //Sounds
    public AudioSource m_MenuAudio;
    public AudioClip m_Click;

    void Start ()
    {
        if (UserAccountManager.IsLoggedIn || UserAccountManager.IsLoginBypassed)
        {
            usernameText.text = UserAccountManager.LoggedIn_Username;
        }
    }

    public void LogOut()
    {
        ClickAudio();

        if (UserAccountManager.IsLoggedIn || UserAccountManager.IsLoginBypassed)
        {
            UserAccountManager.instance.LogOut();
        }
    }

    private void ClickAudio(){
        m_MenuAudio.clip = m_Click;
        m_MenuAudio.Play();
    }
	
}
