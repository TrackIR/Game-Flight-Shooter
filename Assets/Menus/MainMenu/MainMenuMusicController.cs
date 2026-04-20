using UnityEngine;

public class MainMenuMusicController : MonoBehaviour
{
    private const string MusicObjectName = "MainMenuMusicController";

    private static MainMenuMusicController instance;

    private AudioSource audioSource;
    private float clipVolume = 1f;

    public static void Play(AudioClip clip, float volumeMultiplier)
    {
        if (clip == null)
        {
            return;
        }

        EnsureInstance();
        instance.SetClip(clip, volumeMultiplier);
    }

    private static void EnsureInstance()
    {
        if (instance != null)
        {
            return;
        }

        GameObject musicObject = new GameObject(MusicObjectName);
        instance = musicObject.AddComponent<MainMenuMusicController>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep the music alive while menu panels toggle on and off in the same scene.
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f;

        RefreshVolume();
    }

    private void Update()
    {
        RefreshVolume();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void SetClip(AudioClip clip, float volumeMultiplier)
    {
        clipVolume = Mathf.Clamp01(volumeMultiplier);

        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
        }

        RefreshVolume();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void RefreshVolume()
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.mute = AudioSettingsPrefs.IsMuted();
        audioSource.volume = AudioSettingsPrefs.GetSavedVolumeNormalized(AudioSettingsPrefs.MasterVolumeKey)
            * AudioSettingsPrefs.GetSavedVolumeNormalized(
                AudioSettingsPrefs.MainMenuMusicVolumeKey,
                fallbackToLegacyMusic: true)
            * clipVolume;
    }
}
