using UnityEngine;

public class HealingAsteroid : AsteroidParentClass
{
    [SerializeField] private GameObject healthPickupItemPrefab;

    public override void Die()
    {
        Debug.Log("Healing asteroid Shot!");
        SpawnHealthPickupItem();
        ScoreManager.Instance.AddScore(1);
        PlayDeathEffects();
        RemoveSelfFromAsteroidsList();
        Destroy(gameObject);
    }

    private void SpawnHealthPickupItem()
    {
        GameObject healthPickupItem = Instantiate(healthPickupItemPrefab, transform.position, Quaternion.identity);
    }

    private void PlayDeathEffects()
    {
        ExplosionParticleVFX explosion = Instantiate(explosionVFX);
        explosion.transform.position = gameObject.transform.position;
        SoundManager.PlaySound(SoundType.EXPLOSION);
    }

    private void RemoveSelfFromAsteroidsList()
    {
        this.gameObject.transform.parent.GetComponent<AsteroidSpawner>().RemoveAsteroidFromList(this.gameObject);
    }
}
