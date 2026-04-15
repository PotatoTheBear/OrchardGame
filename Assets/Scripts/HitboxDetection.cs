using System.Collections.Generic;
using UnityEngine;

public class HitboxDetection : MonoBehaviour
{
    private HashSet<GameObject> hitEnemies = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Creature creature) && !collision.CompareTag("Player") && !hitEnemies.Contains(collision.gameObject))
        {
            creature.TakeDMG(gameObject.GetComponentInParent<Slash>().damage);
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            hitEnemies.Add(collision.gameObject);
        }
    }
}
