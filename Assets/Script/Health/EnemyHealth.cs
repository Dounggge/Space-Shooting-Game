using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] protected int enemyMaxHealth = 50;
    [SerializeField] public int damageOfEnemy = 10;
    [SerializeField] float deathAnimation = 1f;

    Pools pool;
    EnemySpawner enemySpawner;
    [NonSerialized] protected Animator animator;
    protected bool isDead = false;

    public void InitEnemy(Pools enemyPool, EnemySpawner spawner)
    {
        layer = LayerMask.GetMask("Player");
        damage = damageOfEnemy;
        maxHealth = enemyMaxHealth;
        pool = enemyPool;
        enemySpawner = spawner;
        isDead = false;
        animator = GetComponent<Animator>();

        EnemyShooting shooting = GetComponent<EnemyShooting>();
        if (shooting != null)
            shooting.enabled = true;

        HealthUpdate -= UpdateAnimation;
        HealthUpdate += UpdateAnimation;
        ResetHealth(enemyMaxHealth);
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        EnemyShooting shooting = GetComponent<EnemyShooting>();
        if (shooting != null)
            shooting.enabled = false;

        HealthUpdate -= UpdateAnimation;
        PathFinding pathFinding = GetComponent<PathFinding>();
        if (pathFinding != null) pathFinding.StopMovement();
        StartCoroutine(DeathDelay());
    }

    protected virtual void UpdateAnimation(int current, int max) { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((layer.value & (1 << other.gameObject.layer)) != 0)
            if (other.TryGetComponent<IDamageDealer>(out var damageDeal))
                TakeDamage(damageDeal.GetDamage());
    }

    IEnumerator DeathDelay()
    {
        if (animator != null) animator.SetTrigger("Die");
        GetComponent<EnemySound>()?.PlayDestruction();
        yield return new WaitForSeconds(deathAnimation);
        enemySpawner?.OnEnemyRemoved();
        isDead = false;
        if (pool != null) pool.ReturnObject(gameObject);
        else gameObject.SetActive(false);
    }

    public void Kill()
    {
        if (!isDead)
            Die();
    }

}