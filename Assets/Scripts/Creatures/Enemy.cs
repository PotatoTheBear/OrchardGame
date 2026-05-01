using UnityEngine;

public enum EnemyBehavior
{
    TreesOnly,
    PlayerOnly,
    CloserTarget
}

public class Enemy : Creature
{
    private IEnemyMovement movement;
    [SerializeField] private EnemyBehavior behavior = EnemyBehavior.CloserTarget;

    private void Start()
    {
        movement = new EntityMovement();
    }

    void Update()
    {
        if (behavior == EnemyBehavior.TreesOnly)
        {
            movement.MoveToTree(this);
        }
        else if (behavior == EnemyBehavior.PlayerOnly)
        {
            movement.MoveToPlayer(this);
        }
        else
        {
            // determine whether the player or the closest tree is the nearer target
            var enemyPos2 = new Vector2(transform.position.x, transform.position.y);

            float playerSqr = Mathf.Infinity;
            if (Player.Instance != null)
            {
                var playerPos = Player.Instance.transform.position;
                playerSqr = (new Vector2(playerPos.x, playerPos.y) - enemyPos2).sqrMagnitude;
            }

            var closestTree = FruitTree.GetClosest(enemyPos2);
            float treeSqr = Mathf.Infinity;
            if (closestTree != null)
            {
                var treePos = closestTree.transform.position;
                treeSqr = (new Vector2(treePos.x, treePos.y) - enemyPos2).sqrMagnitude;
            }

            if (playerSqr <= treeSqr)
                movement.MoveToPlayer(this);
            else
                movement.MoveToTree(this);
        }

        Death();
    }
}
