using UnityEngine;

public class EntityMovement : IMovement
{
    public void Move(Creature creature)
    {
        Vector2 targetPos = FruitTree.GetClosest(creature.transform.position).transform.position;
        Vector3 direction = new Vector3(targetPos.x, targetPos.y, 0) - creature.transform.position;

        creature.transform.position += direction.normalized * creature.GetMovementSpeed() * Time.deltaTime;
    }
}
