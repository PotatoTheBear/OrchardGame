using UnityEngine;
using System.Collections;

public class ExplosiveWeapon : RangedWeapon
{
    public GameObject slashAttack;
    protected new ExplosiveWeaponState typeData;

    protected override void Start()
    {
        typeData = weaponData as ExplosiveWeaponState;
    }

    public override void SetBulletStats(GameObject newBullet, bool isPlayer)
    {
        ExplosiveBullet newBulletScript = newBullet.GetComponent<ExplosiveBullet>();
        newBulletScript.damage *= typeData.damage * typeData.explosionDamageMultiplier;
        newBulletScript.explosionRadius = typeData.explosionRadius;
        newBulletScript.slashAttack = slashAttack;
        newBulletScript.isPlayer = isPlayer;
        newBullet.GetComponent<Rigidbody2D>().linearVelocity = newBullet.transform.right * typeData.bulletSpeed;
    }

    public override void Attack(Vector2 position, float zRotation, bool isPlayer)
    {
        base.Attack();
        Debug.Log(typeData.currentAmmo);
        Debug.Log(typeData.canAttack);
        Shoot(position, zRotation, isPlayer, typeData);
    }
}
