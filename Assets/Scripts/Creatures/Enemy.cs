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
    [SerializeField] private EnemyBehavior behavior = EnemyBehavior.CloserTarget;

    private void Start()
    {
        movement = new EntityMovement();
        aim = new AutoAim();
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

        // Aim at the chosen target if available; otherwise fallback to auto-aim behavior
        if (targetObj != null)
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
                else
                {
                    // only non-melee (ranged) weapons
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
                else
                {
                    foreach (var w in weaponsCache)
                    {
                        if (w == null) continue;
                        if (!(w.weaponData is MeleeWeaponState))
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
