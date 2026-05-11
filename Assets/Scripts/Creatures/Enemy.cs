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
        // Determine the preferred target (player or closest tree) based on behavior
        GameObject targetObj = null;
        Vector2 targetPos = Vector2.zero;
        bool targetIsPlayer = false;

        if (behavior == EnemyBehavior.TreesOnly)
        {
            var closestTree = FruitTree.GetClosest(new Vector2(transform.position.x, transform.position.y));
            if (closestTree != null)
            {
                targetObj = closestTree.gameObject;
                targetPos = closestTree.transform.position;
            }
        }
        else if (behavior == EnemyBehavior.PlayerOnly)
        {
            if (Player.Instance != null)
            {
                targetObj = Player.Instance.gameObject;
                targetPos = Player.Instance.transform.position;
                targetIsPlayer = true;
            }
        }
        else // CloserTarget
        {
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

            if (playerSqr <= treeSqr && Player.Instance != null)
            {
                targetObj = Player.Instance.gameObject;
                targetPos = Player.Instance.transform.position;
                targetIsPlayer = true;
            }
            else if (closestTree != null)
            {
                targetObj = closestTree.gameObject;
                targetPos = closestTree.transform.position;
            }
        }

         // Determine if player is in ranged attack range (for aiming)
        bool playerInRangedRange = false;
        Vector2 playerPos2 = Vector2.zero;
        if (Player.Instance != null)
        {
            var enemyPos2 = new Vector2(transform.position.x, transform.position.y);
            playerPos2 = new Vector2(Player.Instance.transform.position.x, Player.Instance.transform.position.y);
            float playerDist = (playerPos2 - enemyPos2).sqrMagnitude;
            float rangedAttackRange = 20f; // Adjust this value for ranged attack range
            playerInRangedRange = playerDist <= (rangedAttackRange * rangedAttackRange);
        }

        // Aim at the player if in ranged range, otherwise aim at chosen target
        if (playerInRangedRange && Player.Instance != null)
        {
            var handleTransform = (handleGameObject != null) ? handleGameObject.transform : transform;
            var angle = Mathf.Atan2(playerPos2.y - handleTransform.position.y, playerPos2.x - handleTransform.position.x);
            var deg = Mathf.Rad2Deg * angle;
            handleTransform.rotation = Quaternion.Euler(0, 0, deg);
        }
        else if (targetObj != null)
        {
            var handleTransform = (handleGameObject != null) ? handleGameObject.transform : transform;
            var angle = Mathf.Atan2(targetPos.y - handleTransform.position.y, targetPos.x - handleTransform.position.x);
            var deg = Mathf.Rad2Deg * angle;
            handleTransform.rotation = Quaternion.Euler(0, 0, deg);
        }
        else
        {
            if (handleGameObject != null)
                aim.Aim(false, handleGameObject);
            else
                aim.Aim(false, gameObject);
        }

        // Cache position/rotation for attacks
        var pos = transform.position;
        var rotZ = (handleGameObject != null) ? handleGameObject.transform.rotation.eulerAngles.z : transform.rotation.eulerAngles.z;

        // Compute maximum melee radius from available weapons (0 if none)
        float maxMeleeRadius = 0f;
        if (weaponsCache != null)
        {
            foreach (var w in weaponsCache)
            {
                if (w == null || w.weaponData == null) continue;
                if (w.weaponData is MeleeWeaponState mstate)
                    maxMeleeRadius = Mathf.Max(maxMeleeRadius, mstate.radius);
            }
        }

        // Determine if we're in melee range of the chosen target (include a small buffer)
        bool inMeleeRange = false;
        if (targetObj != null)
        {
            var enemyPos2 = new Vector2(transform.position.x, transform.position.y);
            float sqrDist = (new Vector2(targetPos.x, targetPos.y) - enemyPos2).sqrMagnitude;
            float buffer = 0.5f;
            float threshold = maxMeleeRadius + buffer;
            inMeleeRange = sqrDist <= (threshold * threshold);
        }

        // If not in melee range, move toward the target. If in range, stop moving.
        if (!inMeleeRange && targetObj != null)
        {
            if (targetIsPlayer)
                movement.MoveToPlayer(this);
            else
                movement.MoveToTree(this);
        }

        // Attack: either melee OR ranged, not both
        if (weaponsCache != null && weaponsCache.Length > 0)
        {
            if (attackAllWeapons)
            {
                if (inMeleeRange)
                {
                    // only melee weapons
                    foreach (var w in weaponsCache)
                    {
                        if (w == null) continue;
                        if (w.weaponData is MeleeWeaponState)
                            w.Attack(pos, rotZ, false);
                    }
                }
                else if (playerInRangedRange)
                {
                    // ranged weapons on player if in range (regardless of primary target)
                    foreach (var w in weaponsCache)
                    {
                        if (w == null) continue;
                        if (!(w.weaponData is MeleeWeaponState))
                            w.Attack(pos, rotZ, false);
                    }
                }
            }
            else
            {
                // attack only the first weapon of the appropriate type
                if (inMeleeRange)
                {
                    foreach (var w in weaponsCache)
                    {
                        if (w == null) continue;
                        if (w.weaponData is MeleeWeaponState)
                        {
                            w.Attack(pos, rotZ, false);
                            break;
                        }
                    }
                }
            }
        }

        Death();
    }
}
