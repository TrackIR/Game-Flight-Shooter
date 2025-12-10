using UnityEngine;
using UnityEngine.UIElements;

using System;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] UIDocument settingsMenuDocument;

    public GameObject settingsMenu;
    public GameObject mainMenu;

    private Toggle angularMomentum;
    private FloatField pitchScale;
    private Button pitchDec, pitchInc;
    private FloatField rollScale;
    private Button rollDec, rollInc;
    private FloatField yawScale;
    private Button yawDec, yawInc;
    private Button backButton;

    private void OnEnable()
    {
        VisualElement root = settingsMenuDocument.rootVisualElement;

        // get the UI element
        angularMomentum = root.Q<Toggle>("angularMomentum");

        pitchScale = root.Q<FloatField>("pitchScale");
        pitchDec = root.Q<Button>("pitchDec");
        pitchInc = root.Q<Button>("pitchInc");

        rollScale = root.Q<FloatField>("rollScale");
        rollDec = root.Q<Button>("rollDec");
        rollInc = root.Q<Button>("rollInc");

        yawScale = root.Q<FloatField>("yawScale");
        yawDec = root.Q<Button>("yawDec");
        yawInc = root.Q<Button>("yawInc");

        backButton = root.Q<Button>("backButton");
        
        // get the current values
        if(SpaceshipMovement.InputType == 1)
            angularMomentum.value = false;
        else
            angularMomentum.value = true;
        pitchScale.value = SpaceshipMovement.PitchScaler;
        rollScale.value = SpaceshipMovement.RollScaler;
        yawScale.value = SpaceshipMovement.YawScaler;

        // handler methods
        angularMomentum.RegisterValueChangedCallback(AMToggle);

        pitchDec.clicked += DecPitch;
        pitchInc.clicked += IncPitch;

        rollDec.clicked += DecRoll;
        rollInc.clicked += IncRoll;

        yawDec.clicked += DecYaw;
        yawInc.clicked += IncYaw;

        backButton.clicked += LeaveMenu;
    }

    private void OnDisable()
    {
        angularMomentum.UnregisterValueChangedCallback(AMToggle);

        pitchDec.clicked -= DecPitch;
        pitchInc.clicked -= IncPitch;

        rollDec.clicked -= DecRoll;
        rollInc.clicked -= IncRoll;

        yawDec.clicked -= DecYaw;
        yawInc.clicked -= IncYaw;

        backButton.clicked -= LeaveMenu;
    }

    private void AMToggle(ChangeEvent<bool> evt)
    {
        if(evt.newValue)
            SpaceshipMovement.InputType = 2;
        else
            SpaceshipMovement.InputType = 1;

        print(SpaceshipMovement.InputType);
    }

    private void DecPitch()
    {
        SpaceshipMovement.PitchScaler -= 0.1f;
        pitchScale.value = SpaceshipMovement.PitchScaler;
    }

    private void IncPitch()
    {
        SpaceshipMovement.PitchScaler += 0.1f;
        pitchScale.value = SpaceshipMovement.PitchScaler;
    }

    private void DecRoll()
    {
        SpaceshipMovement.RollScaler -= 0.1f;
        rollScale.value = SpaceshipMovement.RollScaler;
    }

    private void IncRoll()
    {
        SpaceshipMovement.RollScaler += 0.1f;
        rollScale.value = SpaceshipMovement.RollScaler;
    }

    private void DecYaw()
    {
        SpaceshipMovement.YawScaler -= 0.1f;
        yawScale.value = SpaceshipMovement.YawScaler;
    }

    private void IncYaw()
    {
        SpaceshipMovement.YawScaler += 0.1f;
        yawScale.value = SpaceshipMovement.YawScaler;
    }

    private void LeaveMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
