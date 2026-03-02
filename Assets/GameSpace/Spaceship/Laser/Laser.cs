using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 forwardDirection;
    [SerializeField] private float speed;
    [SerializeField] private float raycastSize = 6.0f;
    private LayerMask layerMask;

    // Timer
    private float time = 0.0f;
    private float timeout = 5.0f;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Asteroids");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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

        if (Physics.SphereCast(transform.position, raycastSize, forwardDirection, out hitData, 1.0f, layerMask) && hitData.collider.tag == "Asteroid")
        {
            // The ray hit an asteroid!
            GameObject asteroid = hitData.transform.gameObject;

            Debug.Log("Asteroid hit!");

            // Now we need to check what type of asteroid it is
            Debug.Log("Hit asteroid is of type " + asteroid.GetComponent<AsteroidParentClass>().GetAsteroidType().ToString());
            
            switch (asteroid.GetComponent<AsteroidParentClass>().GetAsteroidType())
            {
                case AsteroidParentClass.AsteroidInheritanceType.Basic:
                    asteroid.GetComponent<BasicAsteroid>().Die();
                    break;
                case AsteroidParentClass.AsteroidInheritanceType.Healing:
                    asteroid.GetComponent<HealingAsteroid>().Die();
                    break;
                case AsteroidParentClass.AsteroidInheritanceType.Bomb:
                    asteroid.GetComponent<BombAsteroid>().Die();
                    break;
                default:
                    break;
            }

            Destroy(gameObject);
        }
    }
}