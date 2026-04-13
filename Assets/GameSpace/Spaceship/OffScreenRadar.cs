using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenRadar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform asteroidParent;
    public RectTransform radarIconPrefab;

    [Header("UI Sliders")]
    public Slider distanceSlider;
    public Slider asteroidCountSlider;

    [Header("Settings")]
    public float screenEdgeMargin = 50f;
    public float maxDistance = 100f;
    public int maxAsteroidsToShow = 10;

    private readonly List<Transform> asteroids = new List<Transform>();
    private readonly List<RectTransform> radarIcons = new List<RectTransform>();

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        if (distanceSlider != null)
        {
            distanceSlider.value = maxDistance;
            distanceSlider.onValueChanged.AddListener(UpdateDistance);
        }

        if (asteroidCountSlider != null)
        {
            asteroidCountSlider.value = maxAsteroidsToShow;
            asteroidCountSlider.onValueChanged.AddListener(UpdateAsteroidAmount);
        }
    }

    void Update()
    {
        if (player == null || asteroidParent == null || radarIconPrefab == null)
            return;

        if (mainCam == null)
            mainCam = Camera.main;

        RefreshAsteroidList();
        UpdateClosestAsteroids();
    }

    void RefreshAsteroidList()
    {
        asteroids.Clear();

        foreach (Transform child in asteroidParent)
        {
            if (child != null)
                asteroids.Add(child);
        }
    }

    void UpdateDistance(float value)
    {
        maxDistance = value;
    }

    void UpdateAsteroidAmount(float value)
    {
        maxAsteroidsToShow = Mathf.RoundToInt(value);
    }

    void UpdateClosestAsteroids()
    {
        asteroids.Sort((a, b) =>
        {
            float distA = Vector3.SqrMagnitude(player.position - a.position);
            float distB = Vector3.SqrMagnitude(player.position - b.position);
            return distA.CompareTo(distB);
        });

        int count = Mathf.Min(maxAsteroidsToShow, asteroids.Count);

        EnsureRadarIconCount(count);

        for (int i = 0; i < count; i++)
        {
            UpdateRadarForAsteroid(asteroids[i], radarIcons[i]);
        }

        for (int i = count; i < radarIcons.Count; i++)
        {
            radarIcons[i].gameObject.SetActive(false);
        }
    }

    void EnsureRadarIconCount(int count)
    {
        while (radarIcons.Count < count)
        {
            RectTransform icon = Instantiate(radarIconPrefab, radarIconPrefab.parent);
            icon.gameObject.SetActive(false);
            radarIcons.Add(icon);
        }

        while (radarIcons.Count > count)
        {
            RectTransform icon = radarIcons[radarIcons.Count - 1];
            radarIcons.RemoveAt(radarIcons.Count - 1);

            if (icon != null)
                Destroy(icon.gameObject);
        }
    }

    void UpdateRadarForAsteroid(Transform asteroid, RectTransform icon)
    {
        if (asteroid == null || icon == null || mainCam == null)
            return;

        float distance = Vector3.Distance(player.position, asteroid.position);

        if (distance > maxDistance)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        Vector3 viewportPoint = mainCam.WorldToViewportPoint(asteroid.position);

        bool isVisible =
            viewportPoint.z > 0f &&
            viewportPoint.x > 0f && viewportPoint.x < 1f &&
            viewportPoint.y > 0f && viewportPoint.y < 1f;

        if (isVisible)
        {
            icon.gameObject.SetActive(false);
            return;
        }

        icon.gameObject.SetActive(true);

        Vector3 screenPoint = mainCam.WorldToScreenPoint(asteroid.position);

        if (screenPoint.z < 0f)
        {
            screenPoint.x = Screen.width - screenPoint.x;
            screenPoint.y = Screen.height - screenPoint.y;
        }

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Vector3 direction = (new Vector3(screenPoint.x, screenPoint.y, 0f) - screenCenter).normalized;

        float halfWidth = (Screen.width * 0.5f) - screenEdgeMargin;
        float halfHeight = (Screen.height * 0.5f) - screenEdgeMargin;

        float scaleX = direction.x != 0f ? halfWidth / Mathf.Abs(direction.x) : float.MaxValue;
        float scaleY = direction.y != 0f ? halfHeight / Mathf.Abs(direction.y) : float.MaxValue;
        float scale = Mathf.Min(scaleX, scaleY);

        Vector3 edgePosition = screenCenter + direction * scale;
        icon.position = edgePosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        icon.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void InitializeRadar(
        Transform playerTransform,
        Transform asteroidContainer,
        RectTransform iconPrefab,
        Slider maxDistanceSlider = null,
        Slider maxCountSlider = null,
        float distanceLimit = 100f,
        int asteroidLimit = 10)
    {
        player = playerTransform;
        asteroidParent = asteroidContainer;
        radarIconPrefab = iconPrefab;
        distanceSlider = maxDistanceSlider;
        asteroidCountSlider = maxCountSlider;
        maxDistance = distanceLimit;
        maxAsteroidsToShow = asteroidLimit;
    }
}