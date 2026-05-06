using UnityEngine;

public class HPTreeCanvasScript : MonoBehaviour
{
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        GetComponent<Canvas>().worldCamera = cam;
    }
}
