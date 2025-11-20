using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button levelButton = root.Q<Button>("levelModeButton");
        levelButton.clicked += () =>
        {
            SceneManager.LoadScene("GameScene");
        };

        // Display Mode button
        Button displayButton = root.Q<Button>("displayModeButton");
        displayButton.clicked += () =>
        {
            SceneManager.LoadScene("Asteroid");
        };
    }
}