using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public static int controlType;

    private void OnEnable()
    {
        
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button levelButton = root.Q<Button>("levelModeButton");
        levelButton.clicked += () =>
        {
            controlType = 1;
            SceneManager.LoadScene("GameScene");
        };

        // Display Mode button
        Button displayButton = root.Q<Button>("displayModeButton");
        displayButton.clicked += () =>
        {
            controlType = 2;
            SceneManager.LoadScene("GameScene");
        };
    }
}