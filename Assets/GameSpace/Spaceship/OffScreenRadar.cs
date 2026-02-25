using System.Collections.Generic;
using UnityEngine;

public class OffscreenRadar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public List<Transform> asteroids;
    public RectTransform radarIcon;

    [Header("Settings")]
    public float screenEdgeMargin = 50f;

    void Update()
    {
        Transform nearest = GetNearestOffscreenAsteroid();

        if (nearest != null)
        {
            radarIcon.gameObject.SetActive(true);
            UpdateRadar(nearest);
        }
        else
        {
            radarIcon.gameObject.SetActive(false);
        }
    }

    Transform GetNearestOffscreenAsteroid()
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var asteroid in asteroids)
        {
            if (asteroid == null) continue;

            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(asteroid.position);

            bool isVisible =
                viewportPoint.z > 0 &&
                viewportPoint.x > 0 && viewportPoint.x < 1 &&
                viewportPoint.y > 0 && viewportPoint.y < 1;

            if (!isVisible)
            {
                float dist = Vector3.Distance(player.position, asteroid.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = asteroid;
                }
            }
        }

        return nearest;
    }

    void UpdateRadar(Transform target)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.position);

        // If behind camera, flip direction
        if (screenPoint.z < 0)
        {
            screenPoint *= -1;
        }

        // Clamp to screen edge
        screenPoint.x = Mathf.Clamp(screenPoint.x, screenEdgeMargin, Screen.width - screenEdgeMargin);
        screenPoint.y = Mathf.Clamp(screenPoint.y, screenEdgeMargin, Screen.height - screenEdgeMargin);

        radarIcon.position = screenPoint;

        // Rotate to face asteroid direction
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 direction = (screenPoint - screenCenter).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        radarIcon.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}