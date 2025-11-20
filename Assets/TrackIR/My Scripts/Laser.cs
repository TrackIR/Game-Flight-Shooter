using UnityEngine;
using UnityEngine.InputSystem;

public class LaserShooter : MonoBehaviour
{
    public float laserRange = 100f;
    public float laserDuration = 0.05f;
    public LineRenderer lineRenderer;

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            ShootLaser();
            Debug.Log("H key Pressed, Shooting Laser");
        }
    }

    void ShootLaser()
    {
        StartCoroutine(FireLaser());
    }

    System.Collections.IEnumerator FireLaser()
    {
        lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, transform.position);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserRange))
        {
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log("Laser hit: " + hit.collider.name);

            SplitOnHit splitter = hit.collider.GetComponent<SplitOnHit>();
            if (splitter != null)
            {
                splitter.Split();
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + transform.forward * laserRange);
        }

        yield return new WaitForSeconds(laserDuration);
        lineRenderer.enabled = false;
    }
}
