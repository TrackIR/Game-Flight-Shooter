using UnityEngine;

public class SpaceshipDeathAnimation : MonoBehaviour
{
    public float spinSpeed = 200f;
    public float spiralRadius = 3f;
    public float spiralTightenRate = 1f;
    public float fallSpeed = 5f;
    public float deathDuration = 2f;

    public GameObject explosionPrefab;

    private bool isDying = false;
    private float startTime = 0f;
    private float deathTimer = 0f;
    private Vector3 spiralCenter;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spiralCenter = transform.position;
    }

    public void TriggerDeath()
    {
        if (isDying) return;

        isDying = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        if (!isDying) return;
        deathTimer += Time.deltaTime;

        spiralRadius = Mathf.Max(0, spiralRadius - spiralTightenRate * Time.deltaTime);

        float angle = deathTimer * spinSpeed;
        float offsetX = Mathf.Cos(angle * Mathf.Deg2Rad) * spiralRadius;
        float offsetZ = Mathf.Sin(angle * Mathf.Deg2Rad) * spiralRadius;
        float newY = transform.position.y - fallSpeed * Time.deltaTime;

        transform.position = new Vector3(spiralCenter.x + offsetX, newY, spiralCenter.z + offsetZ);

        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);

        if (deathTimer >= deathDuration)
        {
            if (explosionPrefab)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
