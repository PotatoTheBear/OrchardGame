using System.Collections;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected const float ATTACK_SPEED = 1;
    [HideInInspector] public WeaponState weaponData;
    public string weaponName;

    private void Awake()
    {
        weaponData ??= WeaponManager.CreateState(WeaponManager.AllWeaponsData.Find(w => w.weaponName == weaponName));
        StartCoroutine(Cooldown());
    }

    public virtual void Attack()
    {
    }
    public virtual void Attack(Vector2 position, float zRotation, bool isPlayer) 
    {
    }

    public IEnumerator Cooldown()
    {
        weaponData.canAttack = false;
        yield return new WaitForSeconds(ATTACK_SPEED / weaponData.attackrate);
        weaponData.canAttack = true;
    }

}
