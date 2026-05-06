using System;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public static List<Pickupable> All = new();

    public bool canInteract;
    public bool canSell;

    private Action interactAction;
    private Action sellAction;

    public void SetInteract(Action action) => interactAction = action;
    public void SetSell(Action action) => sellAction = action;

    public void Interact() => interactAction?.Invoke();
    public void Sell() => sellAction?.Invoke();

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    public static Pickupable GetClosest(Vector2 position, float minDist = Mathf.Infinity)
    {
        Pickupable closest = null;

        foreach (Pickupable pickupable in All)
        {
            float distance = (new Vector2(pickupable.transform.position.x, pickupable.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = pickupable;
                minDist = distance;
            }
        }

        return closest;
    }
}
