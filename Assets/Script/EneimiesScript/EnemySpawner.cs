using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WavesConfigSO[] waves;
    [SerializeField] Pools enemyPool;

    [Header("Scene to load after all waves")]
    [SerializeField] private int nextSceneBuildIndex = 2;

    WavesConfigSO currentWave;
    int enemiesSpawned = 0;
    int enemiesAlive = 0;
    bool waveStarted = false;

    void Start()
    {
        if (waves == null || waves.Length == 0) return;
        if (enemyPool == null) return;
        WaypointOccupancy.Clear();
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        foreach (WavesConfigSO wave in waves)
        {
            currentWave = wave;
            enemiesSpawned = 0;
            enemiesAlive = 0;
            waveStarted = false;

            WaypointOccupancy.Clear();

            yield return StartCoroutine(SpawnWave(wave));

            yield return new WaitUntil(() => waveStarted && enemiesAlive <= 0);

            yield return new WaitForSeconds(wave.GetTimeBetweenWaves());
        }

        yield return new WaitForSeconds(1f);

        if (SceneManage.instance != null)
        {
            switch (nextSceneBuildIndex)
            {
                case 1: SceneManage.instance.LoadGameplay1Scene(); break;
                case 2: SceneManage.instance.LoadGameplay2Scene(); break;
                case 3: SceneManage.instance.LoadGameplay3Scene(); break;
                default: SceneManager.LoadScene(nextSceneBuildIndex); break;
            }
        }
        else
        {
            SceneManager.LoadScene(nextSceneBuildIndex);
        }
    }

    IEnumerator SpawnWave(WavesConfigSO wave)
    {
        while (enemiesSpawned < wave.GetEnemyCount())
        {
            SpawnEnemy();
            yield return new WaitForSeconds(wave.GetEnemySpawnTime());
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = enemyPool.SetObject();
        if (enemy == null) return;

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null) enemyHealth.InitEnemy(enemyPool, this);

        PathFinding pf = enemy.GetComponent<PathFinding>();
        if (pf != null)
            pf.InitPath(this);

        EnemyBehaviourAbstract behaviour = enemy.GetComponent<EnemyBehaviourAbstract>();
        if (behaviour != null)
            behaviour.InitBehaviour(this);
        else if (pf == null)
        {
            enemyPool.ReturnObject(enemy);
            return;
        }

        enemy.SetActive(true);
        enemiesSpawned++;
        enemiesAlive++;
        waveStarted = true;
    }

    public void OnEnemyRemoved()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    public WavesConfigSO GetCurrentWave() => currentWave;
    public Pools EnemyPool() => enemyPool;
}