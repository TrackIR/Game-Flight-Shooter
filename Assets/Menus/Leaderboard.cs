using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class Leaderboard : MonoBehaviour
{

    public UIDocument leaderboardDocument;
    public TMP_Text label;
    private Button mainMenuButton;

    private void OnEnable()
    {
        VisualElement root = leaderboardDocument.rootVisualElement;

        mainMenuButton = root.Q<Button>("mainMenuButton");

        mainMenuButton.clicked += MainMenu;
    }

    private void OnDisable()
    {
        mainMenuButton.clicked -= MainMenu;
    }

    void Start()
    {
        label.text = "";
        foreach (var entry in LeaderboardManager.Instance.entries)
        {
            label.text += $"{entry.playerName}: {entry.score}\n";
        }
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
