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
    
    private void OnEnable()
    {
        VisualElement root = mainMenuDocument.rootVisualElement;

        playButton = root.Q<Button>("playButton");
        settingsButton = root.Q<Button>("settingButton");
        leaderboardButton = root.Q<Button>("leaderboardButton");

        playButton.clicked += PlayGame;
        settingsButton.clicked += DisplaySettingMenu;
        leaderboardButton.clicked += DisplayLeaderBoard;
    }

    private void OnDisable()
    {
        playButton.clicked -= PlayGame;
        settingsButton.clicked -= DisplaySettingMenu;
        leaderboardButton.clicked -= DisplayLeaderBoard;
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
        print("display leaderboard");
    }
}