using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip destructionClip;
    [SerializeField][Range(0f, 1f)] float volume = 0.1f;

    public void PlayDestruction()
    {
        if (destructionClip != null)
            AudioManage.Instance.PlaySFX(destructionClip,volume);
    }
}