using UnityEngine;

public class EndlessGamemode : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner2 asteroidSpawner;

    [SerializeField] private int spawnNumber = 3;

    // Timer settings
    [Header("Timer Settings")]
    [SerializeField] private float spawnTimer;
    private float curTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        asteroidSpawner.SpawnXAsteroids(100);
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= spawnTimer)
        {
            curTime = 0.0f; // Reset timer
            asteroidSpawner.SpawnXAsteroids(spawnNumber);
        }
    }
}
