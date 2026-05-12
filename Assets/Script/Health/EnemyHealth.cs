using UnityEngine;

public class EnemyHealth : Health, IDamageDealer
{
    [SerializeField] int enemyMaxHealth = 50;
    Pools pool;

    public void Init(Pools enemyPool)
    {
        pool = enemyPool;
        maxHealth = enemyMaxHealth;

        // Xóa event cũ — bắt buộc vì dùng Pool
        HealthUpdate += null;
        HealthUpdate += UpdateAnimation;

        ResetHealth();
    }

    // Abstract method — enemy chết theo cách riêng
    protected override void Die()
    {
        pool.ReturnObject(gameObject);
    }

    void UpdateAnimation(int current, int max) { }

    public void Hit()
    {
        // Enemy có thể phản đòn hoặc có hiệu ứng khi bị hit
    }

}