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
        SpawnNewWeapon(weaponInventory[currentWeaponIndex]);
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
            SpawnNewWeapon(weaponInventory[currentWeaponIndex]);
        }

        if (inputManager.ShootPressed())
        {
            currentWeapon.GetComponent<Weapons>().Attack(transform.position);
        }

        Aim.Aim();
    }

    void SpawnNewWeapon(GameObject newWeapon)
    {
        foreach (Transform child in Handle.transform)
        {
            Destroy(child.gameObject);
        }
        currentWeapon = Instantiate(newWeapon, Handle.transform);
    }
}
