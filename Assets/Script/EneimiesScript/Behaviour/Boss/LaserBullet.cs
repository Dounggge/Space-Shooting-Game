using UnityEngine;

public class LaserBullet : Bullet
{
    [SerializeField] float waveSpeed = 8f;
    [SerializeField] int waveDamage = 15;
    [SerializeField] float waveLifeTime = 5f;
    [SerializeField] LayerMask waveTargetLayer;

    Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        speed = waveSpeed;
        damage = waveDamage;
        lifeTime = waveLifeTime;
        targetLayer = waveTargetLayer;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            direction = (player.transform.position - transform.position).normalized;
        else
            direction = Vector2.down;
    }

    protected override void BulletPath()
    {
        rb.linearVelocity = direction * speed;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0) return;
        if (other.TryGetComponent<Health>(out var health))
            health.TakeDamage(GetDamage());
    }
}