using UnityEngine;
using System.Collections.Generic;

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

    // Environtmental Timer
    private float m_TotalTime;
    private float m_Timer = 0.0f;
    private float m_SpawnTime = 10.0f;
    
    // Asteroid variables
    private AsteroidType m_SpawnType = AsteroidType.Random;
    private float m_AsteroidMinSize = 2.0f;
    private float m_AsteroidMaxSize = 4.99f;
    private float m_AsteroidSpeedFloor = 5.0f;

    void Start()
    {
        spawnAsteroidNoDirection(100);
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
        m_TotalTime += Time.deltaTime;

        if (m_Timer >= m_SpawnTime)
        {
            if (m_SpawnType == AsteroidType.Random)
                spawnAsteroidNoDirection(10);
            else if (m_SpawnType == AsteroidType.Directional)
                spawnAsteroidSpaceshipDirection(10);
            m_Timer = 0.0f;
            UpdateSpawnTime();
            UpdateSpawnType();
            Debug.Log("Debug in AsteroidSpawner.cs:28 : Asteroids Spawned");
        }
    }

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
