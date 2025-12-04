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
        // Left/right local offsets (adjust X to move left/right, Y/Z if needed)
        Vector3 leftLocal  = new Vector3(-0.3f, 0f, 0f);
        Vector3 rightLocal = new Vector3( 0.3f, 0f, 0f);

        // Convert local offsets to world space so they follow the ship's rotation/position
        Vector3 leftWorldPos  = transform.TransformPoint(leftLocal);
        Vector3 rightWorldPos = transform.TransformPoint(rightLocal);

        // Use the ship's rotation so lasers face the same direction as before
        Quaternion spawnRotation = transform.rotation;

        // Instantiate left laser
        GameObject laserLeft = Instantiate(laserObject, leftWorldPos, spawnRotation);
        laserLeft.GetComponent<Laser>().forwardDirection = transform.forward;

        // Instantiate right laser
        GameObject laserRight = Instantiate(laserObject, rightWorldPos, spawnRotation);
        laserRight.GetComponent<Laser>().forwardDirection = transform.forward;
    }
}
