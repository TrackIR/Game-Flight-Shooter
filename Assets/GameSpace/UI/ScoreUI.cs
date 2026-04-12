// using TMPro;
// using UnityEngine;

// public class ScoreUI : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI scoreText;
//     [SerializeField] private int zeroPad = 6;

//     private int score;

//     void Start()
//     {
//         if (GameModeMenu.gameModeSetting == 2)  // wave mode (time based score)
//             score = 100000;
//     }
//     void Update()
//     {
//         if (GameModeMenu.gameModeSetting == 2)  // wave mode (time based score)
//             score -= (int) Time.deltaTime;

//         else
//             score = ScoreManager.Instance ? ScoreManager.Instance.GetScore() : 0;
        
//         scoreText.text = $"SCORE {score.ToString().PadLeft(zeroPad, '0')}";

//     }
// }
