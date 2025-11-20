using UnityEngine;


public class asteroid : MonoBehaviour
{
    // Variables declared in the creation of the asteroid
    private Vector3 movementDirection;
    private float movementSpeed;
    private Vector3 rotationDirection;
    private float rotationSpeed;
    private float size;

    // Events
    public static event Action<asteroid> OnAsteroidDestroyedEvent;

    // Variables used once the asteroid is created
    private int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Choose a random size for the asteroid
        size = Random.Range(1.0f, 3.0f);
        transform.localScale = new Vector3(size, size, size);

        // Choose a random rotation direction and speed
        rotationSpeed = Random.Range(1.0f, 100.0f);
        rotationDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    
        // Choose a random movement direction and speed
        movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementSpeed = Random.Range(1.0f, 5.0f);
    
        // Set a health based on the size of the asteroid
        health = Mathf.FloorToInt(size * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        transform.position += movementDirection * movementSpeed * Time.deltaTime;
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
