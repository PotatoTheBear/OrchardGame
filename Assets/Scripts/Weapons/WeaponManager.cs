using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponInventory = new();
    public static List<GameObject> WeaponInventory { get; private set; } = new();
    private int currentWeaponIndex = 0;

    [SerializeField] public GameObject currentWeapon;

    private PlayerInputManager inputManager;
    
    public static IAim Aim { get; set; }
    public static GameObject Handle { get; private set; }

    private void Awake()
    {
        foreach (var obj in WeaponInventory)
        {
            if (!weaponInventory.Contains(obj))
                weaponInventory.Add(obj);
        }
        WeaponInventory = weaponInventory;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Handle = GameObject.Find("Handle");
        inputManager = PlayerInputManager.Instance;
        SpawnNewWeapon(weaponInventory[currentWeaponIndex]);
        StartCoroutine(OnWeaponAmountChanged());
    }

    // Update is called once per frame
    void Update()
    {
        float swapValue = inputManager.SwapPressed();
        if (swapValue != 0)
        {
            currentWeaponIndex += 1;
            if (currentWeaponIndex >= weaponInventory.Count) currentWeaponIndex -= weaponInventory.Count;
            else if (currentWeaponIndex < 0) currentWeaponIndex += weaponInventory.Count;
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

    private IEnumerator OnWeaponAmountChanged()
    {
        yield return null;
        while (true)
        {
            int currentAmount = weaponInventory.Count;
            yield return new WaitUntil(() => currentAmount != weaponInventory.Count);
            if (currentWeaponIndex >= weaponInventory.Count) currentWeaponIndex -= weaponInventory.Count;
            else if (currentWeaponIndex < 0) currentWeaponIndex += weaponInventory.Count; 
            SpawnNewWeapon(weaponInventory[currentWeaponIndex]);
        }
    }
}
