using UnityEngine;

public class WaveMode : MonoBehaviour
{
    public GameObject gameOverMenu;

    [SerializeField] private AsteroidSpawner asteroidSpawner;

    // // Spawn settings
    [Header("Spawner Settings")]
    [SerializeField] readonly int initWaveAmount = 5;
    [SerializeField] readonly int incWaveAmount = 3;
    [SerializeField] readonly int totalWaves = 5;

    private int waveNumber = 0;

    void Update()
    {
        if (AsteroidSpawner.asteroidCount <= 0)
        {

            if (waveNumber < totalWaves)
            {
                // Debug.Log($"New wave: wave {waveNumber}");

                asteroidSpawner.SpawnXAsteroids((incWaveAmount * waveNumber) + initWaveAmount);
                waveNumber++;
            }
            else{                //finish game mode
                // Debug.Log("Finshed wave mode");

                Time.timeScale = 0f;
                gameOverMenu.SetActive(true);
            }
        }
        
    }
}
