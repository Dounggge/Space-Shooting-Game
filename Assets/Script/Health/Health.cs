using UnityEngine;
using System;

public abstract class Health : MonoBehaviour, IDamageDealer
{
    protected int maxHealth;
    protected int currentHealth;
    protected int damage;
    protected LayerMask layer;


    public event Action<int, int> HealthUpdate; //current health, max health

    public int GetDamage() => damage;
    public LayerMask LayerDamage => layer;

    public void TakeDamage(int damage)
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

//Bat ke doi tuong nao co Healh thi deu co kha nang gay damage va bi damage
//Nhung khong nen dung interface vao abstract class vi no se gay coupling va phuc tap hoa code