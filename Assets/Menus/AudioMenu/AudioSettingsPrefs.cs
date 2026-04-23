using UnityEngine;

public static class AudioSettingsPrefs
{
    public const string MuteKey = "muteVol";
    public const string MasterVolumeKey = "mstrVol";
    public const string LegacyMusicVolumeKey = "mscVol";
    public const string MainMenuMusicVolumeKey = "mainMenuMscVol";
    public const string GameplayMusicVolumeKey = "gameplayMscVol";
    public const string SfxVolumeKey = "sfxVol";
    public const int DefaultVolume = 50;

    public static int GetSavedVolume(string key, bool fallbackToLegacyMusic = false)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return Mathf.Clamp(PlayerPrefs.GetInt(key), 0, 100);
        }

        if (fallbackToLegacyMusic && PlayerPrefs.HasKey(LegacyMusicVolumeKey))
        {
            return Mathf.Clamp(PlayerPrefs.GetInt(LegacyMusicVolumeKey), 0, 100);
        }

        return DefaultVolume;
    }

    public static float GetSavedVolumeNormalized(string key, bool fallbackToLegacyMusic = false)
    {
        return GetSavedVolume(key, fallbackToLegacyMusic) / 100f;
    }

    public static bool IsMuted()
    {
        return PlayerPrefs.GetInt(MuteKey, 0) == 1;
    }
}
