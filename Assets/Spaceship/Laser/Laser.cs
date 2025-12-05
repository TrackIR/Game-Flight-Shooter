using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector3 forwardDirection;
    [SerializeField] private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Asteroid")
        {
            GameObject asteroid = other.gameObject;
            asteroid.GetComponent<asteroid>().Damage(100);
        }

        Destroy(gameObject);
    }
}
