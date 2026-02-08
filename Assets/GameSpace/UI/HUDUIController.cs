using UnityEngine;
using UnityEngine.UIElements;

public class HUDUIController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private UIDocument hudDocument;
    [SerializeField] private SpaceshipDamage player;

    [SerializeField] private GameObject legacyHudCanvasRoot;

    [Header("Lives")]
    [SerializeField] private int maxLives = 3;

    [Header("Score")]
    [SerializeField] private int zeroPad = 6;

    private Label scoreLabel;
    private VisualElement healthSegmentsRoot;
    private VisualElement[] segments;

    private int lastLives = -999;
    private int lastScore = -999;

    void OnEnable()
    {
        if (!hudDocument) hudDocument = GetComponent<UIDocument>();

        var root = hudDocument.rootVisualElement;
        scoreLabel = root.Q<Label>("scoreLabel");
        healthSegmentsRoot = root.Q<VisualElement>("healthSegments");

        BuildSegments();
        ForceRefresh();
    }

    void Update()
    {
        if (!player) return;

        int lives = Mathf.Clamp(player.playerHealth, 0, maxLives);
        int score = ScoreManager.Instance ? ScoreManager.Instance.GetScore() : 0;

        if (lives != lastLives)
        {
            UpdateLives(lives);
            lastLives = lives;
        }

        if (score != lastScore)
        {
            UpdateScore(score);
            lastScore = score;
        }
    }

    private void Awake()
    {
        if (legacyHudCanvasRoot != null)
            legacyHudCanvasRoot.SetActive(false);
    }

    private void BuildSegments()
    {
        healthSegmentsRoot.Clear();
        segments = new VisualElement[maxLives];

        for (int i = 0; i < maxLives; i++)
        {
            var seg = new VisualElement();
            seg.AddToClassList("segment");
            healthSegmentsRoot.Add(seg);
            segments[i] = seg;
        }
    }

    private void UpdateLives(int lives)
    {
        bool danger = (lives <= 2);

        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].RemoveFromClassList("segment-empty");
            segments[i].RemoveFromClassList("segment-danger");

            if (i >= lives)
                segments[i].AddToClassList("segment-empty");
            else if (danger)
                segments[i].AddToClassList("segment-danger");
            // else default "segment" stays orange
        }
    }

    private void UpdateScore(int score)
    {
        if (scoreLabel != null)
            scoreLabel.text = $"SCORE {score.ToString().PadLeft(zeroPad, '0')}";
    }

    private void ForceRefresh()
    {
        if (!player) return;
        UpdateLives(Mathf.Clamp(player.playerHealth, 0, maxLives));
        UpdateScore(ScoreManager.Instance ? ScoreManager.Instance.GetScore() : 0);
    }
}
