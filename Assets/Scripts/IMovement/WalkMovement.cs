using UnityEngine;

public class WalkMovement : IMovement
{
    public void Move(Creature creature)
    {
        Vector2 input = PlayerInputManager.Instance.GetMoveInput();
        Vector3 dir = new Vector3(input.x, input.y, 0);

        creature.transform.position += dir * creature.GetMovementSpeed() * Time.deltaTime;
    }
}