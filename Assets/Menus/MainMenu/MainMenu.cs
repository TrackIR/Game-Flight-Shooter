using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIDocument mainMenuDocument;

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

        if (PlayerPrefs.HasKey("ts"))
        {
            if (PlayerPrefs.GetInt("ts") == 1)
                playLabel.text = "Play";
        }
        else
            playLabel.text = "Game Modes";
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
        if (!PlayerPrefs.HasKey("ts"))
        {
            if (PlayerPrefs.GetInt("ts") == 1)
                SceneManager.LoadScene("GameScene");
            else
            {
                mainMenu.SetActive(false);
                gameModeMenu.SetActive(true);
            }
        }
        else
        {
            mainMenu.SetActive(false);
            gameModeMenu.SetActive(true);
        }
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

        print("game is exiting");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}