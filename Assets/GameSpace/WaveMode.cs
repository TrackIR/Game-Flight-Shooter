using UnityEngine;

public class WaveMode : MonoBehaviour
{
    public GameObject gameOverMenu;

    [SerializeField] private AsteroidSpawner asteroidSpawner;

    // // Spawn settings
    [Header("Spawner Settings")]
    [SerializeField] int initWaveAmount = 5;
    [SerializeField] int incWaveAmount = 3;
    [SerializeField] int waves = 5;

    private int waveNumber = 0;

    void Update()
    {
        if (AsteroidSpawner.asteroidCount <= 0)
        {
            if (AsteroidSpawner.asteroidCount < 0)
                Debug.Log("Uhmm... how?");

            if (waveNumber < waves)
            {
                Debug.Log($"New wave: wave {waveNumber}");

                asteroidSpawner.SpawnXAsteroids((incWaveAmount * waveNumber) + initWaveAmount);
                waveNumber++;
            }
            else{                //finish game mode
                Debug.Log("Finshed wave mode");

                Time.timeScale = 0f;
                gameOverMenu.SetActive(true);
            }
        }
        
    }
}
