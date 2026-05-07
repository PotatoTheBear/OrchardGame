using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FruitTree : MonoBehaviour
{
    public static List<FruitTree> All = new();
    public static List<string> ActiveWeapons = new();

    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float fruitPerDay;
    [SerializeField] protected GameObject fruit;
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected GameObject weaponPickup;

    [SerializeField] protected Sprite weaponSprite;
    protected string weaponName;

    public string typeName;

    private GameObject uiCanvas;
    private Slider uiSlider;
    private float SliderMax
    {
        set
        {
            uiSlider.maxValue = value;
        }
    }
    private float SliderValue 
    { 
        set 
        {
            uiSlider.value = value;
        } 
    }

    private void Awake()
    {
        if (TryGetComponent(out Interactable interactable))
        {
            interactable.SetInteract(WaterTree);
        }
    }

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

    public void TakeDMG(float dmg)
    {
        hp -= dmg;
    }

    public void Heal(float amount)
    {
        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }

    public void WaterTree()
    {
        float curWater = Player.Instance.currentWater;
        float waterAmount = Player.Instance.healPerWater;
        if (curWater > 0 && hp < maxHp)
        {
            Heal(waterAmount);
            Player.Instance.currentWater--;
        }
    }

    public float GetHP() { return hp; }

    public virtual void Death()
    {
        if (hp <= 0)
        {
            // Die
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        uiCanvas = transform.Find("TreeHealthUI").gameObject;
        uiSlider = uiCanvas.transform.Find("hp_slider").GetComponent<Slider>();
        UpdateMaxHP();
        UpdateHealth();
        StartCoroutine(WaitForValueChange(() => maxHp, delegate { UpdateMaxHP(); }));
        StartCoroutine(WaitForValueChange(() => hp, delegate { UpdateHealth(); }));
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

    public static IEnumerator WaitForValueChange(System.Func<float> value, UnityEngine.Events.UnityAction call, bool repeating = true)
    {
        do
        {
            yield return null;
            float oldValue = value();
            yield return new WaitUntil(() => oldValue != value());
            call();
        } while (repeating);
    }

    void UpdateHealth()
    {
        SliderValue = hp;
    }

    void UpdateMaxHP()
    {
        SliderMax = maxHp;
        UpdateHealth();
    }
}
