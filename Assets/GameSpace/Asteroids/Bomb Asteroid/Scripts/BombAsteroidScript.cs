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
            if (asteroid != null)
            {
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
            }

            else
            {
                surroundingAsteroids.Remove(asteroid);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids") & other.tag == "Asteroid")
        {
            surroundingAsteroids.Add(other.gameObject);
            Debug.Log("new asteroid added to layer");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids") & other.tag == "Asteroid")
        {
            // DEBUG: Null checks for a null error
            if (other == null)
            {
                Debug.LogError("'Other' collider is null");
                return;
            }
            
            GameObject asteroid = other.gameObject;
            if (asteroid == null)
            {
                Debug.LogError("Asteorid is null");
                return;
            }

            Debug.Log($"{asteroid.GetComponent<Collider>().tag} tag on {asteroid.name}");

            AsteroidParentClass asteroidComp = asteroid.GetComponent<AsteroidParentClass>();
            if (asteroidComp == null)
            {
                Debug.LogError($"AsteroidParentClass component not found on {asteroid.name}");
                return;
            }
        }

        // if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids"))
        // {
        //     GameObject asteroid = other.gameObject;
        //     int findAsteroidID = asteroid.GetComponent<AsteroidParentClass>().GetAsteroidID();
        //     int findAsteroidIndex = surroundingAsteroids.FindIndex(x => x.GetComponent<AsteroidParentClass>().GetAsteroidID() == findAsteroidID);
        //     surroundingAsteroids.RemoveAt(findAsteroidIndex);
        // }
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
}

