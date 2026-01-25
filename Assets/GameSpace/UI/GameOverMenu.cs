using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] UIDocument gameOverMenuDocument;

    public GameObject gameOverMenu;

    private IntegerField scoreField;
    private TextField nameField;
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
        nameField = root.Q<TextField>("nameField");
        backButton = root.Q<Button>("BackButton");

        backButton.clicked += ToMainMenu;

        scoreField.value = ScoreManager.Instance.GetScore();

        menuEnabled = true;


        StartCoroutine(FocusNameField());

    }


    private IEnumerator FocusNameField()
    {
        yield return null; // wait 1 frame
        nameField.Focus();
        nameField.SelectAll(); // optional: auto-select text
    }

    void OnDisable()
    {
        backButton.clicked -= ToMainMenu;
    }

    private void ToMainMenu()
    {
        LeaderboardManager.Instance.AddScore(nameField.value, ScoreManager.Instance.GetScore());

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
