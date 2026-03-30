using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AsteroidSpawner : MonoBehaviour
{
    private enum AsteroidType
    {
        Random,
        Directional
    }

    [Header("References")]
    public GameObject spaceship;

    public GameObject prefab;
    public List<GameObject> asteroids = new List<GameObject>();

    [Header("Radar")]
    public RectTransform radarIconPrefab;   // Assign your UI icon prefab here
    public Camera playerCamera;
    public float screenEdgeBuffer = 40f;
    public int maxRadarIcons = 5;           // How many asteroids to track
    public float maxRadarDistance = 150f;   // Hide icons past this distance

    private List<RectTransform> radarIcons = new List<RectTransform>();

    // Environtmental Timer
    private float m_TotalTime;
    private float m_Timer = 0.0f;
    private float m_SpawnTime = 10.0f;
    
    // Asteroid variables
    private AsteroidType m_SpawnType = AsteroidType.Random;
    private float m_AsteroidMinSize = 20.0f;
    private float m_AsteroidMaxSize = 49.99f;
    private float m_AsteroidSpeedFloor = 15.0f;

    public int m_AsteroidStartAmount = 10;

    void Start()
    {
        spawnAsteroidNoDirection(m_AsteroidStartAmount);
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
        m_TotalTime += Time.deltaTime;

        if (m_Timer >= m_SpawnTime)
        {
            if (m_SpawnType == AsteroidType.Random)
                spawnAsteroidNoDirection(5);
            else if (m_SpawnType == AsteroidType.Directional)
                spawnAsteroidSpaceshipDirection(5);
            m_Timer = 0.0f;
            UpdateSpawnTime();
            UpdateSpawnType();
            Debug.Log("Debug in AsteroidSpawner.cs:28 : Asteroids Spawned");
        }

        UpdateRadar();
    }

    // ---------------- RADAR SYSTEM ----------------

    private void EnsureRadarIcons()
    {
        if (radarIcons.Count == maxRadarIcons)
            return;

        foreach (var icon in radarIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }

        radarIcons.Clear();

        for (int i = 0; i < maxRadarIcons; i++)
        {
            RectTransform icon = Instantiate(radarIconPrefab, radarIconPrefab.parent);
            radarIcons.Add(icon);
        }
    }

    private void UpdateRadar()
    {
        if (radarIconPrefab == null || playerCamera == null || spaceship == null)
            return;

        EnsureRadarIcons();

        List<GameObject> validAsteroids = new List<GameObject>();

        foreach (GameObject asteroid in asteroids)
        {
            if (asteroid != null)
                validAsteroids.Add(asteroid);
        }

        validAsteroids.Sort((a, b) =>
            Vector3.Distance(spaceship.transform.position, a.transform.position)
            .CompareTo(
            Vector3.Distance(spaceship.transform.position, b.transform.position)));

        for (int i = 0; i < radarIcons.Count; i++)
        {
            if (i >= validAsteroids.Count)
            {
                radarIcons[i].gameObject.SetActive(false);
                continue;
            }

            GameObject target = validAsteroids[i];
            float distance = Vector3.Distance(spaceship.transform.position, target.transform.position);

            if (distance > maxRadarDistance)
            {
                radarIcons[i].gameObject.SetActive(false);
                continue;
            }

            Vector3 viewportPos = playerCamera.WorldToViewportPoint(target.transform.position);

            bool isVisible =
                viewportPos.z > 0 &&
                viewportPos.x > 0 && viewportPos.x < 1 &&
                viewportPos.y > 0 && viewportPos.y < 1;

            if (isVisible)
            {
                radarIcons[i].gameObject.SetActive(false);
                continue;
            }

            radarIcons[i].gameObject.SetActive(true);

            Vector3 direction = (target.transform.position - spaceship.transform.position).normalized;
            Vector3 screenDir = playerCamera.WorldToScreenPoint(spaceship.transform.position + direction) -
                                playerCamera.WorldToScreenPoint(spaceship.transform.position);

            float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;
            radarIcons[i].rotation = Quaternion.Euler(0, 0, angle - 90f);

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Vector3 edgePosition = screenCenter + screenDir.normalized * (Screen.height / 2f - screenEdgeBuffer);

            radarIcons[i].position = edgePosition;

            // Scale icon based on distance
            float minScale = 0.3f;
            float maxScale = 2.0f;
            float scaleMaxDistance = 50f;

            float t = Mathf.Clamp01(1f - (distance / scaleMaxDistance));
            float scale = Mathf.Lerp(minScale, maxScale, t);

            radarIcons[i].localScale = Vector3.one * scale;
        }
    }

    // ---------------- ORIGINAL SPAWN LOGIC (UNCHANGED) ----------------

    public void spawnAsteroidNoDirection(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject asteroid = Instantiate(prefab, transform.position + (Random.onUnitSphere * 100.0f), Quaternion.identity);
            asteroids.Add(asteroid);
            asteroid.GetComponent<asteroid>().Init(/*iSize = */Mathf.FloorToInt(Random.Range(m_AsteroidMinSize, m_AsteroidMaxSize)),
                                                   /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                   /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                                                   /*iMovementSpeed =*/Random.Range(m_AsteroidSpeedFloor, m_AsteroidSpeedFloor + 4.0f),
                                                   /*iMovementDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized);
            
            // Apply saved colors to THIS newly spawned asteroid
            var apply = asteroid.GetComponent<ApplySavedColors>();
            if (apply != null) apply.ApplyNow();
            // If ApplySavedColors is on a child instead, use:
            // var apply = asteroidInstance.GetComponentInChildren<ApplySavedColors>();
        }
    }

     public void spawnAsteroidSpaceshipDirection(int amount)
    {
        Vector3 spawnPos;
        Vector3 direction;
        for (int i = 0; i < amount; i++)
        {
            spawnPos = transform.position + (Random.onUnitSphere * 100.0f);
            direction = (spaceship.transform.position - spawnPos).normalized;
            GameObject asteroid = Instantiate(prefab, spawnPos, Quaternion.identity);
            asteroids.Add(asteroid);
            asteroid.GetComponent<asteroid>().Init(/*iSize = */Mathf.FloorToInt(Random.Range(m_AsteroidMinSize, m_AsteroidMaxSize)),
                                                   /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                   /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                                                   /*iMovementSpeed =*/Random.Range(m_AsteroidSpeedFloor, m_AsteroidSpeedFloor + 4.0f),
                                                   /*iMovementDirection =*/direction);
            
            // Apply saved colors to THIS newly spawned asteroid
            var apply = asteroid.GetComponent<ApplySavedColors>();
            if (apply != null) apply.ApplyNow();
            // If ApplySavedColors is on a child instead, use:
            // var apply = asteroidInstance.GetComponentInChildren<ApplySavedColors>();
        }
    }

    private void UpdateSpawnTime()
    {
        Debug.Log("Debug in AsteroidSpawner.cs:44 : Total Time = " + SecToMin(m_TotalTime));

        switch (MinAsInt(SecToMin(m_TotalTime)))
        {
            case 1:
                m_SpawnTime = 5.0f;
                m_AsteroidSpeedFloor = 8.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:54 : m_SpawnTime = 5.0f");
                break;
            case 2:
                m_SpawnTime = 2.5f;
                m_AsteroidSpeedFloor = 12.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:58 : m_SpawnTime = 2.5f");
                break;
            case 3:
                m_SpawnTime = 1.0f;
                m_AsteroidSpeedFloor = 16.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:62 : m_SpawnTime = 1.0f");
                break;
            default:
                m_SpawnTime = 10.0f;
                m_AsteroidSpeedFloor = 4.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:66 : m_SpawnTime = 10.0f");
                break;
        }
    }

    private void UpdateSpawnType()
    {
        if (MinAsInt(SecToMin(m_TotalTime)) >= 1)
            m_SpawnType = AsteroidType.Directional;

        else
            m_SpawnType = AsteroidType.Random;
    }

    private float SecToMin(float seconds)
    {
        return seconds / 60.0f;
    }

    private int MinAsInt(float minutes)
    {
        return (int)minutes;
    }

    private string FormatMin(float minutes)
    {
        // TODO
        return "";
    }
}