using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 forwardDirection;
    [SerializeField] private float speed;
    [SerializeField] private float raycastSize = 6.0f;
    private LayerMask layerMask;

    // Timer
    private float time = 0.0f;
    private float timeout = 1.0f;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Asteroids");
    }

    void Update()
    {
        ForceRaycast();
        transform.Translate(forwardDirection * speed * Time.deltaTime);

        time += Time.deltaTime;
        if (time >= timeout)
            Destroy(gameObject);
    }

    // Hitscan Method
    void ForceRaycast()
    {
        // Declare the container for hit data
        RaycastHit hitData;

        if (Physics.SphereCast(transform.position, raycastSize, forwardDirection, out hitData, 6.0f, layerMask)
            && hitData.transform.CompareTag("Asteroid"))
        {
            // Debug.Log(hitData.transform.gameObject);

            // The ray hit an asteroid!
            GameObject asteroid = hitData.transform.gameObject;
            // Debug.Log("Asteroid hit!");

            asteroid.GetComponent<AsteroidClass>().Die(false); // diedByBomb = false
            asteroid.GetComponent<AsteroidClass>().hitByLaser = true;

            Destroy(gameObject);
        }
    }
}