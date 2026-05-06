using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Interactions/Sellable")]
public class Sellable : MonoBehaviour
{
    public static List<Sellable> All = new();
    private Action sellAction;
    public void SetSell(Action action) => sellAction = action;
    public void Sell() => sellAction?.Invoke();
    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);
    public static Sellable GetClosest(Vector2 position, float minDist = Mathf.Infinity)
    {
        Sellable closest = null;

        foreach (Sellable sellable in All)
        {
            float distance = (new Vector2(sellable.transform.position.x, sellable.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = sellable;
                minDist = distance;
            }
        }

        return closest;
    }

}
