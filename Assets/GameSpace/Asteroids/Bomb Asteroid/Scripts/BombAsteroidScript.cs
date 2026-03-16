using UnityEngine;
using System.Collections.Generic;

public class BombAsteroid : AsteroidParentClass
{
    private List<GameObject> surroundingAsteroids = new List<GameObject>();

    public override void Die()
    {
        Debug.Log("Bomb asteroid Shot!");
        Explode();
        ScoreManager.Instance.AddScore(1);
        PlayDeathEffects();
        RemoveSelfFromAsteroidsList();
        Destroy(gameObject);
    }

    private void Explode()
    {
        foreach (GameObject asteroid in surroundingAsteroids)
        {
            Debug.Log($"[Debug in BombAsteroidScript.cs] {asteroid.name} is of type {asteroid.GetComponent<AsteroidParentClass>().GetAsteroidType()}");
            
            switch (asteroid.GetComponent<AsteroidParentClass>().GetAsteroidType())
            {
                case AsteroidParentClass.AsteroidInheritanceType.Basic:
                    RemoveFromSurroundingAsteroids(asteroid);
                    asteroid.GetComponent<BasicAsteroid>().DieNoSplit();
                    break;
                case AsteroidParentClass.AsteroidInheritanceType.Healing:
                    RemoveFromSurroundingAsteroids(asteroid);
                    asteroid.GetComponent<HealingAsteroid>().Die();
                    break;
                case AsteroidParentClass.AsteroidInheritanceType.Bomb:
                    RemoveFromSurroundingAsteroids(asteroid);
                    asteroid.GetComponent<BombAsteroid>().Die();
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids") & other.tag == "Asteroid")
        {
            surroundingAsteroids.Add(other.gameObject);
            Debug.Log($"[Debug in BombAsteroidScript.cs] {other.gameObject.name} added to SurroundingAsteroids");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids") && other.tag == "Asteroid")
        {
            GameObject asteroid = other.gameObject;
            
            // DEBUG: Null checks for a null error
            if (other == null)
            {
                Debug.LogError($"Collider is null on {asteroid.name}");
                return;
            }
            
            if (asteroid == null)
            {
                Debug.LogError($"{asteroid.name} is null");
                return;
            }

            Debug.Log($"[Debug in BombAsteroidScript.cs] {other.tag} tag on {asteroid.name}");

            AsteroidParentClass asteroidComp = asteroid.GetComponent<AsteroidParentClass>();
            if (asteroidComp == null)
            {
                Debug.LogError($"AsteroidParentClass component not found on {asteroid.name}");
                return;
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids") && other.tag == "Asteroid")
        {
            GameObject asteroid = other.gameObject;

            // Null check the asteroid, because this will trigger if it dies from the explosion and thus perform this logic twice
            if (asteroid == null)
            {
                Debug.LogError($"[Debug in BombAsteroidScript.cs] {asteroid} is null! We have a problem!");
                return;
            }

            RemoveFromSurroundingAsteroids(asteroid);
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
        this.gameObject.transform.parent.GetComponent<AsteroidSpawner>().RemoveAsteroidFromList(this.gameObject);
    }

    private void RemoveFromSurroundingAsteroids(GameObject asteroid)
    {
        int findAsteroidID = asteroid.GetComponent<AsteroidParentClass>().GetAsteroidID();
        int findAsteroidIndex = surroundingAsteroids.FindIndex(x => x.GetComponent<AsteroidParentClass>().GetAsteroidID() == findAsteroidID);
        surroundingAsteroids.RemoveAt(findAsteroidIndex);
        Debug.Log($"[Debug in BombAsteroidScript.cs] {asteroid.name} removed from SurroundingAsteroids");
    }
}

