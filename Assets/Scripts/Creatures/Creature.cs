using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Creature : MonoBehaviour
{
    public static List<Creature> All = new();
    [SerializeField] protected float maxHp;
    [SerializeField] protected float hp;
    [SerializeField] protected float movementSpeed;

    private void OnEnable() => All.Add(this);
    private void OnDisable() => All.Remove(this);

    public void TakeDMG(float dmg) 
    { 
        hp -= dmg;
        StartCoroutine(DamageTick());
    }

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

    public static Creature GetClosestEnemy(Vector2 position, bool isPlayer, float minDist = Mathf.Infinity)
    {
        if (!isPlayer)
        {
            return All.Find(creature => creature.gameObject.CompareTag("Player"));
        }
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

    private IEnumerator DamageTick()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Color oldColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = oldColor;
        }
        yield return null;
    }
}
