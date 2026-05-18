using UnityEngine;

public class PathFinding : MonoBehaviour
{
    EnemySpawner enemySpawner;
    WavesConfigSO waveConfig;
    Pools pool;
    Transform[] wayPoints;
    int wayPointIndex = 0;
    bool isReturned = false;
    bool isDying = false;

    void Update()
    {
        if (isDying) return;
        FollowPath();
    }

    public void InitPath(EnemySpawner spawner)
    {
        isDying = false;
        isReturned = false;
        wayPointIndex = 0;
        enemySpawner = spawner;

        if (enemySpawner == null)
        {
            //Debug.LogError("PathFinding.Init: spawner is null!");
            return;
        }

        waveConfig = enemySpawner.GetCurrentWave();
        pool = enemySpawner.EnemyPool();

        if (waveConfig == null)
        {
            //Debug.LogError("PathFinding.Init: waveConfig is null!");
            return;
        }

        wayPoints = waveConfig.GetWayPoint();
        transform.position = SetSpawnPoint();
    }

    public void StopMovement()
    {
        isDying = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
    }


    void FollowPath()
    {
        if (isReturned || wayPoints == null || wayPoints.Length == 0 || waveConfig == null || pool == null)
            return;

        if (wayPointIndex >= wayPoints.Length)
        {
            isReturned = true;
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
            //Debug.LogError("PathFinding.SetSpawnPoint: no spawn points defined in waveConfig!");
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}