using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gameTimer = 0f;
    public int killCount = 0;

    public int currentWave = 1;
    public float waveDuration = 0f;
    public event Action<int> KillCountChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void FixedUpdate()
    {
        gameTimer += Time.fixedDeltaTime;
    }

    public void RegisterKill()
    {
        killCount++;
        KillCountChanged?.Invoke(killCount);
    }
}
