using UnityEngine;
using UnityEngine.UIElements;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] UIDocument audioMenuDocument;

    public GameObject audioMenu;
    public GameObject settingsMenu;
    public GameObject mainMenu;

    private Button muteBut;
    private IntegerField masterVol;
    private Button masterDec, masterInc;
    private IntegerField mainMenuMusicVol;
    private Button mainMenuMusicDec, mainMenuMusicInc;
    private IntegerField gameplayMusicVol;
    private Button gameplayMusicDec, gameplayMusicInc;
    private IntegerField sfxVol;
    private Button sfxDec, sfxInc;
    private Button backButton;
    private Button settingsButton;

    private void OnEnable()
    {
        VisualElement root = audioMenuDocument.rootVisualElement;

        // get the UI element
        muteBut = root.Q<Button>("mute");

        masterVol = root.Q<IntegerField>("masterVol");
        masterInc = root.Q<Button>("masterInc");
        masterDec = root.Q<Button>("masterDec");

        mainMenuMusicVol = root.Q<IntegerField>("mainMenuMusicVol");
        mainMenuMusicDec = root.Q<Button>("mainMenuMusicDec");
        mainMenuMusicInc = root.Q<Button>("mainMenuMusicInc");

        gameplayMusicVol = root.Q<IntegerField>("gameplayMusicVol");
        gameplayMusicDec = root.Q<Button>("gameplayMusicDec");
        gameplayMusicInc = root.Q<Button>("gameplayMusicInc");

        sfxVol = root.Q<IntegerField>("sfxVol");
        sfxDec = root.Q<Button>("sfxDec");
        sfxInc = root.Q<Button>("sfxInc");

        backButton = root.Q<Button>("backButton");
        settingsButton = root.Q<Button>("settingsButton");
        
        // get the current values
        GetAudioPrefs();

        // handler methods
        muteBut.clicked += muteToggle;

        masterDec.clicked += DecMaster;
        masterInc.clicked += IncMaster;

        mainMenuMusicDec.clicked += DecMainMenuMusic;
        mainMenuMusicInc.clicked += IncMainMenuMusic;

        gameplayMusicDec.clicked += DecGameplayMusic;
        gameplayMusicInc.clicked += IncGameplayMusic;

        sfxDec.clicked += DecSFX;
        sfxInc.clicked += IncSFX;

        masterVol.RegisterValueChangedCallback(OnMasterVolumeChanged);
        mainMenuMusicVol.RegisterValueChangedCallback(OnMainMenuMusicVolumeChanged);
        gameplayMusicVol.RegisterValueChangedCallback(OnGameplayMusicVolumeChanged);
        sfxVol.RegisterValueChangedCallback(OnSfxVolumeChanged);

        backButton.clicked += LeaveMenu;
        settingsButton.clicked += ToSettingsMenu;
    }

    // Gets the PlayerPrefs for the audio settings
    private void GetAudioPrefs()
    {
        muteBut.text = AudioSettingsPrefs.IsMuted() ? "ON" : "OFF";
        masterVol.value = AudioSettingsPrefs.GetSavedVolume(AudioSettingsPrefs.MasterVolumeKey);
        mainMenuMusicVol.value = AudioSettingsPrefs.GetSavedVolume(
            AudioSettingsPrefs.MainMenuMusicVolumeKey,
            fallbackToLegacyMusic: true);
        gameplayMusicVol.value = AudioSettingsPrefs.GetSavedVolume(
            AudioSettingsPrefs.GameplayMusicVolumeKey,
            fallbackToLegacyMusic: true);
        sfxVol.value = AudioSettingsPrefs.GetSavedVolume(AudioSettingsPrefs.SfxVolumeKey);
    }

    private void OnDisable()
    {
        SaveAudioPrefs();

        muteBut.clicked -= muteToggle;

        masterDec.clicked -= DecMaster;
        masterInc.clicked -= IncMaster;

        mainMenuMusicDec.clicked -= DecMainMenuMusic;
        mainMenuMusicInc.clicked -= IncMainMenuMusic;

        gameplayMusicDec.clicked -= DecGameplayMusic;
        gameplayMusicInc.clicked -= IncGameplayMusic;

        sfxDec.clicked -= DecSFX;
        sfxInc.clicked -= IncSFX;

        masterVol.UnregisterValueChangedCallback(OnMasterVolumeChanged);
        mainMenuMusicVol.UnregisterValueChangedCallback(OnMainMenuMusicVolumeChanged);
        gameplayMusicVol.UnregisterValueChangedCallback(OnGameplayMusicVolumeChanged);
        sfxVol.UnregisterValueChangedCallback(OnSfxVolumeChanged);

        backButton.clicked -= LeaveMenu;
        settingsButton.clicked -= ToSettingsMenu;
    }

    private void muteToggle()
    {
        if(muteBut.text == "OFF")
        {
            muteBut.text = "ON";

            //idea for ui design: grey out the vol settings values to indicate muted?
        }
        else
        {
            muteBut.text = "OFF";

            //un-grey out the vol settings values?
        }

        SaveAudioPrefs();
    }

    private void DecMaster()
    {
        if(masterVol.value > 0)
            masterVol.value -= 1;
    }

    private void IncMaster()
    {
        if(masterVol.value < 100)
            masterVol.value += 1;
    }

    private void DecMainMenuMusic()
    {
        if(mainMenuMusicVol.value > 0)
            mainMenuMusicVol.value -= 1;
    }

    private void IncMainMenuMusic()
    {
        if(mainMenuMusicVol.value < 100)
            mainMenuMusicVol.value += 1;
    }

    private void DecGameplayMusic()
    {
        if(gameplayMusicVol.value > 0)
            gameplayMusicVol.value -= 1;
    }

    private void IncGameplayMusic()
    {
        if(gameplayMusicVol.value < 100)
            gameplayMusicVol.value += 1;
    }

    private void DecSFX()
    {
        if(sfxVol.value > 0)
            sfxVol.value -= 1;
    }

    private void IncSFX()
    {
        if(sfxVol.value < 100)
            sfxVol.value += 1;
    }

    private void LeaveMenu()
    {
        audioMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void ToSettingsMenu()
    {
        audioMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    private void OnMasterVolumeChanged(ChangeEvent<int> evt)
    {
        masterVol.SetValueWithoutNotify(ClampVolume(evt.newValue));
        SaveAudioPrefs();
    }

    private void OnMainMenuMusicVolumeChanged(ChangeEvent<int> evt)
    {
        mainMenuMusicVol.SetValueWithoutNotify(ClampVolume(evt.newValue));
        SaveAudioPrefs();
    }

    private void OnGameplayMusicVolumeChanged(ChangeEvent<int> evt)
    {
        gameplayMusicVol.SetValueWithoutNotify(ClampVolume(evt.newValue));
        SaveAudioPrefs();
    }

    private void OnSfxVolumeChanged(ChangeEvent<int> evt)
    {
        sfxVol.SetValueWithoutNotify(ClampVolume(evt.newValue));
        SaveAudioPrefs();
    }

    private void SaveAudioPrefs()
    {
        if (muteBut == null)
        {
            return;
        }

        PlayerPrefs.SetInt(AudioSettingsPrefs.MuteKey, muteBut.text == "ON" ? 1 : 0);
        PlayerPrefs.SetInt(AudioSettingsPrefs.MasterVolumeKey, ClampVolume(masterVol.value));
        PlayerPrefs.SetInt(AudioSettingsPrefs.MainMenuMusicVolumeKey, ClampVolume(mainMenuMusicVol.value));
        PlayerPrefs.SetInt(AudioSettingsPrefs.GameplayMusicVolumeKey, ClampVolume(gameplayMusicVol.value));
        PlayerPrefs.SetInt(AudioSettingsPrefs.SfxVolumeKey, ClampVolume(sfxVol.value));
        PlayerPrefs.Save();
    }

    private static int ClampVolume(int value)
    {
        return Mathf.Clamp(value, 0, 100);
    }
}
