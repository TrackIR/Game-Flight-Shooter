using UnityEngine;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private const string tradeShowLeaderboardKey = "LEADERBOARD-TS";
    private const string endlessLeaderboardKey = "LEADERBOARD-E";
    private const string waveLeaderboardKey =  "LEADERBOARD-W";

    // private const string PlayerNameKey = "PLAYER_NAME";

    [Header("Leaderboard Settings")]
    public int maxEntries = 6;

    public List<LeaderboardEntry> tsEntries = new List<LeaderboardEntry>();       // tracks the top 6 trade show scores
    public List<LeaderboardEntry> endEntries = new List<LeaderboardEntry>();      // tracks the top 6 endless scores
    public List<LeaderboardEntry> waveEntries = new List<LeaderboardEntry>();     // tracks the top 6 wave scores
    
    // public string CurrentPlayerName { get; private set; } = "Player";

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

        // LoadPlayerName();
        LoadLeaderboard();
    }

    // public void SetPlayerName(string name)
    // {
    //     name = name?.Trim();
    //     if (string.IsNullOrEmpty(name))
    //         name = "Player";

    //     if (name.Length > 12)
    //         name = name.Substring(0, 12);

    //     CurrentPlayerName = name;

    //     PlayerPrefs.SetString(PlayerNameKey, name);
    //     PlayerPrefs.Save();
    // }

    // private void LoadPlayerName()
    // {
    //     CurrentPlayerName = PlayerPrefs.GetString(PlayerNameKey, "Player");
    // }

    // public void AddScore(int score)
    // {
    //     AddScore(CurrentPlayerName, score);
    // }

    public void AddScore(string name, int score, int gameMode)
    {
        if (gameMode == 0)      // trade show mode
        {
            tsEntries ??= new List<LeaderboardEntry>();       // ensures list is there

            tsEntries.Add(new LeaderboardEntry { playerName = name, score = score });

            tsEntries.Sort((a, b) => b.score.CompareTo(a.score));

            if (tsEntries.Count > maxEntries)
                tsEntries.RemoveRange(maxEntries, tsEntries.Count - maxEntries);

            SaveLeaderboard(0);
        }

        if (gameMode == 1)      // endless mode
        {
            endEntries ??= new List<LeaderboardEntry>();       // ensures list is there

            endEntries.Add(new LeaderboardEntry { playerName = name, score = score });

            endEntries.Sort((a, b) => b.score.CompareTo(a.score));

            if (endEntries.Count > maxEntries)
                endEntries.RemoveRange(maxEntries, endEntries.Count - maxEntries);

            SaveLeaderboard(1);
        }

        if (gameMode == 2)      // wave mode
        {
            waveEntries ??= new List<LeaderboardEntry>();       // ensures list is there

            waveEntries.Add(new LeaderboardEntry { playerName = name, score = score });

            waveEntries.Sort((a, b) => b.score.CompareTo(a.score));

            if (waveEntries.Count > maxEntries)
                waveEntries.RemoveRange(maxEntries, waveEntries.Count - maxEntries);

            SaveLeaderboard(2);
        }
    }

    private void SaveLeaderboard(int mode = -1)     // defaults to -1 (save all)
    {
        if (mode == 0 || mode == -1)
        {
            string json = JsonUtility.ToJson(new LeaderboardData { entries = tsEntries });
            PlayerPrefs.SetString(tradeShowLeaderboardKey, json);
        }

        if (mode == 1 || mode == -1)
        {
            string json = JsonUtility.ToJson(new LeaderboardData { entries = endEntries });
            PlayerPrefs.SetString(endlessLeaderboardKey, json);
        }

        if (mode == 2 || mode == -1)
        {
            string json = JsonUtility.ToJson(new LeaderboardData { entries = waveEntries });
            PlayerPrefs.SetString(waveLeaderboardKey, json);
        }

        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(tradeShowLeaderboardKey))
        {
            string json = PlayerPrefs.GetString(tradeShowLeaderboardKey);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            tsEntries = data?.entries ?? new List<LeaderboardEntry>();            
        }
        else
            tsEntries = new List<LeaderboardEntry>();

        if (PlayerPrefs.HasKey(endlessLeaderboardKey))
        {
            string json = PlayerPrefs.GetString(endlessLeaderboardKey);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            endEntries = data?.entries ?? new List<LeaderboardEntry>();
        }
        else
            endEntries = new List<LeaderboardEntry>();

        if (PlayerPrefs.HasKey(waveLeaderboardKey))
        {
            string json = PlayerPrefs.GetString(waveLeaderboardKey);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            waveEntries = data?.entries ?? new List<LeaderboardEntry>();
        }
        else
            waveEntries = new List<LeaderboardEntry>();
    }

    // public List<LeaderboardEntry> GetEntries()
    // {
    //     if (activeEntries == null)
    //         activeEntries = new List<LeaderboardEntry>();
    //     return new List<LeaderboardEntry>(activeEntries); // return a copy
    // }

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