using UnityEngine;

[System.Serializable]
public class CarWeapon {

    public string name = "Mini";

    public int damage = 10;
    public float range = 100f;

    public float fireRate = 0f;

    public int maxBullets = 45;

    [HideInInspector]
    public int bullets;

    public float reload_time = 1.5f;

    public GameObject gungraphics;
    public GameObject turretgraphics;

    public CarWeapon()
    {
        bullets = maxBullets;
    }
}
