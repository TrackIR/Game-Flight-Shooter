using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIDocument mainMenuDocument;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField, Range(0f, 1f)] private float mainMenuMusicVolume = 1f;

    public GameObject mainMenu;
    public GameObject gameModeMenu;
    public GameObject settingsMenu;
    public GameObject colorsMenu;


    private Button playButton;
    private Button settingsButton;
    private Button leaderboardButton;
    private Button exitButton;
    private Button colorsButton;

    private Label playLabel;

    private void OnEnable()
    {
        MainMenuMusicController.Play(mainMenuMusic, mainMenuMusicVolume);

        VisualElement root = mainMenuDocument.rootVisualElement;

        playButton = root.Q<Button>("playButton");
        settingsButton = root.Q<Button>("settingButton");
        leaderboardButton = root.Q<Button>("leaderboardButton");
        exitButton = root.Q<Button>("exitButton");
        colorsButton = root.Q<Button>("colorsButton");

        playLabel = root.Q<Label>("playLabel");

        playButton.clicked += PlayGame;
        settingsButton.clicked += DisplaySettingMenu;
        leaderboardButton.clicked += DisplayLeaderBoard;
        exitButton.clicked += ExitGame;

        if (colorsButton != null)
        {
            colorsButton.clicked += DisplayColorsMenu;
        }
    }

    private void OnDisable()
    {
        playButton.clicked -= PlayGame;
        settingsButton.clicked -= DisplaySettingMenu;
        leaderboardButton.clicked -= DisplayLeaderBoard;
        exitButton.clicked -= ExitGame;

        if (colorsButton != null)
        {
            colorsButton.clicked -= DisplayColorsMenu;
        }

    }

    private void PlayGame()
    {
        mainMenu.SetActive(false);
        gameModeMenu.SetActive(true);
    }

    private void DisplaySettingMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    private void DisplayColorsMenu()
    {
        mainMenu.SetActive(false);
        colorsMenu.SetActive(true);
    }

    private void DisplayLeaderBoard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    private void ExitGame()
    {
        Application.Quit();

        Debug.Log("game is exiting");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
