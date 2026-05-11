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

    // Reference to EnemySpawner for wave timer
    [SerializeField] private EnemySpawner enemySpawner;

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

    public float GetWaveTimeRemaining()
    {
        if (enemySpawner != null)
        {
            return enemySpawner.GetWaveTimeRemaining();
        }
        return 0f;
    }

    public bool IsWaveActive()
    {
        if (enemySpawner != null)
        {
            return enemySpawner.IsWaveActive();
        }
        return false;
    }
}
