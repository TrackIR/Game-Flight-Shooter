using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipDamage : MonoBehaviour
{
    public int playerHealth = 5;

    //cooldown timer
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    public DamageFlashEffect damageFlash;
    public AudioSource damageAudio;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            // Only take damage if enough time has passed
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                damageFlash.Flash();
                damageAudio.Play();
                playerHealth -= 1;
                lastDamageTime = Time.time;  // reset cooldown

                Debug.Log("Spaceship collided with asteroid! -1 Health");
            }
            else
            {
                Debug.Log("Hit asteroid, but cooldown active — no damage taken.");
            }
        }

        if (playerHealth == 0)
        {
            GetComponent<SpaceshipDeathAnimation>().TriggerDeath();
            GameOverUI ui = FindObjectOfType<GameOverUI>();
            if (ui != null)
            {
                ui.Show();
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogError("GameOverUI not found in scene!");
            }
        }
    }
}

