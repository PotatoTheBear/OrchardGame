using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FruitTree : MonoBehaviour
{
    public static List<FruitTree> All = new();
    public static List<string> ActiveWeapons = new();

    [SerializeField] protected float hp;
    [SerializeField] protected float fruitPerDay;
    [SerializeField] protected GameObject fruit;
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected GameObject weaponPickup;

    [SerializeField] protected Sprite weaponSprite;
    protected string weaponName;

    public string typeName;

    void OnEnable()
    {
        All.Add(this);
    }
    void OnDisable()
    {
        All.Remove(this);
        
        if (!All.Exists(obj => obj.typeName == typeName))
        {
            WeaponManager.WeaponInventory.Remove(weapon);
            ActiveWeapons.Remove(weapon.name);
        }
    }

    private IEnumerator Start()
    {
        yield return null;
        if (!WeaponManager.WeaponInventory.Contains(weapon) && !ActiveWeapons.Contains(weapon.name))
        {
            DropWeapon();
        }
        weaponName = weapon.GetComponent<Weapons>().weaponName;
    }

    public void DropFruit()
    {
        for(int i = 0; i < fruitPerDay; i++)
        {
            Vector2 randDirection = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

            GameObject newFruit = Instantiate(fruit);
            newFruit.GetComponent<Fruit>().weaponName = weaponName;

            newFruit.transform.position = new Vector3(transform.position.x, transform.position.y, newFruit.transform.position.z);

            Rigidbody2D rb = newFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = randDirection;
            }

        }
    }

    void DropWeapon()
    {
        Vector2 randDirection = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        GameObject newPickup = Instantiate(weaponPickup);
        ActiveWeapons.Add(weapon.name);

        WeaponPickup weaponP = newPickup.GetComponent<WeaponPickup>();
        weaponP.weapon = weapon;
        newPickup.name = weapon.name + " Pickup";
        newPickup.GetComponent<SpriteRenderer>().sprite = weaponSprite;

        newPickup.transform.position = new Vector3(transform.position.x, transform.position.y, newPickup.transform.position.z);

        Rigidbody2D rb = newPickup.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = randDirection;
        }
    }

    public static FruitTree GetClosest(Vector2 position, float minDist = Mathf.Infinity)
    {
        FruitTree closest = null;

        foreach (FruitTree fruitTree in All)
        {
            float distance = (new Vector2(fruitTree.transform.position.x, fruitTree.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = fruitTree;
                minDist = distance;
            }
        }

        return closest;
    }
}
