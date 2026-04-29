using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public bool isPlayer;

    private void Start()
    {
        Destroy(gameObject, 30f);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.gameObject.GetComponent<Creature>().TakeDMG(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.TakeDMG(damage);
                Destroy(gameObject);
            }
        }
    }
}
