using UnityEngine;

public class HealingAsteroid : AsteroidParentClass
{
    [SerializeField] private GameObject healthPickupItemPrefab;

    public override void Die()
    {
        Debug.Log("Healing asteroid Shot!");
        SpawnHealthPickupItem();
        ScoreManager.Instance.AddScore(1);
        ExplosionParticleVFX explosion = Instantiate(explosionVFX);
        explosion.transform.position = gameObject.transform.position;
        SoundManager.PlaySound(SoundType.EXPLOSION);
        Destroy(gameObject);
    }

    private void SpawnHealthPickupItem()
    {
        GameObject healthPickupItem = Instantiate(healthPickupItemPrefab, transform.position, Quaternion.identity);
    }
}
