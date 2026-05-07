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
    private IAim aim;
    private GameObject handleGameObject;
    private Weapons[] weaponsCache;
    [SerializeField] private bool attackAllWeapons = true;
    [SerializeField] private EnemyBehavior behavior = EnemyBehavior.CloserTarget;

    private void Start()
    {
        movement = new EntityMovement();
        aim = new AutoAim();
        var handle = transform.Find("Handle");
        if (handle != null)
        {
            handleGameObject = handle.gameObject;
            weaponsCache = handleGameObject.GetComponentsInChildren<Weapons>();
        }
        else
        {
            weaponsCache = new Weapons[0];
        }
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
            // if the player is closer than the closest tree, move towards the player, otherwise move towards the tree
            if (playerSqr <= treeSqr)
                movement.MoveToPlayer(this);
            else
                movement.MoveToTree(this);
        }
        // Aim using cached handle if available
        if (handleGameObject != null)
            aim.Aim(false, handleGameObject);
        else
            aim.Aim(false, gameObject);

        var pos = transform.position;
        var rotZ = transform.rotation.eulerAngles.z;

        // Attack with weapons (mode controlled by attackAllWeapons)
        if (weaponsCache != null && weaponsCache.Length > 0)
        {
            if (attackAllWeapons)
            {
                foreach (var w in weaponsCache)
                    w?.Attack(pos, rotZ, false);
            }
            else
            {
                // only use the first weapon (order comes from GetComponentsInChildren)
                weaponsCache[0]?.Attack(pos, rotZ, false);
            }
        }
        Death();
    }
}
