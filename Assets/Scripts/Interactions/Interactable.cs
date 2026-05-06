using System.Collections.Generic;
using UnityEngine;
using System;


[AddComponentMenu("Interactions/Interactable")]
public class Interactable : MonoBehaviour
{
    public static List<Interactable> All = new();
    private Action interactAction;
    public void SetInteract(Action action) => interactAction = action;

    public void Interact() => interactAction?.Invoke();

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    public static Interactable GetClosest(Vector2 position, float minDist = Mathf.Infinity)
    {
        Interactable closest = null;

        foreach (Interactable interactable in All)
        {
            float distance = (new Vector2(interactable.transform.position.x, interactable.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = interactable;
                minDist = distance;
            }
        }

        return closest;
    }
}
