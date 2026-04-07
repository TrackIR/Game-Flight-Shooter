using UnityEngine;

public class BasicAsteroid : AsteroidParentClass
{
    GameObject asteroidSpawner;

    private bool isDead = false;

    private void Start()
    {
        asteroidSpawner = this.gameObject.transform.parent.gameObject;
        if (asteroidSpawner == null)
            Debug.Log("asteroid spawner not found");
    }

    public override void Die()
    {
        // Debug.Log("Asteroid Shot!");

        if(isDead)
            return;

        isDead = true;

        Split();

        ScoreManager.Instance.AddScore(1);
        PlayDeathEffects();
        RemoveSelfFromAsteroidsList();
        Destroy(gameObject);
    }

    public void DieNoSplit()
    {
        Debug.Log("Asteroid exploded with bomb!");

        if(isDead)
            return;

        isDead = true;
        
        ScoreManager.Instance.AddScore(1);
        PlayDeathEffects();
        RemoveSelfFromAsteroidsList();
        Destroy(gameObject);
    }

    public override void Split()
    {
        if (size <= 10)
            return;

        // Spawn child asteroids
        for (int i = 0; i < splitNum; i++)
        {
            GameObject asteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            children.Add(asteroid);
            asteroid.GetComponent<AsteroidParentClass>().Init(  /*iSize = */gameObject.GetComponent<AsteroidParentClass>().size / 2,
                                                                /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                                /*iRotationDirection =*/GenerateRandomDirection(0.5f, 1.0f).normalized,
                                                                /*iMovementSpeed =*/Random.Range(4.0f, 5.0f),
                                                                /*iMovementDirection =*/GenerateRandomDirection(0.5f, 1.0f).normalized);
            
            // Set the split asteroids parent to the current asteroids parent
            asteroidSpawner.GetComponent<AsteroidSpawner>().AddAsteroidToList(asteroid);
            asteroid.transform.parent = asteroidSpawner.transform;
        }
    }

    private void PlayDeathEffects()
    {
        ExplosionParticleVFX explosion = Instantiate(explosionVFX);
        explosion.transform.position = gameObject.transform.position;
        SoundManager.PlaySound(SoundType.EXPLOSION);
    }

    private void RemoveSelfFromAsteroidsList()
    {
        asteroidSpawner
            .GetComponent<AsteroidSpawner>()
            .RemoveAsteroidFromList
            (this
            .gameObject);
    }

    // A method to generate a random vector that is between -1 and 1 exluding 0
    // Min and Max must be positive non-zero floats to work properly
    private Vector3 GenerateRandomDirection(float min, float max)
    {
        float[] axes = new float[3];
        
        for (int i = 0; i < 3; i++)
        {
            axes[i] = Random.Range(min, max);
            if (Random.Range(0, 2) == 1)
                axes[i] = -axes[i];
        }

        return new Vector3(axes[0], axes[1], axes[2]);
    }
}
