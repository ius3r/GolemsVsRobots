using UnityEngine;

public sealed class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool loopMusic = true;

    public AudioSource MusicSource => musicSource;
    public AudioSource SfxSource => sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource != null)
        {
            musicSource.loop = loopMusic;
        }
    }

    private void Start()
    {
        if (playOnAwake)
        {
            PlayMusic(musicTrack);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void PlaySfxOneShot(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }
}
