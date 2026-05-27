using UnityEngine;

public class Autocanon : Bullet
{
    [SerializeField] private float canonSpeed = 10f;
    [SerializeField] private int canonDamage = 10;
    [SerializeField] private float canonLifeTime = 5f;
    [SerializeField] private LayerMask canonTargetLayer;

    protected override void Awake()
    {
        base.Awake();
        speed = canonSpeed;
        damage = canonDamage;
        lifeTime = canonLifeTime;
        targetLayer = canonTargetLayer;
    }

    protected override void BulletPath()
    {
        rb.linearVelocity = Vector2.up * speed;
    }
}