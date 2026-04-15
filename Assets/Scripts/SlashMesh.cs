    using UnityEngine;

public class SlashMesh : MonoBehaviour
{
    public int segments = 20;
    [HideInInspector] public float radius = 2f;
    [HideInInspector] public float angle = 90f;

    public float slashSpeed = 15f;
    float duration = 0.1f;
    private float timer;
    private float progress;
    Mesh mesh;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        duration = radius / slashSpeed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        progress = timer / duration;

        Generate(progress);

        if (progress >= 1f)
            Destroy(gameObject);
    }

    void Generate(float t)
    {
        mesh.Clear();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Color color = renderer.material.color;
        color.a = 1f - t;
        renderer.material.color = color;

        float currentRadius = radius * t;

        int segments = 20;

        Vector3[] vertices = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[segments * 6];

        float halfAngle = angle * 0.5f;

        for (int i = 0; i <= segments; i++)
        {
            float lerpT = (float)i / segments;
            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, lerpT);

            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            Vector3 point = dir * currentRadius;

            // thickness direction (perpendicular in 2D)
            Vector3 perp = new Vector3(-dir.y, dir.x, 0) * 1f; // thickness

            vertices[i * 2] = point - perp;
            vertices[i * 2 + 1] = point + perp;
        }
        int tri = 0;

        for (int i = 0; i < segments; i++)
        {
            int index = i * 2;

            triangles[tri++] = index;
            triangles[tri++] = index + 2;
            triangles[tri++] = index + 1;

            triangles[tri++] = index + 1;
            triangles[tri++] = index + 2;
            triangles[tri++] = index + 3;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}


