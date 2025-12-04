using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipDamage : MonoBehaviour
{
    int playerHealth = 5;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Asteroid"))
        {
            playerHealth -= 1;
            Debug.Log("Spaceship collided with asteroid! -1 Health");
        }
        if (playerHealth == 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
