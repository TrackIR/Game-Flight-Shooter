using UnityEngine;
using UnityEngine.UIElements;

public class ButtonHoverEffects : MonoBehaviour
{
    public Color hoverColor = new Color(0.7137255f, 0.372549f, 0f);   // dark orange
    public Color normalColor = Color.black;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        foreach (Button btn in root.Query<Button>().ToList())
        {
            btn.RegisterCallback<MouseEnterEvent>(e =>
            {
                btn.style.backgroundColor = hoverColor;
            });

            btn.RegisterCallback<MouseLeaveEvent>(e =>
            {
                btn.style.backgroundColor = normalColor;
            });
        }
    }
}
