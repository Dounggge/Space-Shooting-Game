using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public static AudioManage Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip[] backgroundMusic;
    [SerializeField][Range(0, 1)] private float musicVolume = 0.5f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        PlayRandomMusic();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = volume;
        musicSource.Play();
    }

    private void PlayRandomMusic()
    {
        if (backgroundMusic != null && backgroundMusic.Length > 0)
            PlayMusic(backgroundMusic[Random.Range(0, backgroundMusic.Length)], musicVolume);
    }
}