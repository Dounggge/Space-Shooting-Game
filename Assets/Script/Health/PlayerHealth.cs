using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    [Header("Stats")]
    [SerializeField] private int playerMaxHealth = 100;
    [SerializeField] private int damageOfPlayer = 20;

    [Header("Visual")]
    [SerializeField] private Sprite[] spriteUpdate;

    [Header("Death")]
    [SerializeField] private float deathDelay = 1f;

    private SpriteRenderer spriteRenderer;
    private PlayerSound playerSound;

    private static int persistentHealth = -1;
    private bool isDead = false;
    private int previousHealth;

    public static void ResetPersistentHealth() => persistentHealth = -1;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSound = GetComponent<PlayerSound>();

        layer = LayerMask.GetMask("Enemy");
        damage = damageOfPlayer;
        maxHealth = playerMaxHealth;

        if (persistentHealth == -1)
            persistentHealth = playerMaxHealth;
        currentHealth = persistentHealth;
        ResetHealth(currentHealth);

        previousHealth = currentHealth;
        HealthUpdate += OnHealthChanged;
    }

    private void OnHealthChanged(int current, int max)
    {
        if (current < previousHealth)
        {
            CameraShake.Instance?.Shake();
        }
        previousHealth = current;
        persistentHealth = current;
        UpdateSprite(current, max);
    }

    private void UpdateSprite(int current, int max)
    {
        if (spriteUpdate == null || spriteUpdate.Length < 4 || spriteRenderer == null) return;
        if (current <= 0) return; 

        float percent = (float)current / max;
        if (percent > 0.75f)
            spriteRenderer.sprite = spriteUpdate[0];
        else if (percent > 0.5f)
            spriteRenderer.sprite = spriteUpdate[1];
        else if (percent > 0.25f)
            spriteRenderer.sprite = spriteUpdate[2];
        else
            spriteRenderer.sprite = spriteUpdate[3];
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        playerSound?.PlayDestruction();

        persistentHealth = -1;

        StartCoroutine(LoadDie());
    }

    private IEnumerator LoadDie()
    {
        yield return new WaitForSeconds(deathDelay);
        if (SceneManage.instance != null)
            SceneManage.instance.LoadMenuScene();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if ((layer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent<IDamageDealer>(out var dealer))
                TakeDamage(dealer.GetDamage());
        }
    }
}