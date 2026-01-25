using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SpaceshipDamage : MonoBehaviour
{
    public int playerHealth = 5;

    //cooldown timer
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    private float startTime;
    //private SpaceshipDeathAnimation deathAnimation;
    public GameObject gameOverMenu;
    public DamageFlashEffect damageFlash;

    // private SpaceshipMovement spaceshipMovement; 
    // private SpaceshipShoot spaceshipShoot;

    private void Start()
    {
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            // Only take damage if enough time has passed
            if (Time.time - startTime - lastDamageTime >= damageCooldown)
            {
                // damageFlash.Flash();
                SoundManager.PlaySound(SoundType.DAMAGE, 0.5f);
                playerHealth -= 1;
                lastDamageTime = Time.time;  // reset cooldown

                Debug.Log("Spaceship collided with asteroid! -1 Health");
            }
            else
            {
                Debug.Log("Hit asteroid, but cooldown active — no damage taken.");
            }
        }

        if (playerHealth <= 0)
        {
            SoundManager.PlaySound(SoundType.DEATH, 0.75f);
            GetComponent<SpaceshipDeathAnimation>().TriggerDeath();

            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (ScoreManager.Instance.GetScore() > highScore)
            {
                PlayerPrefs.SetInt("HighScore", ScoreManager.Instance.GetScore());
                PlayerPrefs.Save();
            }
            Time.timeScale = 0f;
            gameOverMenu.SetActive(true);
        }
    }
}


