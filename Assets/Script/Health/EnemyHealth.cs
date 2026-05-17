using UnityEngine;

public class EnemyHealth : Health
{ 
    [SerializeField] int enemyMaxHealth = 50;
    [SerializeField] int damageOfEnemy = 10;
    Pools pool;

    public void Init(Pools enemyPool)
    {
        pool = enemyPool;

        HealthUpdate += null;
        HealthUpdate += UpdateAnimation;

        ResetHealth(enemyMaxHealth);
    }

    protected override void Die()
    {
        pool.ReturnObject(gameObject);
    }

    void UpdateAnimation(int current, int max)
    { }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(damageOfEnemy);
        }
    }


}