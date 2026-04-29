using UnityEngine;

[AddComponentMenu("Weapons/MeleeWeapon")]
public class MeleeWeapon : Weapons
{
    public GameObject slashAttack;

    protected MeleeWeaponState typeData;
    private void Start()
    {
        typeData = weaponData as MeleeWeaponState;
    }

    public override void Attack(Vector2 position, float zRotation, bool isPlayer)
    {
        base.Attack();
        if (!typeData.canAttack)
            return;
        StartCoroutine(Cooldown());
        GameObject newSlash = Instantiate(slashAttack);
        newSlash.transform.rotation = Quaternion.Euler(0, 0, zRotation);
        newSlash.transform.position = position;
        Slash newSlashScript = newSlash.GetComponent<Slash>();
        newSlashScript.damage = typeData.damage;
        newSlashScript.radius = typeData.radius;
        newSlashScript.angle = typeData.angle;
        newSlashScript.isPlayer = isPlayer;
    }

    private void OnDrawGizmos()
    {
        GameObject handle = transform.parent.name == "Handle" ? transform.parent.gameObject : WeaponManager.Handle;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(handle.transform.position, typeData.radius);

        Vector3 left = Quaternion.Euler(0, 0, typeData.angle / 2) * handle.transform.right;
        Vector3 right = Quaternion.Euler(0, 0, -typeData.angle / 2) * handle.transform.right;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(handle.transform.position, handle.transform.position + left * typeData.radius);
        Gizmos.DrawLine(handle.transform.position, handle.transform.position + right * typeData.radius);
    }
}
