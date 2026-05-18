using System.Collections;
using UnityEngine;

 public class EnemyHealth : Health
 { 
    [SerializeField] int enemyMaxHealth = 50;
    [SerializeField] public int damageOfEnemy = 10;
    Pools pool;

    [SerializeField] float deathAnimation = 1f; 
    Animator animator;

    public void InitEnemy(Pools enemyPool)
    {
        maxHealth = enemyMaxHealth;
        pool = enemyPool;
        animator = GetComponent<Animator>();

        HealthUpdate += UpdateAnimation;

        ResetHealth(enemyMaxHealth);
    }

    protected override void Die()
    {
        HealthUpdate -= UpdateAnimation;

        PathFinding pathFinding = GetComponent<PathFinding>();
        pathFinding.StopMovement();

        StartCoroutine(DeathDelay());
    }

    void UpdateAnimation(int current, int max)
    {
        if (animator == null) return;
        PathFinding path = GetComponent<PathFinding>();

        if (current <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    //Debug.Log("Triggered by " + other.name);
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerDamage = other.GetComponent<PlayerHealth>();
            if (playerDamage != null)
            {
                TakeDamage(playerDamage.damageOfPlayer);
            }
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathAnimation);

        if (pool != null)
            pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }

}