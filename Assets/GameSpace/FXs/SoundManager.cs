using UnityEngine;

public enum SoundType
{
    LASER,
    DAMAGE,
    EXPLOSION,
    DEATH
}

[System.Serializable]
public class SoundEntry
{
    public SoundType sound;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundEntry[] sounds;

    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound)
    {
        if (instance == null || instance.audioSource == null) return;

        foreach (SoundEntry entry in instance.sounds)
        {
            if (entry.sound == sound && entry.clip != null)
            {
                instance.audioSource.PlayOneShot(entry.clip, entry.volume);
                return;
            }
        }
    }

    public static void PlaySound(SoundType sound, float volumeMultiplier)
    {
        if (instance == null || instance.audioSource == null) return;

        foreach (SoundEntry entry in instance.sounds)
        {
            if (entry.sound == sound && entry.clip != null)
            {
                float finalVolume = Mathf.Clamp01(entry.volume * volumeMultiplier);
                instance.audioSource.PlayOneShot(entry.clip, finalVolume);
                return;
            }
        }
    }
}