using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipShoot : MonoBehaviour
{
    [SerializeField] private Transform spaceshipModel;
    public GameObject laserObject;
    private InputAction shootAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootAction = InputSystem.actions.FindAction("Shoot");
        shootAction.performed += _ => ShootLaser();
        shootAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Hitscan Method
    void ForceRaycast()
    {
        // Declare the ray
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 50);

        // Declare the container for hit data
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData) && hitData.collider.tag == "Asteroid")
        {
            // The ray hit an asteroid!
            GameObject asteroid = hitData.transform.gameObject;
            asteroid.GetComponent<asteroid>().Damage(100);
        }
    }

    // Projectile Method
    void ShootLaser()
    {
        Vector3 forwardByQuat = transform.rotation * transform.forward;
        Quaternion forwardQuat = Quaternion.Euler(forwardByQuat);
        GameObject laser = Instantiate(laserObject, transform.position, forwardQuat);
        laser.GetComponent<Laser>().forwardDirection = transform.forward;
    }
}
