using UnityEngine;

public class ExplosionParticleVFX : MonoBehaviour
{
    // Get particles systems
    [SerializeField] private ParticleSystem flashVFX;
    [SerializeField] private ParticleSystem sparksVFX;
    [SerializeField] private ParticleSystem fireVFX;
    [SerializeField] private ParticleSystem smokeVFX;

    public void PlayVFX()
    {
        flashVFX.Play();
        sparksVFX.Play();
        fireVFX.Play();
        smokeVFX.Play();

        // destory the game object after the FX lifetime + some padding
        float life = Mathf.Max(flashVFX.main.duration,
                            sparksVFX.main.duration,
                            fireVFX.main.duration,
                            smokeVFX.main.duration);

        Destroy(gameObject, life + 0.2f);
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
