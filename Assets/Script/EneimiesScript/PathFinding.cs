using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private WavesConfigSO waveConfig;
    private Pools pool;
    private Transform target;
    private bool isPaused = false;
    private bool isDying = false;

    public void InitPath(EnemySpawner spawner)
    {
        enemySpawner = spawner;
        if (spawner != null)
        {
            waveConfig = spawner.GetCurrentWave();
            pool = spawner.EnemyPool();
        }
        isDying = false;
        isPaused = false;
        target = null;

        Transform[] spawnPoints = waveConfig?.GetSpawnPoint();
        if (spawnPoints != null && spawnPoints.Length > 0)
            transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        else
            transform.position = Vector3.zero;
    }

    public void SetTarget(Transform newTarget) => target = newTarget;

    public void Pause()
    {
        isPaused = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;
    }

    public void Resume() => isPaused = false;

    public void StopMovement()
    {
        isDying = true;
        Pause();
    }

    public void ReturnToPool()
    {
        enemySpawner?.OnEnemyRemoved();
        if (pool != null)
            pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDying || isPaused || target == null || waveConfig == null) return;

        float speed = waveConfig.GetMoveSpeed();
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            transform.position = target.position;
            target = null;
        }
    }
}