using UnityEngine;

public class Weapons : MonoBehaviour
{
    protected const float ATTACK_SPEED = 1;
    [SerializeField] protected float attackrate = 5;
    [HideInInspector] public float cooldown = 1;
    [SerializeField] protected float BASE_DMG = 5;
    protected float damage;
    public string WeaponName => gameObject.name;

    public virtual void Attack()
    {

    }

}
