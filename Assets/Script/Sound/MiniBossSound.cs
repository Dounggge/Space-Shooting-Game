using UnityEngine;

public class MiniBossSound : MonoBehaviour
{
    [Header("Ray")]
    [SerializeField] private AudioClip rayClip;

    [Header("Laser")]
    [SerializeField] private AudioClip laserClip;

    [Header("Destruction")]
    [SerializeField] private AudioClip destructionClip;

    [Header("Volume")]
    [SerializeField][Range(0f, 1f)] float volume = 0.2f;

    public void PlayRay() => Play(rayClip);
    public void PlayLaser() => Play(laserClip);
    public void PlayDestruction() => Play(destructionClip);

    private void Play(AudioClip clip)
    {
        if (clip != null)
            AudioManage.Instance.PlaySFX(clip,volume);
    }
}