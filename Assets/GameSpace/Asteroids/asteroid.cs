using UnityEngine;
using System.Collections.Generic;

public class asteroid : MonoBehaviour
{
    // Asteroid Stats
    public int size;
    private float rotationSpeed;
    private Vector3 rotationDirection;
    private float movementSpeed;
    private Vector3 movementDirection;

    [Header("Asteroid Type")]
    [SerializeField] private bool isHealingAsteroid = false;

    // Asteroid Splitting
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int splitNum = 2;
    [SerializeField] private ExplosionParticleVFX explosionVFX;
    private List<GameObject> children = new List<GameObject>();

    public void Init(
        int iSize,
        float iRotationSpeed,
        Vector3 iRotationDirection,
        float iMovementSpeed,
        Vector3 iMovementDirection)
    {
        size = iSize;
        rotationSpeed = iRotationSpeed;
        rotationDirection = iRotationDirection;
        movementSpeed = iMovementSpeed;
        movementDirection = iMovementDirection;

        transform.localScale = new Vector3(size, size, size);
    }

    void Update()
    {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        transform.position += movementDirection * movementSpeed * Time.deltaTime;
    }

    public void Die()
    {
        if (isHealingAsteroid)
        {
            Debug.Log("Healing asteroid destroyed.");
            Destroy(gameObject);
            return;
        }

        Debug.Log("Asteroid Shot!");
        Split();
        ScoreManager.Instance.AddScore(1);

        ExplosionParticleVFX explosion = Instantiate(explosionVFX);
        explosion.transform.position = gameObject.transform.position;
        SoundManager.PlaySound(SoundType.EXPLOSION);

        Destroy(gameObject);
    }

    public void Split()
    {
        if (isHealingAsteroid)
            return;

        if (size - 1 == 0)
            return;

        for (int i = 0; i < splitNum; i++)
        {
            GameObject asteroidObj = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            children.Add(asteroidObj);

            asteroid asteroidScript = asteroidObj.GetComponent<asteroid>();
            if (asteroidScript != null)
            {
                asteroidScript.Init(
                    gameObject.GetComponent<asteroid>().size - 1,
                    Random.Range(1.0f, 100.0f),
                    new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                    Random.Range(4.0f, 5.0f),
                    new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized
                );
            }
        }
    }
}