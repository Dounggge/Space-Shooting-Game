using UnityEngine;
using System;

public abstract class Health : MonoBehaviour
{
    protected int maxHealth;
    protected int currentHealth;

    public event Action<int, int> HealthUpdate; //current health, max health


    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        HealthUpdate?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    public virtual void Heal(int amount)
    {
        if (amount <= 0) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        HealthUpdate?.Invoke(currentHealth, maxHealth);
    }

    protected abstract void Die();

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        HealthUpdate?.Invoke(currentHealth, maxHealth);
    }

}
