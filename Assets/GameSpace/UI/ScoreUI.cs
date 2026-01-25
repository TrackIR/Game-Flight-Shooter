using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore();
    }
}
