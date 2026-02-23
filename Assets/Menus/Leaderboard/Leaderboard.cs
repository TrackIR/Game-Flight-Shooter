/*using UnityEngine;
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
}*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    public UIDocument leaderboardDocument;
    private Button mainMenuButton;

    private Label firstPlaceLabel;
    private Label secondPlaceLabel;
    private Label thirdPlaceLabel;
    private Label fourthPlaceLabel;
    private Label fifthPlaceLabel;
    private Label sixthPlaceLabel;

    private void OnEnable()
    {
        VisualElement root = leaderboardDocument.rootVisualElement;

        mainMenuButton = root.Q<Button>("mainMenuButton");

        firstPlaceLabel = root.Q<Label>("FirstPlaceLabel");
        secondPlaceLabel = root.Q<Label>("SecondPlaceLabel");
        thirdPlaceLabel = root.Q<Label>("ThirdPlaceLabel");
        fourthPlaceLabel = root.Q<Label>("FourthPlaceLabel");
        fifthPlaceLabel = root.Q<Label>("FifthPlaceLabel");
        sixthPlaceLabel = root.Q<Label>("SixthPlaceLabel");

        mainMenuButton.clicked += MainMenu;

        PopulateLeaderboard();
    }

    private void OnDisable()
    {
        mainMenuButton.clicked -= MainMenu;
    }

    private void PopulateLeaderboard()
    {
        List<LeaderboardEntry> list = LeaderboardManager.Instance.entries;

        // Optional: sort by score descending
        list.Sort((a, b) => b.score.CompareTo(a.score));

        Label[] labels =
        {
            firstPlaceLabel,
            secondPlaceLabel,
            thirdPlaceLabel,
            fourthPlaceLabel,
            fifthPlaceLabel,
            sixthPlaceLabel
        };

        for (int i = 0; i < labels.Length; i++)
        {
            if (i < list.Count)
            {
                labels[i].text = $"{list[i].playerName}: {list[i].score}";
            }
            else
            {
                labels[i].text = "---";
            }
        }
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}