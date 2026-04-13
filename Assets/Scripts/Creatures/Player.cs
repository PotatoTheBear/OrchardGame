using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature
{
    [SerializeField] protected float dashCooldown;
    private PlayerInputManager inputManager;

    private IMovement movement;

    private Coroutine dashCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = PlayerInputManager.Instance;

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
        }

        if (inputManager.SellPressed())
        {
            // Om fruit te verkopen. Ik zit te denken zoek het dichtsbijzijnde fruit, en dan activeer de functie in de fruit script (fruit class waar elk fruit van inherit, met sell en interact functie)
        }

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
}
