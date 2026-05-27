using System.Collections;
using UnityEngine;

public class FighterBehaviour : EnemyBehaviourAbstract
{
    [Header("Fighter Settings")]
    [SerializeField] private float stopDistance = 0.5f;

    [Header("Bob Settings")]
    [SerializeField] private float bobAmplitude = 0.15f;
    [SerializeField] private float bobFrequency = 2f;

    private PathFinding pathFinding;
    private Transform claimedWaypoint;
    private bool hasStopped = false;

    private float bobTimer = 0f;
    private float anchorY;
    private Coroutine bobCoroutine;

    protected override void OnInit()
    {
        hasStopped = false;
        bobTimer = 0f;
        ReleaseClaim();

        pathFinding = GetComponent<PathFinding>();
        if (pathFinding == null) return;

        Transform[] wayPoints = WaveConfig?.GetWayPoint();
        if (wayPoints == null || wayPoints.Length < 2) return;

        Vector3 spawnPos = transform.position;
        const float minDistFromSpawn = 1.0f;
        claimedWaypoint = null;

        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            if (Vector3.Distance(spawnPos, wayPoints[i].position) < minDistFromSpawn)
                continue;

            if (WaypointOccupancy.TryOccupy(wayPoints[i]))
            {
                claimedWaypoint = wayPoints[i];
                break;
            }
        }

        if (claimedWaypoint != null)
        {
            pathFinding.SetTarget(claimedWaypoint);
        }
        else
        {
            pathFinding.SetTarget(wayPoints[wayPoints.Length - 1]);
        }
    }

    void OnDisable() => ReleaseClaim();

    protected override void OnBehaviourUpdate()
    {
        if (pathFinding == null || hasStopped) return;

        if (claimedWaypoint != null &&
            Vector3.Distance(transform.position, claimedWaypoint.position) < stopDistance)
        {
            hasStopped = true;
            pathFinding.Pause();
            transform.position = claimedWaypoint.position;
            anchorY = transform.position.y;

            bobCoroutine = StartCoroutine(InfiniteBob());
        }
    }

    private IEnumerator InfiniteBob()
    {
        while (true)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float offset = Mathf.Sin(bobTimer) * bobAmplitude;
            transform.position = new Vector3(transform.position.x, anchorY + offset, transform.position.z);
            yield return null;
        }
    }

    protected override void OnStop()
    {
        if (bobCoroutine != null)
        {
            StopCoroutine(bobCoroutine);
            bobCoroutine = null;
        }
        hasStopped = false;
        ReleaseClaim();
    }

    private void ReleaseClaim()
    {
        if (claimedWaypoint != null)
        {
            WaypointOccupancy.Release(claimedWaypoint);
            claimedWaypoint = null;
        }
    }
}