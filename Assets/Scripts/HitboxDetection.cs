using System.Collections.Generic;
using UnityEngine;

public class HitboxDetection : MonoBehaviour
{
    private HashSet<GameObject> hitEnemies = new();
    [HideInInspector] public bool isPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Creature creature))
        {
            if (isPlayer)
            {
                if (!collision.CompareTag("Player") && !hitEnemies.Contains(collision.gameObject))
                {
                    creature.TakeDMG(gameObject.GetComponentInParent<Slash>().damage);
                    hitEnemies.Add(collision.gameObject);
                }
            }
            else
            {
                if (collision.CompareTag("Player") && !hitEnemies.Contains(collision.gameObject))
                {
                    creature.TakeDMG(gameObject.GetComponentInParent<Slash>().damage);
                    hitEnemies.Add(collision.gameObject);
                }
            }
        }
    }
}
