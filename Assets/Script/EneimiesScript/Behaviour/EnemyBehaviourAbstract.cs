using UnityEngine;

public abstract class EnemyBehaviourAbstract : MonoBehaviour
{
    protected EnemySpawner EnemySpawner { get; private set; }
    protected WavesConfigSO WaveConfig { get; private set; }
    protected Pools Pool { get; private set; }
    protected bool IsStopped { get; private set; }

    void Awake() => IsStopped = true;

    private void Update()
    {
        if (IsStopped) return;
        OnBehaviourUpdate();
    }

    public void InitBehaviour(EnemySpawner spawner)
    {
        IsStopped = false;
        EnemySpawner = spawner;

        if (spawner != null)
        {
            WaveConfig = spawner.GetCurrentWave();
            Pool = spawner.EnemyPool();
        }

        OnInit();
    }

    public void StopBehaviour()
    {
        IsStopped = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
        OnStop();
    }

    protected virtual void OnInit() { }
    protected abstract void OnBehaviourUpdate();
    protected virtual void OnStop() { }

    protected void ReturnToPool()
    {
        IsStopped = true;
        if (Pool != null) Pool.ReturnObject(gameObject);
    }
}