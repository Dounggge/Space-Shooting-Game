using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Player Sounds")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip destructionClip;
    [SerializeField][Range(0f, 1f)] float volume = 0.3f;

    public void PlayShoot()
    {
        if (shootClip != null)
            AudioManage.Instance.PlaySFX(shootClip,volume);
    }

    public void PlayDestruction()
    {
        if (destructionClip != null)
            AudioManage.Instance.PlaySFX(destructionClip, volume);
    }
}