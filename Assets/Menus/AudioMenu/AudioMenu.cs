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
        
        // get the current values (from save file later?)
        masterVol.value = 50;
        musicVol.value = 50;
        sfxVol.value = 50;

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

    private void OnDisable()
    {
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
            //change gamewide master volume setting to 0, keep menu's setting value
            //idea for ui design: grey out the settings values to indicate muted?
        }
        else
        {
            muteBut.text = "OFF";
            //reset gamewide master volume setting to the menu's setting value
            //un-grey out the settings values?
        }
    }

    private void DecMaster()
    {
        if(masterVol.value > 0)
        {
            // change gamewide setting
            masterVol.value -= 1;
        }
    }

    private void IncMaster()
    {
        if(masterVol.value < 100)
        {
            // change gamewide setting
            masterVol.value += 1;
        }
    }

    private void DecMusic()
    {
        if(musicVol.value > 0)
        {
            // change gamewide setting
            musicVol.value -= 1;
        }
    }

    private void IncMusic()
    {
        if(musicVol.value < 100)
        {
            // change gamewide setting
            musicVol.value += 1;
        }
    }

    private void DecSFX()
    {
        if(sfxVol.value > 0)
        {
            // change gamewide setting
            sfxVol.value -= 1;
        }
    }

    private void IncSFX()
    {
        if(sfxVol.value < 100)
        {
            // change gamewide setting
            sfxVol.value += 1;
        }
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
