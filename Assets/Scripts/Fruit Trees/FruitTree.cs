using System.Collections.Generic;
using UnityEngine;

public abstract class FruitTree : MonoBehaviour
{
    public static List<FruitTree> All = new();

    [SerializeField] protected float hp;
    [SerializeField] protected float fruitPerDay;
    [SerializeField] protected GameObject fruit;
    [SerializeField] protected GameObject weapon;

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    public void DropFruit()
    {
        for(int i = 0; i < fruitPerDay; i++)
        {
            Vector2 randDirection = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

            GameObject newFruit = Instantiate(fruit);

            newFruit.transform.position = new Vector3(transform.position.x, transform.position.y, newFruit.transform.position.z);

            Rigidbody2D rb = newFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = randDirection;
            }

        }
    }

    public static FruitTree GetClosest(Vector2 position, float minDist = Mathf.Infinity)
    {
        FruitTree closest = null;

        foreach (FruitTree fruitTree in All)
        {
            float distance = (new Vector2(fruitTree.transform.position.x, fruitTree.transform.position.y) - position).sqrMagnitude;

            if (distance < minDist)
            {
                closest = fruitTree;
                minDist = distance;
            }
        }

        return closest;
    }
}
