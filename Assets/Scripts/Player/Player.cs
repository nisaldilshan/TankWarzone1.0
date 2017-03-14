using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {
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

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disable_graphics_OnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public void Setup()
    {
        //Switch Cameras
        if (isLocalPlayer)
        {
            GameManagerFPS.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
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


    //void Update()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(99999);
    //    }
    //}


    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= _amount;
        Debug.Log(transform.name + " now has " + currentHealth + "health.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

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

        //DISABLE COLLIDERS
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        //instantiate death effect
        GameObject _gfxins = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxins, 3f);

        //Switch Cameras
        if (isLocalPlayer)
        {
            GameManagerFPS.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is Dead! ");

        //CALL RESPAWN METHOD

        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManagerFPS.instance.matchsettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        Setup();

        Debug.Log(transform.name + " respawned");

    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        //ENABLE Components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //ENABLE Graphics
        for (int i = 0; i < disable_graphics_OnDeath.Length; i++)
        {
            disable_graphics_OnDeath[i].SetActive(true);
        }

        //ENABLE Colliders
        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }

        //Instantiate Spawn Effect
        GameObject _gfxspawn = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxspawn, 3f);

    }
}
