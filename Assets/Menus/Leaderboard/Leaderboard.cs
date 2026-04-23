using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    public UIDocument leaderboardDocument;
    private Button mainMenuButton;
    private Button tradeShowButton;
    private Button endlessButton;
    private Button waveButton;

    public GameObject leaderboardMenu;
    public GameObject mainMenu;

    private Label firstName, firstScore;
    private Label secondName, secondScore;
    private Label thirdName, thirdScore;
    private Label fourthName, fourthScore;
    private Label fifthName, fifthScore;
    private Label sixthName, sixthScore;

    private readonly Color activeColor = new(0.4f, 0.8f, 1f, 0.4f);
    
    List<LeaderboardEntry> list = new List<LeaderboardEntry>();


    private void OnEnable()
    {
        VisualElement root = leaderboardDocument.rootVisualElement;

        mainMenuButton = root.Q<Button>("mainMenuButton");
        tradeShowButton = root.Q<Button>("tradeShowMode");
        endlessButton = root.Q<Button>("endlessMode");
        waveButton = root.Q<Button>("waveMode");

        firstName = root.Q<Label>("firstNameLabel");
        secondName = root.Q<Label>("secondNameLabel");
        thirdName = root.Q<Label>("thirdNameLabel");
        fourthName = root.Q<Label>("fourthNameLabel");
        fifthName = root.Q<Label>("fifthNameLabel");
        sixthName = root.Q<Label>("sixthNameLabel");

        firstScore = root.Q<Label>("firstScoreLabel");
        secondScore = root.Q<Label>("secondScoreLabel");
        thirdScore = root.Q<Label>("thirdScoreLabel");
        fourthScore = root.Q<Label>("fourthScoreLabel");
        fifthScore = root.Q<Label>("fifthScoreLabel");
        sixthScore = root.Q<Label>("sixthScoreLabel");

        tradeShowButton.clicked += TradeShowMode;
        endlessButton.clicked += EndlessMode;
        waveButton.clicked += WaveMode;
        mainMenuButton.clicked += MainMenu;

        TradeShowMode();    // Default to show trade show mode scores
    }

    private void OnDisable()
    {
        tradeShowButton.clicked -= TradeShowMode;
        endlessButton.clicked -= EndlessMode;
        waveButton.clicked -= WaveMode;
        mainMenuButton.clicked -= MainMenu;
    }

    private void TradeShowMode()
    {
        tradeShowButton.style.backgroundColor = activeColor;

        endlessButton.style.backgroundColor = Color.black;
        waveButton.style.backgroundColor = Color.black;

        list = LeaderboardManager.Instance.tsEntries;

        PopulateLeaderboard();
    }

    private void EndlessMode()
    {
        endlessButton.style.backgroundColor = activeColor;

        tradeShowButton.style.backgroundColor = Color.black;
        waveButton.style.backgroundColor = Color.black;

        list = LeaderboardManager.Instance.endEntries;

        PopulateLeaderboard();
    }

    private void WaveMode()
    {
        waveButton.style.backgroundColor = activeColor;

        tradeShowButton.style.backgroundColor = Color.black;
        endlessButton.style.backgroundColor = Color.black;

        list = LeaderboardManager.Instance.waveEntries;

        PopulateLeaderboard();
    }

    private void PopulateLeaderboard()
    {
        // Optional: sort by score descending
        list.Sort((a, b) => b.score.CompareTo(a.score));

        Label[] nameLabels =
        {
            firstName, secondName, thirdName, fourthName, fifthName, sixthName
        };
        Label[] scoreLabels =
        {
            firstScore, secondScore, thirdScore, fourthScore, fifthScore, sixthScore
        };

        for (int i = 0; i < nameLabels.Length; i++)
        {
            if (i < list.Count)
            {
                nameLabels[i].text = list[i].playerName;
                scoreLabels[i].text = $"{list[i].score}";
            }
            else
            {
                nameLabels[i].text = "----";
                scoreLabels[i].text = "--";
            }
        }
    }

    private void MainMenu()
    {
        leaderboardMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


}