// using UnityEngine;
// using System.Collections.Generic;

// public class asteroid : AsteroidParentClass
// {
//     public override void Die()
//     {
//         Debug.Log("Asteroid Shot!");
//         Split();

//         // spawner.RemoveAsteroidFromList(gameObject);     // remove the asteroid from the list
//         ScoreManager.Instance.AddScore(1);      // update score

//         // FXs
//         ExplosionParticleVFX explosion = Instantiate(explosionVFX);
//         explosion.transform.position = gameObject.transform.position;
//         SoundManager.PlaySound(SoundType.EXPLOSION);

//         Destroy(gameObject);

//         // spawner.PrintAsteroidList();
//     }

//     public override void Split()
//     {
//         Debug.Log("Hello?");

//         // Debug.Log($"child asteroid size is {size / 2}");

//         if (size - 50 <= 0)
//             return;

//         // Spawn child asteroids
//         for (int i = 0; i < splitNum; i++)
//         {
//             GameObject childAsteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
            
//             children.Add(childAsteroid);

//             // childAsteroid.GetComponent<asteroid>().SetSpawner(this.spawner);

//             childAsteroid.GetComponent<asteroid>()
//                                             // .SetSpawner(this.spawner)
//                                             .Init(/*iSize = */size - 50,
//                                                    /*iRotationSpeed = */Random.Range(1.0f, 100.0f),
//                                                    /*iRotationDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized,
//                                                    /*iMovementSpeed =*/Random.Range(8.0f, 10.0f),
//                                                    /*iMovementDirection =*/new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized);


//             // spawner.AddAsteroidToList(childAsteroid);
//         }
//     }
// }
