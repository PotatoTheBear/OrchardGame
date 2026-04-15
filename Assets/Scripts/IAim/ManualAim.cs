using UnityEngine;

public class ManualAim : IAim
{
    // Aim at mouse
    public void Aim()
    {
        GameObject handle = WeaponManager.Handle;
        Vector2 lookVector = Camera.main.ScreenToWorldPoint(PlayerInputManager.Instance.GetAimInput());
        var angle = Mathf.Atan2(lookVector.y - handle.transform.position.y, lookVector.x - handle.transform.position.x);
        var deg = Mathf.Rad2Deg * angle;
        handle.transform.rotation = Quaternion.Euler(0, 0, deg);
    }
}
