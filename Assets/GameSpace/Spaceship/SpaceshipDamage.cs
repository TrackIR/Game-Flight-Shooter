using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpaceshipDamage : MonoBehaviour
{
    [Header("Health")]
    public int playerHealth = 5;
    public int maxHealth = 5;

    [Header("References")]
    public GameObject gameOverMenu;
    public DamageFlashEffect damageFlash;
    [SerializeField] private Renderer shipModelRenderer;
    public Image fadeScreenImage;

    [Header("Cooldown Timer Method")]
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    private float startTime;

    [Header("Coroutine Method")]
    public float damageBlinkDuration = 0.8f;
    public float damageBlinkTime = 0.1f;
    public float screenFadingDuration = 1.0f;
    public float screenIsBlackDuration = 1.0f;

    private bool canTakeDamage = true;

    private void Start()
    {
        startTime = Time.time;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Asteroid"))
        {
            if (canTakeDamage)
            {
                TakeDamage();
                StartCoroutine(TakeDamageReorientation());
            }
        }

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void TakeDamage()
    {
        SoundManager.PlaySound(SoundType.DAMAGE, 0.5f);
        playerHealth -= 1;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);

        Debug.Log("Spaceship collided with asteroid! -1 Health");
    }

    public void Heal(int amount)
    {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);

        Debug.Log("Spaceship healed! +" + amount + " Health");
    }

    private void Die()
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

    private IEnumerator TakeDamageReorientation()
    {
        canTakeDamage = false;

        yield return StartCoroutine(BlinkShipModel());

        canTakeDamage = true;
    }

    private IEnumerator BlinkShipModel()
    {
        for (float i = 0.0f; i < 1.0f; i += damageBlinkTime * (1 / damageBlinkDuration))
        {
            shipModelRenderer.enabled = !shipModelRenderer.enabled;
            yield return new WaitForSeconds(damageBlinkTime);
        }

        shipModelRenderer.enabled = true;
    }
}