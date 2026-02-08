using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int zeroPad = 6;

    void Update()
    {
        int score = ScoreManager.Instance ? ScoreManager.Instance.GetScore() : 0;
        scoreText.text = $"SCORE {score.ToString().PadLeft(zeroPad, '0')}";
    }
}
