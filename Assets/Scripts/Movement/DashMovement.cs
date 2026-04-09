using UnityEngine;

public class DashMovement : IMovement
{
    private Vector3 dashDirection;
    private float dashSpeedMultiplier = 3f;

    public DashMovement(Vector3 direction)
    {
        dashDirection = direction.normalized;
    }

    public void Move(Creature creature)
    {
        creature.transform.position += dashDirection * creature.GetMovementSpeed() * dashSpeedMultiplier * Time.deltaTime;
    }
}
