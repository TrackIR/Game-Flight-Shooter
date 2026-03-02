using UnityEngine;

public class BasicAsteroid : AsteroidParentClass
{
    public override void Die()
    {
        Debug.Log("Asteroid Shot!");
        Split();
        ScoreManager.Instance.AddScore(1);
        ExplosionParticleVFX explosion = Instantiate(explosionVFX);
        explosion.transform.position = gameObject.transform.position;
        SoundManager.PlaySound(SoundType.EXPLOSION);
        Destroy(gameObject);
    }

    public override void Split()
    {
        if (size - 1 == 0)
            return;

        // Spawn child asteroids
        for (int i = 0; i < splitNum; i++)
        {
            GameObject asteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            children.Add(asteroid);
            asteroid.GetComponent<AsteroidParentClass>().Init(  /*iSize = */gameObject.GetComponent<AsteroidParentClass>().size - 1,
                                                                /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
                                                                /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
                                                                /*iMovementSpeed =*/Random.Range(4.0f, 5.0f),
                                                                /*iMovementDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized);
        }
    }
}
