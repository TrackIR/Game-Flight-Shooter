using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameModeMenu : MonoBehaviour
{
    [SerializeField] UIDocument gameModeMenuDocument;

    public static int gameModeSetting = 2;

    public GameObject gameModeMenu;
    public GameObject mainMenu;


    private Button tradeShowButton;
    private Button endlessButton;
    private Button waveButton;
    private Button backButton;


    private void OnEnable()
    {
        VisualElement root = gameModeMenuDocument.rootVisualElement;

        tradeShowButton = root.Q<Button>("tradeShowButton");
        endlessButton = root.Q<Button>("endlessButton");
        waveButton = root.Q<Button>("waveButton");
        backButton = root.Q<Button>("backButton");


        tradeShowButton.clicked += TradeShowMode;
        endlessButton.clicked += EndlessMode;
        waveButton.clicked += WaveMode;
        backButton.clicked += MenuBack;
    }

    private void OnDisable()
    {
        tradeShowButton.clicked -= TradeShowMode;
        endlessButton.clicked -= EndlessMode;
        waveButton.clicked -= WaveMode;
        backButton.clicked -= MenuBack;
    }

    private void TradeShowMode()
    {
        gameModeSetting = 0;        // 0 for trade show
        SceneManager.LoadScene("GameScene");
    }
    
    private void EndlessMode()
    {
        gameModeSetting = 1;        // 1 for endless
        SceneManager.LoadScene("GameScene");
    }

    private void WaveMode()
    {
        gameModeSetting = 2;        // 2 for waves
        SceneManager.LoadScene("GameScene");
    }

    private void MenuBack()
    {
        gameModeMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}