using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CarWeaponManager))]
public class CarShoot : NetworkBehaviour{

    private CarWeapon currentWeapon;
    private CarWeaponManager weaponManager;

    private const string PLAYER_TAG = "PlayerCollider";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Player Shoot : No Camera Referenced");
            this.enabled = false;
        }

        weaponManager = GetComponent<CarWeaponManager>();
    }

    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.IsOn)
            return;

        if(currentWeapon.bullets < currentWeapon.maxBullets)
        {
            if (Input.GetButtonDown("Reload"))
            {
                weaponManager.Reload();
                return;
            }
        }

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, (1f / currentWeapon.fireRate));
            }

            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////

    [Command]   // Is call on the server when the player shoots
    void CmdOnShoot()
    {
        RpcDoShootEffects();
    }


    [ClientRpc]   // Is called onall Clients by the server, to do the shoot effects
    void RpcDoShootEffects()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    //\/////////////////////////////////////////////////////
    [Command]    // Is call on the server when something hits
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffects(_pos, _normal);
    }

    [ClientRpc]        // Is called onall Clients by the server, to do the hit effects
    void RpcDoHitEffects(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 1.5f);
    }
    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }
        
        if(currentWeapon.bullets <= 0)
        {
            //Debug.Log("out of bullets");
            weaponManager.Reload();
            return;
        }

        currentWeapon.bullets--;
        

        //we are shooting, so call onshoot method onserver
        CmdOnShoot();

        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            //Debug.Log("We Hit" + _hit.collider.name);
            if (_hit.collider.tag == PLAYER_TAG)
            {
                string a = _hit.collider.name.Substring(0, 8); ;
                CmdPlayerShot(a, currentWeapon.damage, transform.name);   //_hit.collider.name
            }

            // do hit effects
            CmdOnHit(_hit.point, _hit.normal);
        }

        if (currentWeapon.bullets <= 0)
        {
            weaponManager.Reload();
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
    {
        Debug.Log(_playerID + "has been Shot.");

        PlayerObject _player = GameManagerCar.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }
}
