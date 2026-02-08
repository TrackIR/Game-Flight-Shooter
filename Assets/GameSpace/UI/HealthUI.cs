using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private SpaceshipDamage player;

    [Header("Lives Setup")]
    [SerializeField] private int maxLives = 3;

    [Header("Segmented Bar")]
    [SerializeField] private Transform segmentsParent;   // the "Segments" object
    [SerializeField] private Image segmentPrefab;

    [Header("Colors")]
    [SerializeField] private Color healthyColor = new Color(1f, 0.65f, 0f, 1f);   // orange-ish
    [SerializeField] private Color dangerColor = Color.red;                       // when 2 or 1 lives, change color
    [SerializeField] private Color emptyColor = new Color(1f, 1f, 1f, 0.15f);     // dim/empty

    [Header("Optional Text")]
    [SerializeField] private TextMeshProUGUI livesText; // optional

    private Image[] segments;

    private void Awake()
    {
        BuildSegments();
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void BuildSegments()
    {
        if (!segmentsParent || !segmentPrefab) return;


        for (int i = segmentsParent.childCount - 1; i >= 0; i--)
            Destroy(segmentsParent.GetChild(i).gameObject);

        segments = new Image[maxLives];
        for (int i = 0; i < maxLives; i++)
        {
            Image seg = Instantiate(segmentPrefab, segmentsParent);
            seg.gameObject.name = $"LifeSegment_{i + 1}";
            segments[i] = seg;
        }
    }

    private void Refresh()
    {
        if (player == null || segments == null || segments.Length == 0) return;

        int lives = Mathf.Clamp(player.playerHealth, 0, maxLives);

        // “Danger mode” when it drops to 2 or 1
        Color fillColor = (lives <= 2) ? dangerColor : healthyColor;

        for (int i = 0; i < segments.Length; i++)
        {
            bool filled = i < lives;
            segments[i].color = filled ? fillColor : emptyColor;
        }

        if (livesText != null)
            livesText.text = $"{lives}/{maxLives}";
    }
}
