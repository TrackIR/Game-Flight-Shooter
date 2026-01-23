using UnityEngine;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private const string LeaderboardKey = "LEADERBOARD";
    private const string PlayerNameKey = "PLAYER_NAME";

    [Header("Leaderboard Settings")]
    public int maxEntries = 10;

    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    public string CurrentPlayerName { get; private set; } = "Player";

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadPlayerName();
        LoadLeaderboard();
    }

    public void SetPlayerName(string name)
    {
        name = name?.Trim();
        if (string.IsNullOrEmpty(name))
            name = "Player";

        if (name.Length > 12)
            name = name.Substring(0, 12);

        CurrentPlayerName = name;

        PlayerPrefs.SetString(PlayerNameKey, name);
        PlayerPrefs.Save();
    }

    private void LoadPlayerName()
    {
        CurrentPlayerName = PlayerPrefs.GetString(PlayerNameKey, "Player");
    }

    public void AddScore(int score)
    {
        AddScore(CurrentPlayerName, score);
    }

    public void AddScore(string name, int score)
    {
        if (entries == null)
            entries = new List<LeaderboardEntry>();

        entries.Add(new LeaderboardEntry { playerName = name, score = score });

        entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (entries.Count > maxEntries)
            entries.RemoveRange(maxEntries, entries.Count - maxEntries);

        SaveLeaderboard();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new LeaderboardData { entries = entries });
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        if (!PlayerPrefs.HasKey(LeaderboardKey))
        {
            entries = new List<LeaderboardEntry>();
            return;
        }

        string json = PlayerPrefs.GetString(LeaderboardKey);
        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);

        entries = data?.entries ?? new List<LeaderboardEntry>();
    }

    public List<LeaderboardEntry> GetEntries()
    {
        if (entries == null)
            entries = new List<LeaderboardEntry>();
        return new List<LeaderboardEntry>(entries); // return a copy
    }

}

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}