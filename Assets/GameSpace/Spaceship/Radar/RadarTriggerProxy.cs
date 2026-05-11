using UnityEngine;

public class RadarTriggerProxy : MonoBehaviour
{
    [SerializeField] private OffscreenRadar radar;

    private void OnTriggerEnter(Collider other)
    {
        if (radar != null)
            radar.AddAsteroid(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (radar != null)
            radar.RemoveAsteroid(other.transform);
    }
}