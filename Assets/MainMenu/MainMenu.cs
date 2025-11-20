using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find the button by name
        Button levelButton = root.Q<Button>("levelModeButton");

        levelButton.clicked += () =>
        {
            SceneManager.LoadScene("GameScene");
        };
    }
}