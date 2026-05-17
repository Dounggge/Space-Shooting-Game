using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    [SerializeField] int playerMaxHealth = 100;
    [SerializeField] int damageOfPlayer = 10;
    [SerializeField] Sprite[] spriteUpdate;

    SpriteRenderer spriteRenderer;


    void Awake()
    {
        if (spriteUpdate == null || spriteUpdate.Length == 0)
        {
            Debug.LogError("PlayerHealth: Sprite array is not assigned or empty!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteUpdate[0];

        ResetHealth(playerMaxHealth);

        HealthUpdate += null;
        HealthUpdate += OnHealthChanged;
    }



    private void OnHealthChanged(int current, int max)
    {
        if (spriteUpdate == null || spriteUpdate.Length == 0 || spriteRenderer == null)
            return;

        float healthPercent = (float)current / max;
        while (healthPercent <= 0f || healthPercent > 1f)
        {
            ResetHealth(playerMaxHealth);
            spriteRenderer.sprite = spriteUpdate[0];
        }
        if (healthPercent <= 0.75f && healthPercent > 0.5f)
            spriteRenderer.sprite = spriteUpdate[1];
        else if (healthPercent <= 0.5f && healthPercent > 0.25f)
            spriteRenderer.sprite = spriteUpdate[2];
        else if(healthPercent <= 0.25f && healthPercent > 0f)
            spriteRenderer.sprite = spriteUpdate[3];
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
        if(other.TryGetComponent<IDamageDealer>(out var dealer))
        {
            TakeDamage(damageOfPlayer);
        }
    }
}