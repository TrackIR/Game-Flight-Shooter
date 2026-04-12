using UnityEngine;

public class EndlessMode : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    [SerializeField] private readonly int spawnNumber = 1;

    // Spawn settings
    [Header("Spawner Settings")]
    [SerializeField] float spawnTimerMin = 3;
    [SerializeField] float reachSpawnTimerMin = 60;
    [SerializeField] float initialSpawnTimer = 10;

    private float spawnTimer = 0;
    private float curTime = 0.0f;

    private float spawnEquationM;


    void Start()
    {
        spawnEquationM = (spawnTimerMin - initialSpawnTimer) / reachSpawnTimerMin;

        asteroidSpawner.SpawnXAsteroids(10 - spawnNumber);
    }

    void Update()
    {
        curTime += Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0.0f)
        {
            Debug.Log("Timer reached 0, spawning an asteroid");

            // Reset Spawn timer based on the equation
            spawnTimer = (spawnEquationM * curTime) + initialSpawnTimer;

            if (spawnTimer < spawnTimerMin)
                spawnTimer = spawnTimerMin;

            asteroidSpawner.SpawnXAsteroids(spawnNumber);
        }
    }
}
