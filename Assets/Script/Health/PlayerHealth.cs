using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    [SerializeField] int playerMaxHealth = 100;
    [SerializeField] int hitsPerAnimationUpdate = 2;   // Cập nhật animation máu sau mỗi ? đòn
    [SerializeField] Animator animator;                // Kéo thả Animator của Player vào đây
    [SerializeField] float damage = 10f;                        // Sát thương mặc định khi va chạm với enemy

    private int hitCounter;
    private int lastAnimatedHealth;                    // Máu ở lần cập nhật animation cuối

    // Tham số trong Animator (có thể đổi tên nếu cần)
    private readonly string healthRatioParam = "HealthRatio";
    private readonly string hurtTriggerParam = "Hurt";

    void Awake()
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        lastAnimatedHealth = currentHealth;
        hitCounter = 0;

        // Đăng ký sự kiện để nhận thông báo mỗi khi máu thay đổi
        HealthUpdate += OnHealthChanged;

        // Nếu chưa gán animator từ Inspector, tự tìm
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Gọi method này từ bên ngoài (OnTriggerEnter2D, va chạm đạn, ...) để gây sát thương
    public void ApplyDamage(int damage)
    {
        if (damage <= 0) return;

        // Trừ máu thực (gọi base.TakeDamage)
        TakeDamage(damage);

        // Tăng đếm số đòn
        hitCounter++;

        // animation bị đánh (phản ứng tức thì) – tuỳ chọn
        if (animator != null)
            animator.SetTrigger(hurtTriggerParam);
    }

    // Sự kiện kích hoạt mỗi khi máu thay đổi (do HealthUpdate?.Invoke)
    private void OnHealthChanged(int current, int max)
    {
        // Nếu đã đủ số đòn để cập nhật animation trạng thái máu
        if (hitCounter >= hitsPerAnimationUpdate)
        {
            hitCounter = 0;
            UpdateHealthAnimation(current);
            lastAnimatedHealth = current;
        }
        // Có thể thêm logic khác nếu cần hiển thị số máu text (không liên quan)
    }

    // Cập nhật tham số HealthRatio cho Animator
    private void UpdateHealthAnimation(int currentHealth)
    {
        if (animator == null) return;

        float ratio = (float)currentHealth / maxHealth;
        animator.SetFloat(healthRatioParam, ratio);
    }

    // Khi player chết
    protected override void Die()
    {
        // Có thể chơi animation chết ở đây, sau đó load scene
        // Ví dụ:
        // animator.SetTrigger("Die");
        // Invoke("LoadGameOver", 1f); // delay 1 giây
        LoadGameOverScreen();
    }

    void LoadGameOverScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }

    // Xử lý va chạm trigger với enemy (gắn script này lên Player)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Giả sử enemy có tag "Enemy" và gây sát thương cố định hoặc lấy từ component
        if (other.CompareTag("Enemy"))
        {
            int damage = 10; // Mặc định hoặc lấy từ other.GetComponent<Enemy>().damage
            ApplyDamage(damage);
        }
    }

    // Hàm Hit() có thể dùng cho các loại sát thương khác (đạn, gai...)
    public void Hit()
    {
        // Gọi từ bên ngoài, ví dụ: PlayerHealth.Hit()
        // Có thể truyền tham số damage nếu cần
        ApplyDamage(10);
    }
}