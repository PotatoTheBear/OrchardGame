using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float hp;
    [SerializeField] protected float movementSpeed;


    public void TakeDMG(float dmg) { hp -= dmg; }

    public void Heal(float amount) 
    { 
        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }

    public float GetHP() { return hp; }

    public float GetMovementSpeed() { return movementSpeed; }

    public virtual void Death()
    {
        if (hp <= 0)
        {
            // Die
            Destroy(gameObject);
        }
    }
}
