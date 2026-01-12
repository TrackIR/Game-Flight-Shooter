using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SpaceshipDamage : MonoBehaviour
{
    public int playerHealth = 5;

    //cooldown timer
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    public GameObject gameOverMenu;

    // private SpaceshipMovement spaceshipMovement; 
    // private SpaceshipShoot spaceshipShoot;

    // private void Start()
    // {
    //     spaceshipMovement = GetComponent<SpaceshipMovement>();
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            // Only take damage if enough time has passed
            if (Time.time - lastDamageTime >= damageCooldown)
            {
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
            // GameOverUI ui = FindAnyObjectByType<GameOverUI>();
            // if (ui != null)
            // {
                // ui.Show();
                Time.timeScale = 0f;
                // spaceshipMovement.OnDisable();
                // // SpaceshipShoot.OnDestroy();
                gameOverMenu.SetActive(true);
            // }
            // else
            // {
                // Debug.LogError("GameOverUI not found in scene!");
            // }
        }
    }
}
