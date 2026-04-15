using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float lifetime = 0.2f;
    public int particleCount = 40;
    [HideInInspector] public float damage;
    [HideInInspector] public float radius;
    [HideInInspector] public float angle;

    public GameObject colliderObj;
    public GameObject slashAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
        angle = Mathf.Clamp(angle, -359.9f, 359.9f);
        GenerateCollider();
        VisualSlash();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCollider()
    {
        PolygonCollider2D col = colliderObj.GetComponent<PolygonCollider2D>();

        int segments = Mathf.Max(2, Mathf.CeilToInt(angle / 10f));

        Vector2[] points = new Vector2[segments + 2];
        points[0] = Vector2.zero;

        float halfAngle = angle / 2;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);
            float rad = Mathf.Deg2Rad * currentAngle;

            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            points[i + 1] = new Vector2(x, y);
        }
        col.points = points;
    }

    void VisualSlash()
    {
        GameObject obj = Instantiate(slashAnimation, transform.position, transform.rotation);

        SlashMesh slash = obj.GetComponent<SlashMesh>();
        slash.radius = radius;
        slash.angle = angle;
    }
}
