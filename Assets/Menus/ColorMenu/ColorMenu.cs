using UnityEngine;
using UnityEngine.UIElements;

public class ColorMenu : MonoBehaviour
{
    [SerializeField] private UIDocument colorMenuDocument;

    public GameObject colorMenu;
    public GameObject mainMenu;

    private SliderInt shipR, shipG, shipB;
    private SliderInt astR, astG, astB;

    private Label shipRVal, shipGVal, shipBVal;
    private Label astRVal, astGVal, astBVal;

    private VisualElement shipPreview, astPreview;
    private Button doneButton;

    private const string ShipKey = "ShipColor";
    private const string AstKey  = "AstColor";

    private void OnEnable()
    {
        var root = colorMenuDocument.rootVisualElement;

        shipR = root.Q<SliderInt>("shipR");
        shipG = root.Q<SliderInt>("shipG");
        shipB = root.Q<SliderInt>("shipB");

        astR = root.Q<SliderInt>("astR");
        astG = root.Q<SliderInt>("astG");
        astB = root.Q<SliderInt>("astB");

        shipRVal = root.Q<Label>("shipRVal");
        shipGVal = root.Q<Label>("shipGVal");
        shipBVal = root.Q<Label>("shipBVal");

        astRVal = root.Q<Label>("astRVal");
        astGVal = root.Q<Label>("astGVal");
        astBVal = root.Q<Label>("astBVal");

        shipPreview = root.Q<VisualElement>("shipPreview");
        astPreview = root.Q<VisualElement>("astPreview");

        doneButton = root.Q<Button>("doneButton");

        // Load saved colors (or defaults)
        LoadIntoSliders();

        // Hook events
        shipR.RegisterValueChangedCallback(_ => UpdateShipUI());
        shipG.RegisterValueChangedCallback(_ => UpdateShipUI());
        shipB.RegisterValueChangedCallback(_ => UpdateShipUI());

        astR.RegisterValueChangedCallback(_ => UpdateAstUI());
        astG.RegisterValueChangedCallback(_ => UpdateAstUI());
        astB.RegisterValueChangedCallback(_ => UpdateAstUI());

        doneButton.clicked += Done;

        // Refresh previews/labels
        UpdateShipUI();
        UpdateAstUI();
    }

    private void OnDisable()
    {
        if (doneButton != null) doneButton.clicked -= Done;
    }

    private void LoadIntoSliders()
    {
        var ship = LoadColor(ShipKey, new Color(240/255f, 250/255f, 255/255f, 1f));
        var ast  = LoadColor(AstKey,  new Color(180/255f, 220/255f, 255/255f, 1f));

        shipR.value = Mathf.RoundToInt(ship.r * 255f);
        shipG.value = Mathf.RoundToInt(ship.g * 255f);
        shipB.value = Mathf.RoundToInt(ship.b * 255f);

        astR.value = Mathf.RoundToInt(ast.r * 255f);
        astG.value = Mathf.RoundToInt(ast.g * 255f);
        astB.value = Mathf.RoundToInt(ast.b * 255f);
    }

    private void UpdateShipUI()
    {
        shipRVal.text = shipR.value.ToString();
        shipGVal.text = shipG.value.ToString();
        shipBVal.text = shipB.value.ToString();

        var c = new Color(shipR.value/255f, shipG.value/255f, shipB.value/255f, 1f);
        shipPreview.style.backgroundColor = new StyleColor(c);
    }

    private void UpdateAstUI()
    {
        astRVal.text = astR.value.ToString();
        astGVal.text = astG.value.ToString();
        astBVal.text = astB.value.ToString();

        var c = new Color(astR.value/255f, astG.value/255f, astB.value/255f, 1f);
        astPreview.style.backgroundColor = new StyleColor(c);
    }

    private void Done()
    {
        var ship = new Color(shipR.value/255f, shipG.value/255f, shipB.value/255f, 1f);
        var ast  = new Color(astR.value/255f,  astG.value/255f,  astB.value/255f,  1f);

        SaveColor(ShipKey, ship);
        SaveColor(AstKey, ast);

        // Return to main menu
        colorMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private static void SaveColor(string keyPrefix, Color c)
    {
        PlayerPrefs.SetFloat(keyPrefix + "_R", c.r);
        PlayerPrefs.SetFloat(keyPrefix + "_G", c.g);
        PlayerPrefs.SetFloat(keyPrefix + "_B", c.b);
        PlayerPrefs.Save();
    }

    private static Color LoadColor(string keyPrefix, Color fallback)
    {
        if (!PlayerPrefs.HasKey(keyPrefix + "_R")) return fallback;

        float r = PlayerPrefs.GetFloat(keyPrefix + "_R", fallback.r);
        float g = PlayerPrefs.GetFloat(keyPrefix + "_G", fallback.g);
        float b = PlayerPrefs.GetFloat(keyPrefix + "_B", fallback.b);
        return new Color(r, g, b, 1f);
    }
}
