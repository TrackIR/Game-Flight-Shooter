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

    // Asteroid Splitting
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int splitNum = 2;
    private List<GameObject> children = new List<GameObject>();
    
    /*=================================================================*/

    public void Init(int iSize, 
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        transform.position += movementDirection * movementSpeed * Time.deltaTime;
    }

    public void Die()
    {
        Split();
        ScoreManager.Instance.AddScore(1 + (int)Mathf.Floor(Time.time * 0.25f));
        Destroy(gameObject);
    }

    public void Split()
    {
        if (size - 1 == 0)
            return;

        // Spawn child asteroids
        for (int i = 0; i < splitNum; i++)
        {
            GameObject asteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            children.Add(asteroid);
            asteroid.GetComponent<asteroid>().Init(/*iSize = */gameObject.GetComponent<asteroid>().size - 1,
                                                   /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                   /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                                                   /*iMovementSpeed =*/Random.Range(4.0f, 5.0f),
                                                   /*iMovementDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized);
        }
    }
}
