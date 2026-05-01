using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveBullet : Bullet
{
    public float explosionRadius;
    public GameObject slashAttack;

    private void OnDestroy()
    {
        GameObject newSlash = Instantiate(slashAttack);
        newSlash.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        newSlash.transform.position = transform.position;
        Slash newSlashScript = newSlash.GetComponent<Slash>();
        newSlashScript.damage = damage;
        newSlashScript.radius = explosionRadius;
        newSlashScript.angle = 359.9f;
        newSlashScript.isPlayer = isPlayer;
    }
}
