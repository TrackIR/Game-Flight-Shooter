using UnityEngine;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> asteroids = new List<GameObject>();

    void Start()
    {

    }

    public void spawnAsteroid()
    {
        GameObject asteroid = Instantiate(prefab, Random.onUnitSphere * 10.0f, Quaternion.identity);
        asteroids.Add(asteroid);
    }

    public void damageAsteroid()
    {
        asteroids[^1].GetComponent<asteroid>().Damage(1);
    }
}
