using UnityEngine;
using System.Collections;

public class ScoutBehaviour : EnemyBehaviourAbstract
{
    [Header("Scout Settings")]
    [SerializeField] private Pools bulletPool;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float stopDistance = 0.1f;

    private PathFinding pathFinding;
    private Transform chosenWaypoint;
    private bool hasStopped = false;
    private bool isShooting = false;

    protected override void OnInit()
    {
        hasStopped = false;
        isShooting = false;
        pathFinding = GetComponent<PathFinding>();
        if (pathFinding == null) return;

        Transform[] wayPoints = WaveConfig?.GetWayPoint();
        if (wayPoints == null || wayPoints.Length < 2) return;

        int randomIndex = Random.Range(0, wayPoints.Length - 1);
        chosenWaypoint = wayPoints[randomIndex];
        pathFinding.SetTarget(chosenWaypoint);
    }

    protected override void OnBehaviourUpdate()
    {
        if (pathFinding == null || hasStopped) return;

        if (chosenWaypoint != null &&
            Vector3.Distance(transform.position, chosenWaypoint.position) < stopDistance)
        {
            hasStopped = true;
            pathFinding.Pause();
            transform.position = chosenWaypoint.position;

            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootLoop());
            }
        }
    }

    protected override void OnStop()
    {
        StopAllCoroutines();
        isShooting = false;
    }

    IEnumerator ShootLoop()
    {
        while (!IsStopped)
        {
            FireBullet();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void FireBullet()
    {
        if (bulletPool == null) return;
        GameObject b = bulletPool.SetObject();
        if (b == null) return;
        b.transform.position = transform.position;
        b.transform.rotation = Quaternion.identity;

        if (b.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.SetPool(bulletPool);
            bullet.InitBullet(targetLayer);
        }
        b.SetActive(true);
    }
}