using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CarWeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private Transform turretHolder;

    [SerializeField]
    private CarWeapon primaryWeapon;

    private CarWeapon currentWeapon;
    private CarWeaponGraphics currentGraphics;
    private Component turretGraphics;

    public bool isReloading = false;

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    /// /////////////////////////////////////
    public CarWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public CarWeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }
    /////////////////////////////////////////

    /////////////////////////////////////////
    void EquipWeapon(CarWeapon _weapon)
    {
        currentWeapon = _weapon;

        //////////////////////////////gun init/////////////////////////////////////////
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.gungraphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        
        ////////////////////////////////////////////////////////////////////////////////

        currentGraphics = _weaponIns.GetComponent<CarWeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.LogError("No Weapon Graphics Component on weapon_object:" + _weaponIns.name);
        }

        if (isLocalPlayer)
        {
            //_weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
            CarUtil.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }

        ////////////////////////// turret init //////////////////////////////////////
        GameObject _turretIns = (GameObject)Instantiate(_weapon.turretgraphics, turretHolder.position, turretHolder.rotation);
        _turretIns.transform.SetParent(turretHolder);
        turretGraphics = _turretIns.GetComponentInChildren<Animator>();
        /////////////////////////////////////////////////////////////////////////////
    }
    ////////////////////////////////////////
    public void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(Reload_Coroutine());
    }

    private IEnumerator Reload_Coroutine()
    {
        isReloading = true;
        //Debug.Log("Reloading");

        CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reload_time);
        //reload
        currentWeapon.bullets = currentWeapon.maxBullets;

        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim1 = currentGraphics.GetComponent<Animator>();
        Animator anim2 = turretGraphics.GetComponent<Animator>(); 
        if ( anim1 != null && anim2 != null)
        {
            anim1.SetTrigger("Reload");
            anim2.SetTrigger("Reload");
        }
    }

}
