using System.Collections;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private Pickupable pickupable;
    [HideInInspector] public GameObject weapon;

    private void Awake()
    {
        pickupable = GetComponent<Pickupable>();

        pickupable.SetInteract(Interact);
    }

    private void Start()
    {
        StartCoroutine(CheckForDestroy());
    }

    public void Interact()
    {
        if (!WeaponManager.WeaponInventory.Contains(weapon))
        {
            WeaponManager.WeaponInventory.Add(weapon);
        }
        FruitTree.ActiveWeapons.Remove(weapon.name);
        Destroy(gameObject);
    }

    IEnumerator CheckForDestroy()
    {
        while (true)
        {
            yield return new WaitUntil(() => !FruitTree.ActiveWeapons.Contains(weapon.name));
            Destroy(gameObject);
        }
    }
}
