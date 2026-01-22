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
    }
}
