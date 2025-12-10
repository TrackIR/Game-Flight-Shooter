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
    
    // public static int controlType = 1;

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

    // private void Enable()
    // {
        
    //     var root = GetComponent<UIDocument>().rootVisualElement;

    //     Button levelButton = root.Q<Button>("levelModeButton");
    //     levelButton.clicked += () =>
    //     {
    //         controlType = 1;
    //         SceneManager.LoadScene("GameScene");
    //     };

    //     // Display Mode button
    //     Button displayButton = root.Q<Button>("displayModeButton");
    //     displayButton.clicked += () =>
    //     {
    //         controlType = 2;
    //         SceneManager.LoadScene("GameScene");
    //     };
    // }
}