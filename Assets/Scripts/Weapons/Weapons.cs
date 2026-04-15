using System.Collections;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected const float ATTACK_SPEED = 1;
    [SerializeField] protected float attackrate = 5;
    [SerializeField] protected float BASE_DMG = 5;
    protected bool canAttack = true;
    protected float damage => BASE_DMG;
    public string WeaponName => gameObject.name;

    public virtual void Attack()
    {
    }
    public virtual void Attack(Vector2 position) 
    {
    }

    public IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(ATTACK_SPEED / attackrate);
        canAttack = true;
    }

}
