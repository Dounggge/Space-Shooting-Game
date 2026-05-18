using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    [SerializeField] int playerMaxHealth = 100;
    [SerializeField] public int damageOfPlayer = 20;
    [SerializeField] Sprite[] spriteUpdate;

    SpriteRenderer spriteRenderer;
    EnemyHealth enemyDamage;

    void Awake()
    {
        if (spriteUpdate == null || spriteUpdate.Length == 0)
        {
            Debug.LogError("PlayerHealth: Sprite array is not assigned or empty!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteUpdate[0];

        maxHealth = playerMaxHealth;
        ResetHealth(playerMaxHealth);

        HealthUpdate += OnHealthChanged;
    }



    private void OnHealthChanged(int current, int max)
    {
        if (spriteUpdate == null || spriteUpdate.Length == 0 || spriteRenderer == null)
            return;

        if (max <= 0)
            return;

        float healthPercent = (float)current / max;

        if (current <= 0)
        {
            return;
        }
        else if (healthPercent > 0.75f)
        {
            spriteRenderer.sprite = spriteUpdate[0]; // full
        }
        else if (healthPercent > 0.5f)
        {
            spriteRenderer.sprite = spriteUpdate[1];
        }
        else if (healthPercent > 0.25f)
        {
            spriteRenderer.sprite = spriteUpdate[2];
        }
        else // 0 < healthPercent <= 0.25
        {
            spriteRenderer.sprite = spriteUpdate[3];
        }
    }


    protected override void Die()
    {
        //particle effect
        //StartCoroutine(LoadGameOverScreen());
        Destroy(gameObject);
    }

    /*IEnumerator LoadParticle()
    {
        //yield return new WaitForSeconds(2f);
        //SceneManager.Instance.GameOverScene();
    }*/

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("triggered by " + other.name);
        if (other.CompareTag("Enemy"))
        {
            enemyDamage = other.GetComponent<EnemyHealth>();
            TakeDamage(enemyDamage.damageOfEnemy);
        }
    }
}