using UnityEngine;

public class MeleeWeapon : Weapons
{
    [SerializeField] protected float radius;
    [SerializeField] protected float angle;
    public GameObject slashAttack;


    public override void Attack(Vector2 position)
    {
        base.Attack();
        if (!canAttack)
            return;
        StartCoroutine(Cooldown());
        GameObject newSlash = Instantiate(slashAttack);
        newSlash.transform.rotation = Quaternion.Euler(0, 0, WeaponManager.Handle.transform.rotation.eulerAngles.z);
        newSlash.transform.position = position;
        Slash newSlashScript = newSlash.GetComponent<Slash>();
        newSlashScript.damage = damage;
        newSlashScript.radius = radius;
        newSlashScript.angle = angle;
    }

    private void OnDrawGizmos()
    {
        GameObject handle = GameObject.Find("Handle");
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(handle.transform.position, radius);

        Vector3 left = Quaternion.Euler(0, 0, angle / 2) * handle.transform.right;
        Vector3 right = Quaternion.Euler(0, 0, -angle / 2) * handle.transform.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(handle.transform.position, handle.transform.position + left * radius);
        Gizmos.DrawLine(handle.transform.position, handle.transform.position + right * radius);
    }
}
