using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenRadar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public List<Transform> asteroids;
    public RectTransform radarIconPrefab;   // prefab now

    [Header("UI Sliders")]
    public Slider distanceSlider;
    public Slider asteroidCountSlider;

    [Header("Settings")]
    public float screenEdgeMargin = 50f;
    public float maxDistance = 100f;

    private List<RectTransform> radarIcons = new List<RectTransform>();

    void Start()
    {
        // Distance slider
        if (distanceSlider != null)
        {
            distanceSlider.value = maxDistance;
            distanceSlider.onValueChanged.AddListener(UpdateDistance);
        }

        // Asteroid amount slider
        if (asteroidCountSlider != null)
        {
            asteroidCountSlider.value = asteroids.Count;
            asteroidCountSlider.onValueChanged.AddListener(UpdateAsteroidAmount);
        }

        CreateRadarIcons();
    }

    void Update()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            if (asteroids[i] == null) continue;

            UpdateRadarForAsteroid(asteroids[i], radarIcons[i]);
        }
    }

    void UpdateDistance(float value)
    {
        maxDistance = value;
    }

    void UpdateAsteroidAmount(float value)
    {
        int targetCount = Mathf.RoundToInt(value);

        while (asteroids.Count > targetCount)
        {
            asteroids.RemoveAt(asteroids.Count - 1);
        }

        CreateRadarIcons();
    }

    void CreateRadarIcons()
    {
        // Clear old icons
        foreach (var icon in radarIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }

        radarIcons.Clear();

        // Create one icon per asteroid
        foreach (var asteroid in asteroids)
        {
            RectTransform icon = Instantiate(radarIconPrefab, radarIconPrefab.parent);
            radarIcons.Add(icon);
        }
    }

    void UpdateRadarForAsteroid(Transform asteroid, RectTransform icon)
    {
        float distance = Vector3.Distance(player.position, asteroid.position);

        if (distance > maxDistance)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(asteroid.position);

        bool isVisible =
            viewportPoint.z > 0 &&
            viewportPoint.x > 0 && viewportPoint.x < 1 &&
            viewportPoint.y > 0 && viewportPoint.y < 1;

        if (isVisible)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        icon.gameObject.SetActive(true);

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(asteroid.position);

        if (screenPoint.z < 0)
        {
            screenPoint *= -1;
        }

        screenPoint.x = Mathf.Clamp(screenPoint.x, screenEdgeMargin, Screen.width - screenEdgeMargin);
        screenPoint.y = Mathf.Clamp(screenPoint.y, screenEdgeMargin, Screen.height - screenEdgeMargin);

        icon.position = screenPoint;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 direction = (screenPoint - screenCenter).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        icon.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}