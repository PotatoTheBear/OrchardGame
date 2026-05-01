using UnityEngine;

public class EntityMovement : IEnemyMovement
{
    public void MoveToTree(Creature creature)
    {
        var closest = FruitTree.GetClosest(new Vector2(creature.transform.position.x, creature.transform.position.y));
        if (closest == null) return;

        Vector2 targetPos = closest.transform.position;
        Vector3 direction = new Vector3(targetPos.x, targetPos.y, 0) - creature.transform.position;

        creature.transform.position += direction.normalized * creature.GetMovementSpeed() * Time.deltaTime;
    }
    public void MoveToPlayer(Creature creature)
    {
        if (Player.Instance == null) return;

        Vector2 targetPos = Player.Instance.transform.position;
        Vector3 direction = new Vector3(targetPos.x, targetPos.y, 0) - creature.transform.position;
        creature.transform.position += direction.normalized * creature.GetMovementSpeed() * Time.deltaTime;
    }
}
