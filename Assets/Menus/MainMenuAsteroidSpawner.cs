using UnityEngine;

public class MainMenuAsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab; // Assign your asteroid prefab in the Inspector
    public int numberOfAsteroids = 5;
    public float spawnRadius = 10f;
    public float minMovementSpeed = 0.5f;
    public float maxMovementSpeed = 2f;
    public float minRotationSpeed = 10f;
    public float maxRotationSpeed = 50f;
    public float minSize = 1f;
    public float maxSize = 3f;

    // Internal data for each asteroid
    private class AsteroidData
    {
        public Transform transform;
        public Vector3 moveDirection;
        public float moveSpeed;
        public Vector3 rotationAxis;
        public float rotationSpeed;
    }

    private AsteroidData[] asteroids;

    void Start()
    {
        SpawnAsteroids();
    }

    void Update()
    {
        MoveAsteroids();
    }

    void SpawnAsteroids()
    {
        Camera mainCam = Camera.main;
        asteroids = new AsteroidData[numberOfAsteroids];

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            // Random position around camera
            Vector3 randomPos = mainCam.transform.position + Random.onUnitSphere * spawnRadius;
            randomPos.y = Mathf.Max(randomPos.y, 1f); // Keep above ground

            // Random rotation
            Quaternion randomRot = Random.rotation;

            // Instantiate asteroid
            GameObject asteroidInstance = Instantiate(asteroidPrefab, randomPos, randomRot);

            // Random size
            float size = Random.Range(minSize, maxSize);
            asteroidInstance.transform.localScale = Vector3.one * size;

            // Setup movement and rotation
            AsteroidData data = new AsteroidData
            {
                transform = asteroidInstance.transform,
                moveDirection = Random.onUnitSphere.normalized,
                moveSpeed = Random.Range(minMovementSpeed, maxMovementSpeed),
                rotationAxis = Random.onUnitSphere.normalized,
                rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed)
            };

            asteroids[i] = data;
        }
    }

    void MoveAsteroids()
    {
        if (asteroids == null) return;

        for (int i = 0; i < asteroids.Length; i++)
        {
            AsteroidData data = asteroids[i];

            // Move
            data.transform.position += data.moveDirection * data.moveSpeed * Time.deltaTime;

            // Rotate
            data.transform.Rotate(data.rotationAxis, data.rotationSpeed * Time.deltaTime);
        }
    }
}
