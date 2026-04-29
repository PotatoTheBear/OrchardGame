using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[AddComponentMenu("Weapons/RangedWeapon")]
public class RangedWeapon : Weapons
{
    public GameObject bullet;
    protected RangedWeaponState typeData;

    protected virtual void Start()
    {
        typeData = weaponData as RangedWeaponState;
    }

    public override void Attack(Vector2 position, float zRotation, bool isPlayer)
    {
        base.Attack();
        Debug.Log(typeData.currentAmmo);
        Debug.Log(typeData.canAttack);
        Shoot(position, zRotation, isPlayer, typeData);
    }

    public virtual void Shoot(Vector2 position, float zRotation, bool isPlayer, RangedWeaponState typeData)
    {
        Debug.Log(typeData.reserveAmmo);
        if (!typeData.canAttack)
            return;
        else if (typeData.currentAmmo <= 0)
        {
            StartCoroutine(Reload(typeData));
            return;
        }
        GameObject newBullet = InstantiateBullet(position, zRotation, isPlayer);
        SetBulletStats(newBullet, isPlayer);
        typeData.currentAmmo--;
        if (typeData.currentAmmo <= 0)
            StartCoroutine(Reload(typeData));
        else
            StartCoroutine(Cooldown());
    }

    public virtual GameObject InstantiateBullet(Vector2 position, float zRotation, bool isPlayer)
    {
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.rotation = Quaternion.Euler(0, 0, zRotation);
        newBullet.transform.position = position;
        return newBullet;
    }

    public virtual void SetBulletStats(GameObject newBullet, bool isPlayer)
    {
        Bullet newBulletScript = newBullet.GetComponent<Bullet>();
        newBulletScript.isPlayer = isPlayer;
        newBulletScript.damage = typeData.damage;
        newBullet.GetComponent<Rigidbody2D>().linearVelocity = newBullet.transform.right * typeData.bulletSpeed;
    }

    public IEnumerator Reload(RangedWeaponState typeData)
    {
        if (typeData.currentAmmo == typeData.maxAmmo || typeData.reserveAmmo <= 0)
            StopCoroutine(Reload(typeData));
        typeData.canAttack = false;
        yield return new WaitForSeconds(typeData.reloadTime);
        int ammoNeeded = typeData.maxAmmo - typeData.currentAmmo;
        if (typeData.reserveAmmo >= ammoNeeded)
        {
            typeData.currentAmmo += ammoNeeded;
            typeData.reserveAmmo -= ammoNeeded;
        }
        else
        {
            typeData.currentAmmo += typeData.reserveAmmo;
            typeData.reserveAmmo = 0;
        }
        typeData.canAttack = true;
    }
}
