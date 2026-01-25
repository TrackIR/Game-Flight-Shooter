using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIDocument mainMenuDocument;

    public GameObject mainMenu;
    public GameObject settingsMenu;


    private Button playButton;
    private Button settingsButton;
    private Button leaderboardButton;
    private Button exitButton;
    
    private void OnEnable()
    {
        VisualElement root = mainMenuDocument.rootVisualElement;

        playButton = root.Q<Button>("playButton");
        settingsButton = root.Q<Button>("settingButton");
        leaderboardButton = root.Q<Button>("leaderboardButton");
        exitButton = root.Q<Button>("exitButton");

        playButton.clicked += PlayGame;
        settingsButton.clicked += DisplaySettingMenu;
        leaderboardButton.clicked += DisplayLeaderBoard;
        exitButton.clicked += ExitGame;
    }

    private void OnDisable()
    {
        playButton.clicked -= PlayGame;
        settingsButton.clicked -= DisplaySettingMenu;
        leaderboardButton.clicked -= DisplayLeaderBoard;
        exitButton.clicked -= ExitGame;
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void DisplaySettingMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
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