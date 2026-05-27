using System.Collections;
using UnityEngine;

public class BomberBehaviour : EnemyBehaviourAbstract
{
    [Header("Bomber Settings")]
    [SerializeField] private float waitBeforeDash = 2f;
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private int dashDamage = 20;
    [SerializeField] private float explosionRange = 0.5f;

    private enum State { Waiting, Dashing, Dead }
    private State state;
    private Vector3 dashTarget;
    private float waitTimer;

    private EnemyHealth enemyHealth;

    protected override void OnInit()
    {
        state = State.Waiting;
        waitTimer = waitBeforeDash;
        enemyHealth = GetComponent<EnemyHealth>();

        Transform[] spawnPoints = WaveConfig?.GetSpawnPoint();
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[BomberBehaviour] not found spawn!");
            ReturnToPool();
            return;
        }
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }

    protected override void OnBehaviourUpdate()
    {
        switch (state)
        {
            case State.Waiting:
                HandleWaiting();
                break;
            case State.Dashing:
                HandleDashing();
                break;
        }
    }

    protected override void OnStop()
    {
        state = State.Dead;
        StopAllCoroutines();
        ReturnToPool();
    }

    private void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer > 0f) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            ReturnToPool();
            return;
        }

        dashTarget = player.transform.position;
        state = State.Dashing;
    }

    private void HandleDashing()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, dashTarget, dashSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, dashTarget) < explosionRange)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (state != State.Dashing) return;
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<Health>(out var playerHealth))
            playerHealth.TakeDamage(dashDamage);

        Explode();
    }

    private void Explode()
    {
        if (state == State.Dead) return;
        state = State.Dead;

        if (enemyHealth != null)
            enemyHealth.Kill();
        else
            ReturnToPool();
    }

    private void OnDisable()
    {
        state = State.Dead;
        StopAllCoroutines();
    }
}