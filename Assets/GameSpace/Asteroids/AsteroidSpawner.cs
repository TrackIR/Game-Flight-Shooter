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
    public List<GameObject> asteroidPrefabs = new List<GameObject>();
    public List<GameObject> asteroidsList = new List<GameObject>();
    /* IMPORTANT */
    // Until I figure out a better system, I'm doing this a stupid way
    // the `asteroidPrefabs` list is going to be set up in a very specific way:
    //  - the first element will be the basic asteroid
    //  - the second element will be the healing asteroid
    //  - the third element will be the bomb asteroid
    // When accessing this list for instantiation or other purposes, please remember this order

    [Header("Radar")]
    public RectTransform radarIcon;   // Assign your UI icon here
    public Camera playerCamera;
    public float screenEdgeBuffer = 40f;

    // Environtmental Timer
    private float m_TotalTime;
    private float m_Timer = 0.0f;
    private float m_SpawnTime = 10.0f;
    
    // Asteroid variables
    private AsteroidType m_SpawnType = AsteroidType.Random;
    private float m_AsteroidMinSize = 3.0f;
    private float m_AsteroidMaxSize = 5.99f;
    private float m_AsteroidSpeedFloor = 5.0f;
    private int   m_IncrementingAsteroidID = 0;

    void Start()
    {
        SpawnAsteroidNoDirection(100);
    }

    void Update()
    {
        // Update the time variables
        m_Timer += Time.deltaTime;
        m_TotalTime += Time.deltaTime;

        // If we have reached the current spawn timer countdown 
        if (m_Timer >= m_SpawnTime)
        {
            // Check the current asteroid spawn type and spawn an asteroid of that type
            if (m_SpawnType == AsteroidType.Random)
                SpawnAsteroidNoDirection(10);
            else if (m_SpawnType == AsteroidType.Directional)
                SpawnAsteroidSpaceshipDirection(10);

            // Update the game state based on time passed in game
            UpdateSpawnTime();
            UpdateSpawnType();
            
            Debug.Log("Debug in AsteroidSpawner.cs:28 : Asteroids Spawned");
            
            // Reset the timer
            m_Timer = 0.0f;
        }

        UpdateRadar();
    }

    /*================================================================*/
    //* Methods for Changing Game State *//

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
    
    /*================================================================*/
    //* Methods for Spawning Asteroids *//

    private GameObject ChooseAsteroidClassToSpawn()
    {
        int choice = Random.Range(1, 20);
        
        // Return an object from the Asteroid Prefabs list
        switch (choice)
        {
            case 1: // If the choice lands on a 5%, return the healing asteroid
                return asteroidPrefabs[1];
            case 2: // Another 5% chance, return bomb asteroid
                return asteroidPrefabs[2];
            default:
                return asteroidPrefabs[0];
        }
    }

    private void GenerateAsteroidID(GameObject asteroid)
    {
        asteroid.GetComponent<AsteroidParentClass>().SetAsteroidID(m_IncrementingAsteroidID);
        m_IncrementingAsteroidID += 1;
    }

    public void SpawnAsteroidNoDirection(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject asteroid = Instantiate(ChooseAsteroidClassToSpawn(), transform.position + (Random.onUnitSphere * 100.0f), Quaternion.identity);
            asteroid.GetComponent<AsteroidParentClass>().Init(  /*iSize = */Mathf.FloorToInt(Random.Range(m_AsteroidMinSize, m_AsteroidMaxSize)),
                                                                /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                                /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                                                                /*iMovementSpeed =*/Random.Range(m_AsteroidSpeedFloor, m_AsteroidSpeedFloor + 4.0f),
                                                                /*iMovementDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized);
            
            // Set the asteroids ID
            GenerateAsteroidID(asteroid);

            // Apply saved colors to THIS newly spawned asteroid
            var apply = asteroid.GetComponent<ApplySavedColors>();
            if (apply != null) apply.ApplyNow();
            // If ApplySavedColors is on a child instead, use:
            // var apply = asteroidInstance.GetComponentInChildren<ApplySavedColors>();

            // Set the asteroids parent to the asteroid spawner game object
            asteroid.transform.parent = this.gameObject.transform;

            // Add asteroid to asteroids list
            AddAsteroidToList(asteroid);
        }
    }

    public void SpawnAsteroidSpaceshipDirection(int amount)
    {
        Vector3 spawnPos;
        Vector3 direction;
        for (int i = 0; i < amount; i++)
        {
            spawnPos = transform.position + (Random.onUnitSphere * 100.0f);
            direction = (spaceship.transform.position - spawnPos).normalized;
            GameObject asteroid = Instantiate(ChooseAsteroidClassToSpawn(), spawnPos, Quaternion.identity);
            asteroid.GetComponent<AsteroidParentClass>().Init(  /*iSize = */Mathf.FloorToInt(Random.Range(m_AsteroidMinSize, m_AsteroidMaxSize)),
                                                                /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                                /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                                                                /*iMovementSpeed =*/Random.Range(m_AsteroidSpeedFloor, m_AsteroidSpeedFloor + 4.0f),
                                                                /*iMovementDirection =*/direction);
            
            // Set the asteroids ID
            GenerateAsteroidID(asteroid);

            // Apply saved colors to THIS newly spawned asteroid
            var apply = asteroid.GetComponent<ApplySavedColors>();
            if (apply != null) apply.ApplyNow();
            // If ApplySavedColors is on a child instead, use:
            // var apply = asteroidInstance.GetComponentInChildren<ApplySavedColors>();

            // Set the asteroids parent to the asteroid spawner game object
            asteroid.transform.parent = this.gameObject.transform;
            
            // Add asteroid to asteroids list
            AddAsteroidToList(asteroid);
        }
    }

    public void AddAsteroidToList(GameObject asteroid)
    {
        asteroidsList.Add(asteroid);
    }

    public void RemoveAsteroidFromList(GameObject asteroid)
    {
        int findAsteroidID = asteroid.GetComponent<AsteroidParentClass>().GetAsteroidID();
        int findAsteroidIndex = asteroidsList.FindIndex(x => x.GetComponent<AsteroidParentClass>().GetAsteroidID() == findAsteroidID);
        asteroidsList.RemoveAt(findAsteroidIndex);
    }

    /*================================================================*/
    //* Methods for Radar System *//

    private void UpdateRadar()
    {
        if (radarIcon == null || playerCamera == null || spaceship == null)
            return;

        GameObject nearest = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject asteroid in asteroidsList)
        {
            if (asteroid == null) continue;

            float distance = Vector3.Distance(spaceship.transform.position, asteroid.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = asteroid;
            }
        }

        if (nearest == null)
        {
            radarIcon.gameObject.SetActive(false);
            return;
        }

        Vector3 viewportPos = playerCamera.WorldToViewportPoint(nearest.transform.position);

        bool isVisible =
            viewportPos.z > 0 &&
            viewportPos.x > 0 && viewportPos.x < 1 &&
            viewportPos.y > 0 && viewportPos.y < 1;

        if (isVisible)
        {
            radarIcon.gameObject.SetActive(false);
            return;
        }

        radarIcon.gameObject.SetActive(true);

        Vector3 direction = (nearest.transform.position - spaceship.transform.position).normalized;
        Vector3 screenDir = playerCamera.WorldToScreenPoint(spaceship.transform.position + direction) -
                            playerCamera.WorldToScreenPoint(spaceship.transform.position);

        float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;
        radarIcon.rotation = Quaternion.Euler(0, 0, angle - 90f);

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 edgePosition = screenCenter + screenDir.normalized * (Screen.height / 2f - screenEdgeBuffer);

        radarIcon.position = edgePosition;

        //Scale icon based on distance
        float minScale = 0.3f;     // size when far away
        float maxScale = 2.0f;     // size when very close
        float maxDistance = 50f;  // distance where scaling stops shrinking

        float t = Mathf.Clamp01(1f - (closestDistance / maxDistance));
        float scale = Mathf.Lerp(minScale, maxScale, t);

        radarIcon.localScale = Vector3.one * scale;
    }

    /*================================================================*/
    //* Methods for Formatting Time *//

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