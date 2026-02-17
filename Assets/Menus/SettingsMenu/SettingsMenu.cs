using UnityEngine;
using UnityEngine.UIElements;

using System;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] UIDocument settingsMenuDocument;

    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject audioMenu;

    private Button angularMomentum;
    private FloatField pitchScale;
    private Button pitchDec, pitchInc;
    private FloatField rollScale;
    private Button rollDec, rollInc;
    private FloatField yawScale;
    private Button yawDec, yawInc;
    private Button fullScreen;
    private Button pov;
    private Button backButton;
    private Button audioButton;

    private void OnEnable()
    {
        VisualElement root = settingsMenuDocument.rootVisualElement;

        // get the UI element
        angularMomentum = root.Q<Button>("angularMomentum");

        pitchScale = root.Q<FloatField>("pitchScale");
        pitchDec = root.Q<Button>("pitchDec");
        pitchInc = root.Q<Button>("pitchInc");

        rollScale = root.Q<FloatField>("rollScale");
        rollDec = root.Q<Button>("rollDec");
        rollInc = root.Q<Button>("rollInc");

        yawScale = root.Q<FloatField>("yawScale");
        yawDec = root.Q<Button>("yawDec");
        yawInc = root.Q<Button>("yawInc");

        fullScreen = root.Q<Button>("fullScreen");
        pov = root.Q<Button>("pointofView");

        backButton = root.Q<Button>("backButton");
        audioButton = root.Q<Button>("audioButton");
        
        // get the current values
        GetSettingsPrefs();

        // handler methods
        angularMomentum.clicked += AMToggle;

        pitchDec.clicked += DecPitch;
        pitchInc.clicked += IncPitch;

        rollDec.clicked += DecRoll;
        rollInc.clicked += IncRoll;

        yawDec.clicked += DecYaw;
        yawInc.clicked += IncYaw;

        fullScreen.clicked += FSToggle;
        pov.clicked += PoVToggle;

        backButton.clicked += LeaveMenu;
        audioButton.clicked += ToAudioMenu;
    }

    private void GetSettingsPrefs()
    {
        if (PlayerPrefs.HasKey("angMoment"))
        {
            if (PlayerPrefs.GetInt("angMoment") == 1)
                angularMomentum.text = "ON";
        }
        else
            angularMomentum.text = "OFF";
        
        if (PlayerPrefs.HasKey("ptchScl"))
            pitchScale.value = PlayerPrefs.GetFloat("ptchScl");
        else
            pitchScale.value = 1.0f;     // default values in SpaceshipMovement.cs

        if (PlayerPrefs.HasKey("rollScl"))
            rollScale.value = PlayerPrefs.GetFloat("rollScl");
        else
            rollScale.value = 1.1f;

        if (PlayerPrefs.HasKey("yawScl"))
            yawScale.value = PlayerPrefs.GetFloat("yawScl");
        else
            yawScale.value = 1.5f;

        if (PlayerPrefs.HasKey("fullScrn"))
        {
            if (PlayerPrefs.GetInt("fullScrn") == 1)
            {
                fullScreen.text = "ON";
                Screen.fullScreen = true;
            }
        }
        else
        {
            fullScreen.text = "OFF";
            Screen.fullScreen = false;
        }

        if (PlayerPrefs.HasKey("povFrst"))
        {
            if (PlayerPrefs.GetInt("povFrst") == 1)
            {
                pov.text = "First";
                CameraSwitcher.isFirstPerson = true;
            }
        }
        else
        {
            pov.text = "Third";
            CameraSwitcher.isFirstPerson = false;
        }
        
    }
    private void OnDisable()
    {
        //save playerPrefs
        PlayerPrefs.SetInt("angMoment", angularMomentum.text == "ON" ? 1 : 0);
        PlayerPrefs.SetFloat("ptchScl", pitchScale.value);
        PlayerPrefs.SetFloat("rollScl", rollScale.value);
        PlayerPrefs.SetFloat("yawScl", yawScale.value);
        PlayerPrefs.SetInt("fullScrn", fullScreen.text == "ON" ? 1 : 0);
        PlayerPrefs.SetInt("povFrst", pov.text == "First" ? 1 : 0);
        PlayerPrefs.Save();

        angularMomentum.clicked += AMToggle;

        pitchDec.clicked -= DecPitch;
        pitchInc.clicked -= IncPitch;

        rollDec.clicked -= DecRoll;
        rollInc.clicked -= IncRoll;

        yawDec.clicked -= DecYaw;
        yawInc.clicked -= IncYaw;

        fullScreen.clicked -= FSToggle;
        pov.clicked -= PoVToggle;

        backButton.clicked -= LeaveMenu;
        audioButton.clicked -= ToAudioMenu;
    }

    private void AMToggle()
    {
        if(angularMomentum.text == "OFF")
            angularMomentum.text = "ON";
        else
            angularMomentum.text = "OFF";
    }

    private void DecPitch()
    {
        if(SpaceshipMovement.PitchScaler > 0.0f)
        {
            pitchScale.value -= 0.1f;
            // SpaceshipMovement.PitchScaler -= 0.1f;
            // pitchScale.value = SpaceshipMovement.PitchScaler;
        }
    }

    private void IncPitch()
    {
        if(SpaceshipMovement.PitchScaler < 3.0f)
        {
            pitchScale.value += 0.1f;
            // SpaceshipMovement.PitchScaler += 0.1f;
            // pitchScale.value = SpaceshipMovement.PitchScaler;
        }
    }

    private void DecRoll()
    {
        if(SpaceshipMovement.RollScaler > 0.0f)
        {
            rollScale.value -= 0.1f;
            // SpaceshipMovement.RollScaler -= 0.1f;
            // rollScale.value = SpaceshipMovement.RollScaler;
        }
    }

    private void IncRoll()
    {
        if(SpaceshipMovement.RollScaler < 3.0f)
        {
            rollScale.value += 0.1f;
            // SpaceshipMovement.RollScaler += 0.1f;
            // rollScale.value = SpaceshipMovement.RollScaler;
        }
    }

    private void DecYaw()
    {
        if(SpaceshipMovement.YawScaler > 0.0f)
        {
            yawScale.value -= 0.1f;
            // SpaceshipMovement.YawScaler -= 0.1f;
            // yawScale.value = SpaceshipMovement.YawScaler;
        }
    }

    private void IncYaw()
    {
        if(SpaceshipMovement.YawScaler < 3.0f)
        {
            yawScale.value += 0.1f;
            // SpaceshipMovement.YawScaler += 0.1f;
            // yawScale.value = SpaceshipMovement.YawScaler;
        }
    }

    private void FSToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;

        if(fullScreen.text == "OFF")
            fullScreen.text = "ON";
        else
            fullScreen.text = "OFF";
    }

    private void PoVToggle()
    {
        if(pov.text == "First")
        {
            pov.text = "Third";
            // CameraSwitcher.isFirstPerson = false;
        }
        else
        {
            pov.text = "First";
            // CameraSwitcher.isFirstPerson = true;
        }
    }

    private void LeaveMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void ToAudioMenu()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }
}
