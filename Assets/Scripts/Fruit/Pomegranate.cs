using UnityEngine;

public class Pomegranate : Fruit
{
    [SerializeField] protected int ammoValue;

    public override void Interact()
    {
        base.Interact();
        if (WeaponManager.currentWeaponsData.TryGetValue(weaponName, out WeaponState weaponData)) {
            RangedWeaponState rangedData = weaponData as RangedWeaponState;
            rangedData.reserveAmmo += ammoValue;
        }
    }
}
