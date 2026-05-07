using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject Player;

    public float spawnIntervalBase = 5f;
    public float spawnRadius = 10f;
    public int enemiesPerWaveBase = 5;
    public float waveDuration = 20f;
    public float timeBetweenWaves = 3f;

    private float timer = 0f;
    private float waveTimer = 0f;
    private int enemiesSpawnedThisWave = 0;
    private int enemiesThisWaveMax = 0;
    private bool waveActive = false;

    private void Start()
    {
        if (Player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) Player = p;
        }

        StartWave();
    }

    private void Update()
    {
        if (Player == null)
        {
            return;
        }

        int currentWave = GameManager.Instance != null ? GameManager.Instance.currentWave : 1;

        if (!waveActive)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenWaves)
            {
                StartWave();
            }
            return;
        }

        float adjustedInterval = Mathf.Clamp(spawnIntervalBase - (currentWave * 0.2f), 0.3f, spawnIntervalBase);

        timer += Time.deltaTime;
        waveTimer += Time.deltaTime;

        if (timer >= adjustedInterval && enemiesSpawnedThisWave < enemiesThisWaveMax)
        {
            SpawnEnemy();
            timer = 0f;
            enemiesSpawnedThisWave++;
        }

        if (waveTimer >= waveDuration || enemiesSpawnedThisWave >= enemiesThisWaveMax)
        {
            EndWave();
        }
    }

    private void StartWave()
    {
        timer = 0f;
        waveTimer = 0f;
        enemiesSpawnedThisWave = 0;
        waveActive = true;

        int currentWave = GameManager.Instance != null ? GameManager.Instance.currentWave : 1;
        enemiesThisWaveMax = Mathf.Max(1, enemiesPerWaveBase + (currentWave - 1) * 2);
    }

    private void EndWave()
    {
        waveActive = false;
        timer = 0f;
        waveTimer = 0f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentWave++;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector2 spawnPos = (Vector2)Player.transform.position + Random.insideUnitCircle * spawnRadius;
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
