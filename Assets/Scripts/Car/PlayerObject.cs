using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarSetup))]
public class PlayerObject : NetworkBehaviour {

    /////////////////////////////////////////
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    /////////////////////////////////////////

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\\\
    [SyncVar]
    public string username = "Loading ...";

    public int Kills;
    public int Deaths;
    //\/////////////////////////

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disable_graphics_OnDeath;

    [SerializeField]
    private Collider[] colliders_disableOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    private Rigidbody r_body;
    
    void Start()
    {
        r_body = GetComponent<Rigidbody>();
    }

    public void Setup()
    {
        //Switch Cameras
        if (isLocalPlayer)
        {
            GameManagerCar.instance.SetSceneCameraActive(false);
            //
            GameManagerCar.instance.SetminimapActive(true);
            //
            GetComponent<CarSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllTheClients();
    }
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    [ClientRpc]
    private void RpcSetupPlayerOnAllTheClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(99999, username);
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= _amount;
        //Debug.Log(transform.name + " now has " + currentHealth + "health.");

        if (currentHealth <= 0)
        {  
            Die(_sourceID);
        }
    }

    public void Die(string _sourceID)
    {
        isDead = true;

        PlayerObject sourcePlayer = GameManagerCar.GetPlayer(_sourceID);

        if(sourcePlayer != null)
        {
            sourcePlayer.Kills++;
            GameManagerCar.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        Deaths++;

        //DISABLE COMPONENTS
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //DISABLE Graphics
        for (int i = 0; i < disable_graphics_OnDeath.Length; i++)
        {
            disable_graphics_OnDeath[i].SetActive(false);
        }

        //disable colliders
        for (int i = 0; i < colliders_disableOnDeath.Length; i++)
        {
            colliders_disableOnDeath[i].enabled = false;
        }

        //instantiate death effect
        GameObject _gfxins = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxins, 3f);

        //Switch Cameras
        if (isLocalPlayer)
        {
            GameManagerCar.instance.SetSceneCameraActive(true);
            //
            GameManagerCar.instance.SetminimapActive(false);
            //
            GetComponent<CarSetup>().playerUIInstance.SetActive(false);
        }

        r_body.useGravity = false;

        Debug.Log(transform.name + " is Dead! ");

        //CALL RESPAWN METHOD

        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManagerCar.instance.matchsettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.5f);

        Setup();

        Debug.Log(transform.name + " respawned");

    }

    public void SetDefaults()
    {
        r_body.useGravity = true;

        isDead = false;

        currentHealth = maxHealth;

        //Enable Components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //ENABLE Graphics
        for (int i = 0; i < disable_graphics_OnDeath.Length; i++)
        {
            disable_graphics_OnDeath[i].SetActive(true);
        }

        //enable colliders
        for (int i = 0; i < colliders_disableOnDeath.Length; i++)
        {
            colliders_disableOnDeath[i].enabled = true;
        }

        WaitforTime(0.05f);

        //Instantiate Spawn Effect
        GameObject _gfxspawn = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxspawn, 3f);
    }

    private IEnumerator WaitforTime(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
