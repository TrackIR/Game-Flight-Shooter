using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] UIDocument gameOverMenuDocument;

    public GameObject gameOverMenu;

    private IntegerField scoreField;
    private Button backButton;
    private bool menuEnabled = false;

    void Update()
    {
        if (menuEnabled && Input.GetKeyDown(KeyCode.Space))
        {
            //include other button press effects, if any
            ToMainMenu();
        }
    }

    void OnEnable()
    {
        VisualElement root = gameOverMenuDocument.rootVisualElement;

        scoreField = root.Q<IntegerField>("ScoreField");
        backButton = root.Q<Button>("BackButton");

        backButton.clicked += ToMainMenu;

        scoreField.value = ScoreManager.Instance.GetScore();

        menuEnabled = true;
    }

    void OnDisable()
    {
        backButton.clicked -= ToMainMenu;
    }

    private void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
