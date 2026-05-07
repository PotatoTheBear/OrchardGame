using UnityEngine;

public class AutoAim : IAim
{
    // Aim at the closest enemy
    public void Aim(bool isPlayer, GameObject handle)
    {
        Creature closestEnemyCreature = Creature.GetClosestEnemy(handle.transform.position, isPlayer);
        if (closestEnemyCreature == null)
            return;
        GameObject closestEnemy = closestEnemyCreature.gameObject;

        var angle = Mathf.Atan2(closestEnemy.transform.position.y - handle.transform.position.y, closestEnemy.transform.position.x - handle.transform.position.x);
        var deg = Mathf.Rad2Deg * angle;

        handle.transform.rotation = Quaternion.Euler(0, 0, deg);
    }
}
