using System;
using UnityEngine;
using System.Collections;

public class BossHealth : EnemyHealth
{
    public event Action OnHalfHealth;

    [Header("Boss References")]
    [SerializeField] private MiniBossSound bossSound;
    [SerializeField] private MinibossController controller;

    private bool halfTriggered = false;

    protected void Awake()
    {
        if (bossSound == null) bossSound = GetComponent<MiniBossSound>();
        if (controller == null) controller = GetComponent<MinibossController>();

        animator = GetComponent<Animator>();
        layer = LayerMask.GetMask("Player");
        damage = damageOfEnemy;
        maxHealth = enemyMaxHealth;
        isDead = false;

        HealthUpdate -= UpdateAnimation;
        HealthUpdate += UpdateAnimation;
        ResetHealth(enemyMaxHealth);
    }

    protected override void UpdateAnimation(int current, int max)
    {
        if (!halfTriggered && current <= max / 2)
        {
            halfTriggered = true;
            animator?.SetTrigger("Phase2");
            OnHalfHealth?.Invoke();
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        bossSound?.PlayDestruction();

        controller?.OnBossDead();

        if (animator != null)
            animator.SetTrigger("Die");

        StartCoroutine(LoadMenuAfterDelay(1f));
    }

    private IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (SceneManage.instance != null)
            SceneManage.instance.LoadMenuScene();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}