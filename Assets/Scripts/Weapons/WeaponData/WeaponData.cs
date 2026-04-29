using UnityEngine;
//[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float damage;
    [Tooltip("Amount of times the weapon can attack per second")]
    public float attackrate = 5;
    [HideInInspector] public bool canAttack = true;
}

public class WeaponState
{
    public string weaponName;
    public float damage;
    public float attackrate;
    public bool canAttack;

    public WeaponState(WeaponData data)
    {
        weaponName = data.weaponName;
        damage = data.damage;
        attackrate = data.attackrate;
        canAttack = data.canAttack;
    }
}
