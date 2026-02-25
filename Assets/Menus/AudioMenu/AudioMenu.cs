using UnityEngine;
using UnityEngine.UIElements;

using System;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] UIDocument audioMenuDocument;

    public GameObject audioMenu;
    public GameObject settingsMenu;
    public GameObject mainMenu;

    private Button muteBut;
    private IntegerField masterVol;
    private Button masterDec, masterInc;
    private IntegerField musicVol;
    private Button musicDec, musicInc;
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

        musicVol = root.Q<IntegerField>("musicVol");
        musicDec = root.Q<Button>("musicDec");
        musicInc = root.Q<Button>("musicInc");

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

        musicDec.clicked += DecMusic;
        musicInc.clicked += IncMusic;

        sfxDec.clicked += DecSFX;
        sfxInc.clicked += IncSFX;

        backButton.clicked += LeaveMenu;
        settingsButton.clicked += ToSettingsMenu;
    }

    // Gets the PlayerPrefs for the audio settings
    private void GetAudioPrefs()
    {
        if (PlayerPrefs.HasKey("muteVol"))
            muteBut.text = PlayerPrefs.GetInt("muteVol") == 1 ? "ON" : "OFF";
        else
            muteBut.text = "OFF";

        if (PlayerPrefs.HasKey("mstrVol"))
            masterVol.value = PlayerPrefs.GetInt("mstrVol");
        else
            masterVol.value = 50;

        if (PlayerPrefs.HasKey("mscVol"))
            musicVol.value = PlayerPrefs.GetInt("mscVol");
        else
            musicVol.value = 50;

        if (PlayerPrefs.HasKey("sfxVol"))
            sfxVol.value = PlayerPrefs.GetInt("sfxVol");
        else
            sfxVol.value = 50;
    }
    private void OnDisable()
    {
        //save the changes
        PlayerPrefs.SetInt("muteVol", muteBut.text == "ON" ? 1 : 0);
        PlayerPrefs.SetInt("mstrVol", masterVol.value);
        PlayerPrefs.SetInt("mscVol", musicVol.value);
        PlayerPrefs.SetInt("sfxVol", sfxVol.value);
        PlayerPrefs.Save();

        muteBut.clicked -= muteToggle;

        masterDec.clicked -= DecMaster;
        masterInc.clicked -= IncMaster;

        musicDec.clicked -= DecMusic;
        musicInc.clicked -= IncMusic;

        sfxDec.clicked -= DecSFX;
        sfxInc.clicked -= IncSFX;

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

    private void DecMusic()
    {
        if(musicVol.value > 0)
            musicVol.value -= 1;
    }

    private void IncMusic()
    {
        if(musicVol.value < 100)
            musicVol.value += 1;
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
}
