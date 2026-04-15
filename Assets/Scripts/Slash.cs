using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float lifetime = 0.2f;
    [HideInInspector] public float damage;
    [HideInInspector] public float radius;
    [HideInInspector] public float angle;

    public GameObject colliderObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
        GenerateCollider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCollider()
    {
        PolygonCollider2D col = colliderObj.GetComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[3];
        points[0] = Vector2.zero;

        float halfAngle = angle / 2;

        for (int i = -1; i < 2; i += 2)
        {
            float currentAngle = (halfAngle * i);
            float rad = Mathf.Deg2Rad * currentAngle;

            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            points[i == -1 ? 1 : 2] = new Vector2(x, y);
        }
        col.points = points;
    }

    
}
