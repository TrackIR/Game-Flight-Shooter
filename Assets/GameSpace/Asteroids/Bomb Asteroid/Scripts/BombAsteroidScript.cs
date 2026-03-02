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
        ExplosionParticleVFX explosion = Instantiate(explosionVFX);
        explosion.transform.position = gameObject.transform.position;
        SoundManager.PlaySound(SoundType.EXPLOSION);
        Destroy(gameObject);
    }

    private void Explode()
    {
        foreach (GameObject asteroid in surroundingAsteroids)
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
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroids"))
        {
            surroundingAsteroids.Add(other.gameObject);
            Debug.Log("new asteroid added to layer");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (surroundingAsteroids.Contains(other.gameObject))
        {
            surroundingAsteroids.Remove(other.gameObject);
            Debug.Log("removed asteroid from layer");
        }
    }
}

