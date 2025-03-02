using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    Text maxAmmoText;

    //private Player player;
    private PlayerObject car;

    //private PlayerController controller1;
    private CarController controller2;

    private CarWeaponManager weaponManager;

    private int type;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreBoard;

    //
    public bool ssao = true;
    public bool motionblur = true;
    public bool bloom = true;
    public bool sunshafts = true;
    public bool reflections = true;
    //

    void Start()
    {
        PauseMenu.IsOn = false;
        pauseMenu.SetActive(false);
    }

    void Update ()
    {
        //if(type == 1)
            //SetFuelAmount(controller1.GetThrusterFuelAmount());
        //else 
        if(type == 2)
            SetFuelAmount(controller2.GetThrusterFuelAmount());

        //health
        SetHealthAmount(car.GetHealthPercentage());
        //ammo
        SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets, weaponManager.GetCurrentWeapon().maxBullets);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetAmmoAmount(int _amount, int max_amount)
    {
        ammoText.text = _amount.ToString();
        maxAmmoText.text = "/" + max_amount.ToString();
    }

    public void SetControllerType(int _type)
    {
        type = _type;
    }

    // public void SetPlayer(Player _player)
    // {
    //     player = _player;
    //     controller1 = player.GetComponent<PlayerController>();
    // }

    public void SetCar(PlayerObject _car)
    {
        car = _car;
        controller2 = car.GetComponent<CarController>();
        weaponManager = car.GetComponent<CarWeaponManager>();
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;

        PauseMenu.instance.Options_OFF();
    }

    
}
