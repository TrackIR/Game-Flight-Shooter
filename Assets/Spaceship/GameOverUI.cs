using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverCanvas;   // Drag Canvas here
    public Button backToMenuButton;     // Drag the button here

    void Start()
    {
        gameOverCanvas.SetActive(false); // hidden at start
        backToMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Main Menu");
        });
    }

    public void Show()
    {
        gameOverCanvas.SetActive(true);
    }
}
