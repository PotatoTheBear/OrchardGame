using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    //private PlayerInput playerInput;

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private Vector2 aimInput;

    private InputActionMap activeMap;

    public bool isKBM;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        inputActions = new InputSystem_Actions();

        inputActions.KBM.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.KBM.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.KBM.Look.performed += ctx => aimInput = ctx.ReadValue<Vector2>();

        inputActions.Arcade.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Arcade.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Enable();

        inputActions.Arcade.Disable();
        inputActions.KBM.Enable();
        activeMap = inputActions.KBM;
        WeaponManager.Aim = new ManualAim();
    }

    public Vector2 GetMoveInput() { return moveInput; }
    public Vector2 GetAimInput() { return aimInput; }

    public bool DashPressed() => activeMap["Dash"].triggered;
    public bool ShootPressed() => activeMap["Attack"].triggered;

    public bool InteractPressed() => activeMap["Interact"].triggered;
    public bool InventoryPressed() => activeMap["Inventory"].triggered;
    public bool SellPressed() => activeMap["Sell"].triggered;
    public float SwapPressed() => activeMap["Swap"].ReadValue<float>();

    public bool FruitPressed() => inputActions.KBM.SpawnFruit.triggered;

    public void SwitchControlScheme()
    {
        isKBM = !isKBM;
        
        if (isKBM)
        {
            //playerInput.SwitchCurrentControlScheme("Keyboard&Mouse");
            inputActions.Arcade.Disable();
            inputActions.KBM.Enable();
            activeMap = inputActions.KBM;
            WeaponManager.Aim = new ManualAim();
        }
        else
        {
            //playerInput.SwitchCurrentControlScheme("Arcade Kast");
            inputActions.KBM.Disable();
            inputActions.Arcade.Enable();
            activeMap = inputActions.Arcade;
            WeaponManager.Aim = new AutoAim();
        }
    }
}
