using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WavesConfigSO[] waves;
    [SerializeField] Pools enemyPool;
    [SerializeField] int enemyCount = 5;
    [SerializeField] float timeBetweenWaves = 5f;

    WavesConfigSO currentWave;
    int currentWaveIndex = 0;

    void Start()
    {
        enemyPool.InitPool(enemyCount);
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
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
        if (enemy == null) return;

        enemy.SetActive(false);

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.InitEnemy(enemyPool);
        }

        PathFinding pf = enemy.GetComponent<PathFinding>();
        if (pf == null)
        {
            enemyPool.ReturnObject(enemy);
            return;
        }
        pf.InitPath(this);

        enemy.SetActive(true);
    }

    public WavesConfigSO GetCurrentWave() => currentWave;
    public Pools EnemyPool() => enemyPool;
}