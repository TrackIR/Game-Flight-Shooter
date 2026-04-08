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
        float flashDuration = flashVFX.main.duration + flashVFX.main.startLifetime.Evaluate(1);
        float sparksDuration = sparksVFX.main.duration + sparksVFX.main.startLifetime.Evaluate(1);
        float fireDuration = fireVFX.main.duration + fireVFX.main.startLifetime.Evaluate(1);
        float smokeDuration = smokeVFX.main.duration + smokeVFX.main.startLifetime.Evaluate(1);

        Destroy(flashVFX, flashDuration);
        Destroy(sparksVFX, sparksDuration);
        Destroy(fireVFX, fireDuration);
        Destroy(smokeVFX, smokeDuration);

        if (!flashVFX && !sparksVFX && !fireVFX && !smokeVFX)
            Destroy(gameObject);
    }
}
