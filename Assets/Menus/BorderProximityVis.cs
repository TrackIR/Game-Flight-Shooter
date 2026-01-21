using UnityEngine;

public class BorderFaceProximity : MonoBehaviour
{
    public Transform spaceship;
    public float showDistance = 20f;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    void Update()
    {
        if (!spaceship) return;

        // Vector from face to ship
        Vector3 toShip = spaceship.position - transform.position;

        // Signed distance from the plane
        float distanceToPlane = Vector3.Dot(toShip, transform.forward);

        // Only show when the ship is INSIDE the cube and close to this face
        bool shouldShow =
            distanceToPlane < 0 &&
            Mathf.Abs(distanceToPlane) <= showDistance;

        rend.enabled = shouldShow;
    }
}
