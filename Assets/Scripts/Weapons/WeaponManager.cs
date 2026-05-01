using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class WeaponManager : MonoBehaviour
{
    public static Dictionary<string, WeaponState> currentWeaponsData = new();
    [SerializeField] private List<GameObject> weaponInventory = new();
    public static List<GameObject> WeaponInventory { get; private set; } = new();
    private int currentWeaponIndex = 0;

    [SerializeField] public GameObject currentWeapon;

    private PlayerInputManager inputManager;
    
    public static IAim Aim { get; set; }
    public static GameObject Handle { get; private set; }

    [SerializeField] private List<WeaponData> allWeaponsData = new();
    public static List<WeaponData> AllWeaponsData { get; private set; } = new();

    private void Awake()
    {
        foreach (var obj in WeaponInventory)
        {
            if (!weaponInventory.Contains(obj))
                weaponInventory.Add(obj);
        }
        WeaponInventory = weaponInventory;
        AllWeaponsData = allWeaponsData;
        //var data = new WeaponState(AllWeaponsData[0] as RangedWeaponData);
        //data.data.damage--;
        //(AllWeaponsData[0] as RangedWeaponData).currentAmmo--;
        //ScriptableObject.c
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Handle = transform.Find("Handle").gameObject;
        Debug.Log(Handle);
        inputManager = PlayerInputManager.Instance;
        if (weaponInventory.Count > 0)
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

        if (inputManager.ShootPressed() && currentWeapon != null)
        {
            currentWeapon.GetComponent<Weapons>().Attack(transform.position, Handle.transform.rotation.eulerAngles.z, true);
        }

        Aim.Aim(true, Handle);
    }

    void SpawnNewWeapon(GameObject newWeapon)
    {
        foreach (Transform child in Handle.transform)
        {
            Destroy(child.gameObject);
        }
        currentWeapon = Instantiate(newWeapon, Handle.transform);
        var currentWeaponScript = currentWeapon.GetComponent<Weapons>();
        if (!currentWeaponsData.ContainsKey(currentWeaponScript.weaponName))
            currentWeaponsData.Add(currentWeaponScript.weaponName, CreateState(AllWeaponsData.Find(w => w.weaponName == currentWeaponScript.weaponName)));
        currentWeaponScript.weaponData = currentWeaponsData[currentWeaponScript.weaponName];
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

    public static WeaponState CreateState(WeaponData data)
    {
        Debug.Log(data.weaponName);
        if (data is ExplosiveWeaponData explosiveData) // IMPORTANT >> This is nested from RangedWeaponData, so must be checked first
            return new ExplosiveWeaponState(explosiveData);
        else if (data is RangedWeaponData rangedData)
            return new RangedWeaponState(rangedData);
        else if (data is MeleeWeaponData meleeData)
            return new MeleeWeaponState(meleeData);
        else
            return new WeaponState(data);
    }
}
