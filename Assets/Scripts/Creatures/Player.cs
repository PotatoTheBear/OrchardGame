using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Creature
{
    public static Player Instance { get; set; }

    public int currentWater;
    [HideInInspector] public int maxWater;
    public int healPerWater;

    [SerializeField] protected float dashCooldown;
    private PlayerInputManager inputManager;
    private float minInteractDistance = 16;

    private IMovement movement;

    private Coroutine dashCoroutine;

    private GameObject PlayerUI;
    Slider waterSlider;
    Slider hpSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = PlayerInputManager.Instance;
        Instance = this;
        maxWater = currentWater;
        PlayerUI = GameObject.Find("PlayerUI");
        waterSlider = PlayerUI.transform.Find("water_slider").GetComponent<Slider>();
        hpSlider = PlayerUI.transform.Find("hp_slider").GetComponent<Slider>();
        UpdateMaxSliderValue(hpSlider, maxHp);
        UpdateMaxSliderValue(waterSlider, maxWater);
        UpdateSliderValue(hpSlider, hp);
        UpdateSliderValue(waterSlider, currentWater);
        StartCoroutine(FruitTree.WaitForValueChange(() => maxHp, delegate { UpdateMaxSliderValue(hpSlider, maxHp); })); // Max HP Update
        StartCoroutine(FruitTree.WaitForValueChange(() => maxWater, delegate { UpdateMaxSliderValue(waterSlider, maxWater); })); // Max Water Update
        StartCoroutine(FruitTree.WaitForValueChange(() => hp, delegate { UpdateSliderValue(hpSlider, hp); })); // Current HP Update
        StartCoroutine(FruitTree.WaitForValueChange(() => currentWater, delegate { UpdateSliderValue(waterSlider, currentWater); })); // Current Water Update

        movement = new WalkMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.DashPressed())
        {
            Vector2 input = inputManager.GetMoveInput();
            Vector3 dashDir = new Vector3(input.x, input.y);
            if (dashDir != Vector3.zero && dashCoroutine == null)
                dashCoroutine = StartCoroutine(DashRoutine(dashDir));
        }

        if (inputManager.InteractPressed())
        {
            // Voor interact met bijv. fruit
            var closestFruit = Interactable.GetClosest(transform.position, minInteractDistance);
            if (closestFruit != null)
                closestFruit.Interact();
        }

        if (inputManager.SellPressed())
        {
            // Om fruit te verkopen. Ik zit te denken zoek het dichtsbijzijnde fruit, en dan activeer de functie in de fruit script (fruit class waar elk fruit van inherit, met sell en interact functie)
            var closestFruit = Sellable.GetClosest(transform.position, minInteractDistance);
            if (closestFruit != null)
                closestFruit.Sell();
        }

        if (inputManager.FruitPressed())
            foreach (var tree in FruitTree.All)
                tree.DropFruit();

        movement.Move(this);
    }

    IEnumerator DashRoutine(Vector3 direction)
    {
        movement = new DashMovement(direction);
        yield return new WaitForSeconds(0.2f);
        movement = new WalkMovement();
        yield return new WaitForSeconds(dashCooldown);
        dashCoroutine = null;
    }

    public static void UpdateSliderValue(Slider slider, float value) => slider.value = value;
    public static void UpdateMaxSliderValue(Slider slider, float value) => slider.maxValue = value;
}
