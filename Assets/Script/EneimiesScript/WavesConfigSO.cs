using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "WavesConfig")]
public class WavesConfigSO : ScriptableObject
{
    [SerializeField] Transform pathPrefab;
    [SerializeField] Transform spawnPrefab;

    [Header("Enemy Stats")]
    [SerializeField] float enemyMoveSpeed = 1f;

    [Header("Wave Settings")]
    [SerializeField] int enemyCount = 5;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] float timeBetweenEnemySpawns = 1f;
    [SerializeField] float enemySpawnVariance = 0.5f;
    [SerializeField] float minimumSpawnTime = 0.5f;

    public Transform[] GetWayPoint()
    {
        Transform[] waypoints = new Transform[pathPrefab.childCount];
        for (int i = 0; i < pathPrefab.childCount; i++)
            waypoints[i] = pathPrefab.GetChild(i);
        return waypoints;
    }

    public Transform[] GetSpawnPoint()
    {
        Transform[] spawnPoints = new Transform[spawnPrefab.childCount];
        for (int i = 0; i < spawnPrefab.childCount; i++)
            spawnPoints[i] = spawnPrefab.GetChild(i);
        return spawnPoints;
    }

    public float GetMoveSpeed() => enemyMoveSpeed;
    public int GetEnemyCount() => enemyCount;
    public float GetTimeBetweenWaves() => timeBetweenWaves;
    public float GetEnemySpawnTime()
    {
        float spawnTime = Random.Range(timeBetweenEnemySpawns - enemySpawnVariance,
                                       timeBetweenEnemySpawns + enemySpawnVariance);
        return Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);
    }
}