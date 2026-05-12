using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WavesConfigSO[] waves;
    [SerializeField] Pools enemyPool;
    [SerializeField] int enemyCount = 5;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] bool isSpawning = false;

    WavesConfigSO currentWave;
    int currentWaveIndex = 0;

    void Start()
    {
        enemyPool.InitPool(enemyCount);
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        do
        {
            foreach (WavesConfigSO wave in waves)
            {
                currentWave = wave;
                currentWaveIndex++;
                Debug.Log($"Starting wave {currentWaveIndex}");

                yield return StartCoroutine(SpawnWave(wave));

                // Wait between waves
                yield return new WaitForSeconds(timeBetweenWaves);
            }

        }
        while (isSpawning);
        Debug.Log("All waves complete.");
    }

    IEnumerator SpawnWave(WavesConfigSO wave)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(wave.GetEnemySpawnTime());
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = enemyPool.SetObject();

        if (enemy == null)
        {
            Debug.LogError("EnemySpawner.SpawnEnemy: pool returned null!");
            return;
        }

        PathFinding pf = enemy.GetComponent<PathFinding>();

        if (pf == null)
        {
            Debug.LogError("EnemySpawner.SpawnEnemy: PathFinding missing on prefab!");
            enemyPool.ReturnObject(enemy);
            return;
        }

        pf.Init(this);
    }

    public WavesConfigSO GetCurrentWave() => currentWave;
    public Pools EnemyPool() => enemyPool;
}