using UnityEngine;

public class SplitOnHit : MonoBehaviour
{
    public float offset = 0.5f;  // How far apart the duplicates spawn

    // Call this when the laser hits the object
    public void Split()
    {
        // Duplicate #1
        GameObject clone1 = Instantiate(gameObject, transform.position + Vector3.left * offset, transform.rotation);

        // Duplicate #2
        GameObject clone2 = Instantiate(gameObject, transform.position + Vector3.right * offset, transform.rotation);

        // Optional: rename so hierarchy is cleaner
        clone1.name = $"{gameObject.name}_Clone1";
        clone2.name = $"{gameObject.name}_Clone2";

        // Destroy the original
        Destroy(gameObject);
    }
}