using UnityEngine;

public abstract class Bullet : MonoBehaviour, IDamageDealer
{
    protected float speed;
    protected int damage;
    protected float lifeTime;
    protected LayerMask targetLayer;

    protected Rigidbody2D rb;
    private Pools poolBullet;
    protected float timer;

    public virtual int GetDamage() => damage;
    public LayerMask LayerDamage => targetLayer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(Pools pool)
    {
        poolBullet = pool;
    }

    protected virtual void OnEnable()
    {
        if (poolBullet == null)
            poolBullet = GetComponentInParent<Pools>();

        timer = lifeTime;
    }

    public virtual void InitBullet(LayerMask newTargetLayer)
    {
        targetLayer = newTargetLayer;
        timer = lifeTime;
    }

    protected virtual void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            ReturnToPool();
    }

    protected virtual void FixedUpdate()
    {
        BulletPath();
    }

    protected abstract void BulletPath();

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.TryGetComponent<Health>(out var health))
            health.TakeDamage(GetDamage());

        ReturnToPool();
    }

    protected void ReturnToPool()
    {
        if (poolBullet != null)
            poolBullet.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }
}