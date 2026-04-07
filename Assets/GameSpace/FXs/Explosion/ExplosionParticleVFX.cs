using UnityEngine;

public class ExplosionParticleVFX : MonoBehaviour
{
    // Get particles systems
    [SerializeField] private ParticleSystem flashVFX;
    [SerializeField] private ParticleSystem sparksVFX;
    [SerializeField] private ParticleSystem fireVFX;
    [SerializeField] private ParticleSystem smokeVFX;

    private void Awake()
    {
        SetStopActionDestroy(flashVFX);
        SetStopActionDestroy(sparksVFX);
        SetStopActionDestroy(fireVFX);
        SetStopActionDestroy(smokeVFX);

        PlayVFX();
    }
    
    private static void SetStopActionDestroy(ParticleSystem ps)
    {
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }

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
}
