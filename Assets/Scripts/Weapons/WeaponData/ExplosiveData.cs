using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Explosive")]
public class ExplosiveWeaponData : RangedWeaponData
{
    public float explosionRadius;
    public float explosionDamageMultiplier;
}

public class ExplosiveWeaponState : RangedWeaponState
{
    public float explosionRadius;
    public float explosionDamageMultiplier;
    public ExplosiveWeaponState(ExplosiveWeaponData data) : base(data)
    {
        explosionRadius = data.explosionRadius;
        explosionDamageMultiplier = data.explosionDamageMultiplier;
    }
}