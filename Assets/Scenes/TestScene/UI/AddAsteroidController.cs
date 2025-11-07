using UnityEngine;
using UnityEngine.UIElements;

public class AddAsteroidController : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroidSpawner;

    public VisualElement ui;
    public Button addButton;
    public Button damageButton;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }
    
    private void OnEnable()
    {
        addButton = ui.Q<Button>("AddAsteroidButton");
        addButton.clicked += OnAddButtonClicked;

        damageButton = ui.Q<Button>("DamageAsteroidButton");
        damageButton.clicked += OnDamageButtonClicked;
    }

    private void OnAddButtonClicked()
    {
        asteroidSpawner.GetComponent<AsteroidSpawner>().spawnAsteroid();
    }

    private void OnDamageButtonClicked()
    {
        asteroidSpawner.GetComponent<AsteroidSpawner>().damageAsteroid();
    }
}
