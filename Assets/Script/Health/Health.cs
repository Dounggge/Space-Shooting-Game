using UnityEngine;
using System;

public abstract class Health : MonoBehaviour
{
    protected int maxHealth;
    protected int currentHealth;

    public event Action<int, int> HealthUpdate; //current health, max health


    protected virtual void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        HealthUpdate?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    protected abstract void Die();

    protected void ResetHealth(int Health)
    {
        currentHealth = Health;
        HealthUpdate?.Invoke(currentHealth, maxHealth);
    }

}
/*
protected virtual void Heal(int amount)
{
   if (amount <= 0) return;
   currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
   HealthUpdate?.Invoke(currentHealth, maxHealth);
}*/ //Update sau prototype
