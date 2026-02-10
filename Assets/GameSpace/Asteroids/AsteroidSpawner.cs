using UnityEngine;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> asteroids = new List<GameObject>();

    // Environtmental Timer
    private float m_TotalTime;
    private float m_Timer = 0.0f;
    private float m_SpawnTime = 10.0f;

    void Start()
    {
        spawnAsteroid(100);
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
        m_TotalTime += Time.deltaTime;

        if (m_Timer >= m_SpawnTime)
        {
            spawnAsteroid(10);
            m_Timer = 0.0f;
            UpdateSpawnTime();
            Debug.Log("Debug in AsteroidSpawner.cs:28 : Asteroids Spawned");
        }
    }

    public void spawnAsteroid(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject asteroidInstance =
                Instantiate(prefab, transform.position + (Random.onUnitSphere * 100.0f), Quaternion.identity);

            asteroids.Add(asteroidInstance);

            asteroidInstance.GetComponent<asteroid>().Init(
                Mathf.FloorToInt(Random.Range(1.0f, 3.99f)),
                Random.Range(1.0f, 100.0f),
                new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                Random.Range(1.0f, 5.0f),
                new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized
            );

            // Apply saved colors to THIS newly spawned asteroid
            var apply = asteroidInstance.GetComponent<ApplySavedColors>();
            if (apply != null) apply.ApplyNow();
            // If ApplySavedColors is on a child instead, use:
            // var apply = asteroidInstance.GetComponentInChildren<ApplySavedColors>();
        }
    }


    private void UpdateSpawnTime()
    {
        Debug.Log("Debug in AsteroidSpawner.cs:44 : Total Time = " + SecToMin(m_TotalTime));

        switch (MinInInt(SecToMin(m_TotalTime)))
        {
            case 1:
                m_SpawnTime = 7.5f;
                Debug.Log("Debug in AsteroidSpawner.cs:50 : m_SpawnTime = 7.5f");
                break;
            case 2:
                m_SpawnTime = 5.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:54 : m_SpawnTime = 5.0f");
                break;
            case 3:
                m_SpawnTime = 2.5f;
                Debug.Log("Debug in AsteroidSpawner.cs:58 : m_SpawnTime = 2.5f");
                break;
            case 4:
                m_SpawnTime = 1.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:62 : m_SpawnTime = 1.0f");
                break;
            default:
                m_SpawnTime = 10.0f;
                Debug.Log("Debug in AsteroidSpawner.cs:66 : m_SpawnTime = 10.0f");
                break;
        }
    }

    private float SecToMin(float seconds)
    {
        return seconds / 60.0f;
    }

    private int MinInInt(float minutes)
    {
        return (int)minutes;
    }

    private string FormatMin(float minutes)
    {
        // TODO
        return "";
    }
}
