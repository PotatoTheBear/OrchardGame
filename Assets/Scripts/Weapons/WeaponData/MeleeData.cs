using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Melee")]
public class MeleeWeaponData : WeaponData
{
    public float radius;
    public float angle;
}

public class MeleeWeaponState : WeaponState
{
    public float radius;
    public float angle;
    public MeleeWeaponState(MeleeWeaponData data) : base(data)
    {
        radius = data.radius;
        angle = data.angle;
    }
}