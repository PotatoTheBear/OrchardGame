using System.Collections.Generic;
using UnityEngine;

public abstract class Fruit : MonoBehaviour
{
    public static List<Fruit> All = new();

    [SerializeField] protected float sellValue;
    [SerializeField] protected float decayMultiplier;

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    public void Sell()
    {
        // Grab money here and increase the value
        Debug.Log($"Selling fruit. Giving {sellValue} coins");
        Destroy(gameObject);
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with fruit");
        Destroy(gameObject);
    }

    public static Fruit GetClosest(Vector2 position, float minDist = Mathf.Infinity)
    {
        Fruit closest = null;

        foreach (Fruit fruit in All)
        {
            float distance = (new Vector2(fruit.transform.position.x, fruit.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = fruit;
                minDist = distance;
            }
        }

        return closest;
    }
}
