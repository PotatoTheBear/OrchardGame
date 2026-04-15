using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weaponInventory = new();
    private int currentWeaponIndex = 0;

    [SerializeField] public GameObject currentWeapon;

    private PlayerInputManager inputManager;
    
    public static IAim Aim { get; set; }
    public static GameObject Handle { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Handle = GameObject.Find("Handle");
        inputManager = PlayerInputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float swapValue = inputManager.SwapPressed();
        if (swapValue != 0)
        {
            currentWeaponIndex += 1;
            if (currentWeaponIndex >= weaponInventory.Count) currentWeaponIndex -= weaponInventory.Count;
            else if (currentWeaponIndex < 0) currentWeaponIndex += weaponInventory.Count; // Weapon switching. Next step > Destroy existing weapon instantiate new. Job for tomorrow
        }

        if (inputManager.ShootPressed())
        {
            currentWeapon.GetComponent<Weapons>().Attack(transform.position);
        }

        Aim.Aim();
    }

    void AimWeapon()
    {
        if (inputManager.isKBM)
        {
            // Aim at mouse
            Vector2 lookVector = Camera.main.ScreenToWorldPoint(inputManager.GetAimInput());
            var angle = Mathf.Atan2(lookVector.y - Handle.transform.position.y, lookVector.x - Handle.transform.position.x);
            var deg = Mathf.Rad2Deg * angle;
            Handle.transform.rotation = Quaternion.Euler(0, 0, deg);
        } 
        else
        {
            // Aim at closest target if exists
            Creature closestEnemyCreature = Creature.GetClosestEnemy(transform.position);
            if (closestEnemyCreature == null)
                return;
            GameObject closestEnemy = closestEnemyCreature.gameObject;

            var angle = Mathf.Atan2(closestEnemy.transform.position.y - transform.position.y, closestEnemy.transform.position.x - transform.position.x);
            var deg = Mathf.Rad2Deg * angle;

            Handle.transform.rotation = Quaternion.Euler(0, 0, deg);
        }


    }

    //private void OnDrawGizmos()
    //{
    //    if (currentWeapon == null) return;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, currentWeapon.GetComponent<Weapons>().range);

    //    if ()
    //    Vector3 left = Quaternion.Euler(0, 0, 45 / 2) * transform.right;
    //    Vector3 right = Quaternion.Euler(0, 0, -45 / 2) * transform.right;

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(transform.position, transform.position + left * currentWeapon.GetComponent<Weapons>().range);
    //    Gizmos.DrawLine(transform.position, transform.position + right * currentWeapon.GetComponent<Weapons>().range);
    //}

}
