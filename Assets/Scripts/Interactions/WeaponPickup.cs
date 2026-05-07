using System.Collections;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private Interactable interactable;
    [HideInInspector] public GameObject weapon;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();

        interactable.SetInteract(Interact);
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
