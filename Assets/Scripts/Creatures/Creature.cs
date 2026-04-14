using UnityEngine;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{
    public static List<Creature> All = new();
    [SerializeField] protected float maxHp;
    [SerializeField] protected float hp;
    [SerializeField] protected float movementSpeed;

    private void OnEnable() => All.Add(this);
    private void OnDisable() => All.Remove(this);

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

    public static Creature GetClosestEnemy(Vector2 position, float minDist = Mathf.Infinity)
    {
        Creature closest = null;

        foreach (Creature creature in All)
        {
            if (creature.gameObject.CompareTag("Player")) 
                continue;

            float distance = (new Vector2(creature.transform.position.x, creature.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = creature;
                minDist = distance;
            }
        }
            
        return closest;
    }
}
