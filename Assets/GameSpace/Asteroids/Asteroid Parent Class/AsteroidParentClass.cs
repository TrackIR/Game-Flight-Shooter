using UnityEngine;
using System.Collections.Generic;

public class AsteroidParentClass : MonoBehaviour
{
    public enum AsteroidInheritanceType
    {
        Basic,
        Healing,
        Bomb
    }

    // For identification of unique asteroids when necessary
    private int _asteroidID;
    public int asteroidID { get { return _asteroidID; } set { _asteroidID = value; } }

    // Asteroid Stats
    [SerializeField] protected AsteroidInheritanceType asteroidType;
    public int size;
    protected float rotationSpeed;
    protected Vector3 rotationDirection;
    protected float movementSpeed;
    protected Vector3 movementDirection;

    // Asteroid Splitting
    [SerializeField] protected GameObject asteroidPrefab;
    [SerializeField] protected int splitNum = 2;
    [SerializeField] protected ExplosionParticleVFX explosionVFX;
    protected List<GameObject> children = new List<GameObject>();
    
    /*=================================================================*/

    /*=== Common asteroid logic ===*/

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
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        transform.position += movementDirection * movementSpeed * Time.deltaTime;
    }

    public AsteroidInheritanceType GetAsteroidType()
    {
        return asteroidType;
    }

    public void SetAsteroidID(int newID)
    {
        asteroidID = newID;
    }

    public int GetAsteroidID()
    {
        return asteroidID;
    }

    /*=== Asteroid specific logic ===*/

    public virtual void Die() {}

    public virtual void Split() {}
}
