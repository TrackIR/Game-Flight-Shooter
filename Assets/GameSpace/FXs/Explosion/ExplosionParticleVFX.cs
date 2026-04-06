using UnityEngine;

public class ExplosionParticleVFX : MonoBehaviour
{
    // Get particles systems
    [SerializeField] private ParticleSystem flashVFX;
    [SerializeField] private ParticleSystem sparksVFX;
    [SerializeField] private ParticleSystem fireVFX;
    [SerializeField] private ParticleSystem smokeVFX;

    private void Start()
    {

    }

    public void PlayVFX()
    {
        flashVFX.Play();
        sparksVFX.Play();
        fireVFX.Play();
        smokeVFX.Play();
    }

    private void Update()
    {
        float flashDuration = flashVFX.duration + flashVFX.startLifetime;
        float sparksDuration = sparksVFX.duration + sparksVFX.startLifetime;
        float fireDuration = fireVFX.duration + fireVFX.startLifetime;
        float smokeDuration = smokeVFX.duration + smokeVFX.startLifetime;

        Destroy(flashVFX, flashDuration);
        Destroy(sparksVFX, sparksDuration);
        Destroy(fireVFX, fireDuration);
        Destroy(smokeVFX, smokeDuration);

        if (!flashVFX && !sparksVFX && !fireVFX && !smokeVFX)
            Destroy(gameObject);
    }
}
