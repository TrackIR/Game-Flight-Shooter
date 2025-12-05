using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public SpaceshipDamage player;
    public TextMeshProUGUI healthText;

    void Update()
    {
        healthText.text = "Health: " + player.playerHealth;
    }
}
