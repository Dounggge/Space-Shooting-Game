using System.Collections;
using UnityEngine;

public class RayBullet: Bullet
{
    [Header("Ray Settings")]
    [SerializeField] float rayWarningDuration = 0.8f;
    [SerializeField] float rayActiveDuration = 0.3f;
    [SerializeField] float rayFadeOutDuration = 0.15f;
    [SerializeField] int rayDamage = 30;
    [SerializeField] float rayLifeTime = 5f;
    [SerializeField] LayerMask rayTargetLayer;

    SpriteRenderer spriteRenderer;
    Collider2D col;

    protected override void Awake()
    {
        base.Awake();
        damage = rayDamage;
        lifeTime = rayLifeTime;
        targetLayer = rayTargetLayer;
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (col != null) col.enabled = false;
        SetAlpha(0f);
        StopAllCoroutines();
        StartCoroutine(RayRoutine());
    }

    protected override void BulletPath() { }
    protected override void OnTriggerEnter2D(Collider2D other) { }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0) return;
        if (other.TryGetComponent<Health>(out var health))
            health.TakeDamage(GetDamage());
    }

    IEnumerator RayRoutine()
    {
        float elapsed = 0f;
        while (elapsed < rayWarningDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(elapsed / rayWarningDuration));
            yield return null;
        }

        SetAlpha(1f);
        if (col != null) col.enabled = true;
        yield return new WaitForSeconds(rayActiveDuration);

        if (col != null) col.enabled = false;
        elapsed = 0f;
        while (elapsed < rayFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(1f - Mathf.Clamp01(elapsed / rayFadeOutDuration));
            yield return null;
        }

        ReturnToPool();
    }

    void SetAlpha(float alpha)
    {
        if (spriteRenderer == null) return;
        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;
    }
}