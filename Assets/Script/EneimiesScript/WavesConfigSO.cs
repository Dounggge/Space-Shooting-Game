using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "WavesConfig")]
public class WavesConfigSO : ScriptableObject
{
    [SerializeField] Transform pathPrefab;
    [SerializeField] Transform spawnPrefab;
    [SerializeField] float enemyMoveSpeed = 1f;
    [SerializeField] float timeBetweenEnemySpawns = 1f;
    [SerializeField] float enemySpawnVariance = 0.5f;
    [SerializeField] float minimumSpawnTime = 0.5f;
    public Transform[] GetWayPoint()
    {
        Transform[] waypoints = new Transform[pathPrefab.childCount];
        for (int i = 0; i < pathPrefab.childCount; i++)
        {
            waypoints[i] = pathPrefab.GetChild(i);
        }
        return waypoints;
    }

    public Transform[] GetSpawnPoint()
    {
        Transform[] spawnPoints = new Transform[spawnPrefab.childCount];
        for (int i = 0; i < spawnPrefab.childCount; i++)
        {
            spawnPoints[i] = spawnPrefab.GetChild(i);
        }
        return spawnPoints;
    }

    public float GetMoveSpeed()
    {
        return enemyMoveSpeed;
    }

    public float GetEnemySpawnTime()
    {
        float spawnTime = Random.Range(timeBetweenEnemySpawns - enemySpawnVariance, 
                                       timeBetweenEnemySpawns + enemySpawnVariance);
        return spawnTime = Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);

    }

}