using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text label;

    void Start()
    {
        label.text = "Ryan: 20000\nBryan: 15000\nHunter: 5000\nJakob: 2000\nLeo: 500\nSam: 200";
        int score = PlayerPrefs.GetInt("HighScore", 0);
        if (score < 200)
        {
            label.text = "Ryan: 20000\nBryan: 15000\nHunter: 5000\nJakob: 2000\nLeo: 500\nSam: 200\nYou: " + score.ToString();
        }
        else if (score < 500)
        {
            label.text = "Ryan: 20000\nBryan: 15000\nHunter: 5000\nJakob: 2000\nLeo: 500\n" + "You: " + score.ToString() + "\nSam: 200";
        }
        else if (score < 2000)
        {
            label.text = "Ryan: 20000\nBryan: 15000\nHunter: 5000\nJakob: 2000\nYou: " + score.ToString() + "\nLeo: 500\nSam: 200";
        }
        else if (score < 5000)
        {
            label.text = "Ryan: 20000\nBryan: 15000\nHunter: 5000\nYou: " + score.ToString() + "\nJakob: 2000\nLeo: 500\nSam: 200";
        }
        else if (score < 15000)
        {
            label.text = "Ryan: 20000\nBryan: 15000\nYou: " + score.ToString() + "\nHunter: 5000\nJakob: 2000\nLeo: 500\nSam: 200";
        }
        else if (score < 20000)
        {
            label.text = "Ryan: 20000\nYou: " + score.ToString() + "\nBryan: 15000\nHunter: 5000\nJakob: 2000\nLeo: 500\nSam: 200";
        }
        else
        {
            label.text = "You: " + score.ToString() + "\nRyan: 20000\nBryan: 15000\nHunter: 5000\nJakob: 2000\nLeo: 500\nSam: 200";
        }
    }
}
