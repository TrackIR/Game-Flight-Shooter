using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] UIDocument gameOverMenuDocument;

    public GameObject gameOverMenu;

    private IntegerField scoreField;
    private Button backButton;
    
    void OnEnable()
    {
        VisualElement root = gameOverMenuDocument.rootVisualElement;

        scoreField = root.Q<IntegerField>("ScoreField");
        backButton = root.Q<Button>("BackButton");

        backButton.clicked += ToMainMenu;

        scoreField.value = ScoreManager.Instance.GetScore();
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
