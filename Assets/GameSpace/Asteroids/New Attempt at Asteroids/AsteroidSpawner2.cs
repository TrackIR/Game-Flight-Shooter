using UnityEngine;

public class AsteroidSpawner2 : MonoBehaviour
{
    [Header("Spawn Position")]
    // Parent of all asteroids spawned
    [SerializeField] private Transform parentOfAsteroids;

    // Radius of perimiter where asteroids can spawn
    [SerializeField] private int spawnRadius = 100;

    [Header("Asteroid Types")]
    // Different types of asteroids to spawn
    [SerializeField] private GameObject basicAsteroidPrefab;
    [SerializeField] private GameObject healingAsteroidPrefab;
    [SerializeField] private GameObject bombAsteroidPrefab;

    [Header("Asteroid Size")]
    // Size range of asteroids
    [SerializeField] private int minAsteroidSize = 1;
    [SerializeField] private int maxAsteroidSize = 3;

    [Header("Asteroid Move Speed")]
    // Move speed range of asteroids
    [SerializeField] private float minAsteroidMoveSpeed = 5.0f;
    [SerializeField] private float maxAsteroidMoveSpeed = 10.0f;
 
    [Header("Asteroid Rotation Speed")]
    // Rotation speed range of asteroids
    [SerializeField] private float minAsteroidRotSpeed = 1.0f;
    [SerializeField] private float maxAsteroidRotSpeed = 100.0f;


    // Spawns X amount of asteroids in the spawn radius
    public void SpawnXAsteroids(int x)
    {
        for (int i = 0; i < x; i++)
        {
            // Prepare asteroid to be instantiated
            GameObject asteroid = null;
            int randomSize = Random.Range(minAsteroidSize, maxAsteroidSize);
            float randomMoveSpeed = Random.Range(minAsteroidMoveSpeed, maxAsteroidMoveSpeed);
            float randomRotSpeed = Random.Range(minAsteroidRotSpeed, maxAsteroidRotSpeed);
            Vector3 randomMoveDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            Vector3 randomRotDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            Vector3 randomPosition = parentOfAsteroids.position + (Random.onUnitSphere * spawnRadius);

            // Generate a random number and use that number for chance calculations
            int randChance = Random.Range(1, 20);

            // Spawn asteroids based on chance variable, set their position and rotation, and add them as a child of the chosen parent
            switch (randChance)
            {
                case 1: // Healing Asteroid, 5% of the time
                    asteroid = Instantiate(healingAsteroidPrefab, randomPosition, Quaternion.identity, parentOfAsteroids);
                    asteroid.GetComponent<AsteroidClass>().InitType(AsteroidClass.InheritanceType.Healing);
                    break;
                case 2: // Bomb asteroid, 5% of the time
                    asteroid = Instantiate(bombAsteroidPrefab, randomPosition, Quaternion.identity, parentOfAsteroids);
                    asteroid.GetComponent<AsteroidClass>().InitType(AsteroidClass.InheritanceType.Bomb);
                    break;
                default: // Basic asteroid, 90% of the time
                    asteroid = Instantiate(basicAsteroidPrefab, randomPosition, Quaternion.identity, parentOfAsteroids);
                    asteroid.GetComponent<AsteroidClass>().InitType(AsteroidClass.InheritanceType.Basic);
                    break;
            }

            // Initialize asteroid, which is the same process for all asteroid types
            asteroid.GetComponent<AsteroidClass>().Init(
                /* iSize = */       randomSize,
                /* iMoveSpeed = */  randomMoveSpeed,
                /* iRotSpeed = */   randomRotSpeed,
                /* iMoveDir = */    randomMoveDir,
                /* iRotDir = */     randomRotDir
            );

            // Apply the saved color scheme to the newly spawned asteroid
            var apply = asteroid.GetComponent<ApplySavedColors>();
            if (apply != null)
                apply.ApplyNow();
            
            print(asteroid.GetComponent<AsteroidClass>().GetAsteroidType());
        }
    }
}
