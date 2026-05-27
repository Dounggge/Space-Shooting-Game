using UnityEngine;
using System.Collections;

public class EnemyBullet : Bullet
{
    [SerializeField] private float EBSpeed = 10f;
    [SerializeField] private int EBDamage = 10;
    [SerializeField] private float EBLifeTime = 5f;
    [SerializeField] private float EBReloadTime = 1f;
    [SerializeField] private LayerMask EBTargetLayer;

    Coroutine reloadCoroutine;

    protected override void Awake()
    {
        base.Awake();
        speed = EBSpeed;
        damage = EBDamage;
        lifeTime = EBLifeTime;
        targetLayer = EBTargetLayer;
    }

    protected override void BulletPath()
    {
        if (rb != null)
            rb.linearVelocity = - transform.up * speed;
    }

    IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(EBReloadTime);
    }
}