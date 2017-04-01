using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private GameObject CAR;

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject onlineMenu;
    [SerializeField]
    private GameObject lanMenu;

    [SerializeField]
    private GameObject onlinehostgame;
    [SerializeField]
    private GameObject lanhostgame;

    [SerializeField]
    private InputField nickname_text;

    //Sounds
    public AudioSource m_MenuAudio;
    public AudioClip m_Click;

    //Scripts
    [SerializeField]
    private GameObject hostgamescript;
    [SerializeField]
    private GameObject joingamescript;
    [SerializeField]
    private GameObject LANgamescript;


    public int number;


    void Start ()
    {
        mainMenu.SetActive(true);
        MapSelect.instance.ISHOST(false);
        MapSelect.instance.setNoCalls(0);
        CAR.SetActive(true);

    }


    public void MainMenu2Online()
    {
        mainMenu.SetActive(false);
        onlineMenu.SetActive(true);

        hostgamescript.SetActive(true);
        joingamescript.SetActive(true);
        LANgamescript.SetActive(true);

        //Sound
        ClickAudio();
    }


    public void MainMenu2LAN()
    {
        mainMenu.SetActive(false);
        lanMenu.SetActive(true);

        //hostgamescript.SetActive(true);
        //joingamescript.SetActive(true);
        LANgamescript.SetActive(true);

        //Sound
        ClickAudio();   
    }


    public void Online2MainMenu()
    {
        //Sound
        ClickAudio();
        
        onlineMenu.SetActive(false);
        mainMenu.SetActive(true);

        hostgamescript.SetActive(false);
        joingamescript.SetActive(false);
        LANgamescript.SetActive(false);
    }


    public void LAN2MainMenu()
    {
        //Sound
        ClickAudio();

        lanMenu.SetActive(false);
        mainMenu.SetActive(true);

        hostgamescript.SetActive(false);
        joingamescript.SetActive(false);
        LANgamescript.SetActive(false);
    }

    public void LAN2Host()
    {
        //Sound
        ClickAudio();

        lanMenu.SetActive(false);
        lanhostgame.SetActive(true);
        MapSelect.instance.SetMapNumber(0);
        MapSelect.instance.ISHOST(true);
    }


    public void Host2LAN()
    {
        //Sound
        ClickAudio();

        lanhostgame.SetActive(false);
        lanMenu.SetActive(true);
        MapSelect.instance.ISHOST(false);
    }


    public void Online2Host()
    {
        //Sound
        ClickAudio();

        onlineMenu.SetActive(false);
        onlinehostgame.SetActive(true);
        MapSelect.instance.SetMapNumber(0);
        MapSelect.instance.ISHOST(true);
    }


    public void Host2Online()
    {
        //Sound
        ClickAudio();

        onlinehostgame.SetActive(false);
        onlineMenu.SetActive(true);
        MapSelect.instance.ISHOST(false);
    }

    public void ToggleNickName()
    {
        //Sound
        ClickAudio();

        if (UserAccountManager.IsnicknameEnabled)
        {
            UserAccountManager.instance.NickName_OFF();
            nickname_text.interactable = false;
        }
        else
        {
            UserAccountManager.instance.NickName_ON();
            nickname_text.interactable = true;
        }

    }

    public void setNickName(string _nickname)
    {
        if (UserAccountManager.IsnicknameEnabled)
        {
            UserAccountManager.instance.SetNickName(_nickname);
        }
    }

    public void SetMapNumber(int _number)
    {
        number = _number;
        MapSelect.instance.SetMapNumber(number);
    }

    private void ClickAudio(){
        m_MenuAudio.clip = m_Click;
        m_MenuAudio.Play();
    }

}
