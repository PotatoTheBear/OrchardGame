using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Ranged")]
public class RangedWeaponData : WeaponData
{
    public float bulletSpeed;
    public int maxAmmo;
    public int reserveAmmo;
    public float reloadTime;
    [HideInInspector] public int currentAmmo; 
}

public class RangedWeaponState : WeaponState
{
    public float bulletSpeed;
    public int maxAmmo;
    public int reserveAmmo;
    public float reloadTime;
    public int currentAmmo;
    public RangedWeaponState(RangedWeaponData data) : base(data)
    {
        bulletSpeed = data.bulletSpeed;
        maxAmmo = data.maxAmmo;
        reserveAmmo = data.reserveAmmo;
        reloadTime = data.reloadTime;
        currentAmmo = data.maxAmmo;
    }
}
