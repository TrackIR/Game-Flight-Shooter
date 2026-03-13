using UnityEngine;

public class HealingAsteroid : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        SpaceshipDamage spaceshipDamage = other.GetComponent<SpaceshipDamage>();

        if (spaceshipDamage == null)
        {
            spaceshipDamage = other.GetComponentInParent<SpaceshipDamage>();
        }

        if (spaceshipDamage != null)
        {
            spaceshipDamage.Heal(healAmount);
            Debug.Log("Healing asteroid collected! +" + healAmount + " health");
            Destroy(gameObject);
        }
    }
}