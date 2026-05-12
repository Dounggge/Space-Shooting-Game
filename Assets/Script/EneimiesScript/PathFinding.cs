using UnityEngine;

public class PathFinding : MonoBehaviour
{
    EnemySpawner enemySpawner;
    WavesConfigSO waveConfig;
    Pools pool;
    Transform[] wayPoints;
    int wayPointIndex = 0;
    bool isReturned = false;

    void Update()
    {
        FollowPath();
    }

    public void Init(EnemySpawner spawner)
    {
        isReturned = false;
        wayPointIndex = 0;
        enemySpawner = spawner;

        if (enemySpawner == null)
        {
            Debug.LogError("PathFinding.Init: spawner is null!");
            return;
        }

        waveConfig = enemySpawner.GetCurrentWave();
        pool = enemySpawner.EnemyPool();

        if (waveConfig == null)
        {
            Debug.LogError("PathFinding.Init: waveConfig is null!");
            return;
        }

        wayPoints = waveConfig.GetWayPoint();
        transform.position = SetSpawnPoint();
    }

    void FollowPath()
    {
        // Guard: block all frames until Init() has fully run
        if (isReturned || wayPoints == null || wayPoints.Length == 0 || waveConfig == null || pool == null)
            return;

        // Reached end of path — return once and stop
        if (wayPointIndex >= wayPoints.Length)
        {
            isReturned = true;          // prevents re-entry this frame and next
            pool.ReturnObject(gameObject);
            return;
        }

        Vector3 targetPosition = wayPoints[wayPointIndex].position;
        float deltaMove = waveConfig.GetMoveSpeed() * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, deltaMove);

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            wayPointIndex++;
        }
    }

    private Vector3 SetSpawnPoint()
    {
        Transform[] spawnPoints = waveConfig.GetSpawnPoint();

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("PathFinding.SetSpawnPoint: no spawn points defined in waveConfig!");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}