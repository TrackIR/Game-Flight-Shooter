using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{

    private Rigidbody rb;
    private PlayerInputActions spaceshipControls;
    private InputAction thrustInput;
    private InputAction rollInput;
    private InputAction pitchInput;
    private InputAction yawInput;

    private float thrust;
    private float roll;
    private float pitch;
    private float yaw;

    private void Awake()
    {
        spaceshipControls = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        thrustInput = spaceshipControls.Player.Thrust;
        thrustInput.Enable();

        rollInput = spaceshipControls.Player.Roll;
        rollInput.Enable();

        pitchInput = spaceshipControls.Player.Pitch;
        pitchInput.Enable();

        yawInput = spaceshipControls.Player.Yaw;
        yawInput.Enable();
    }

    private void OnDisable()
    {
        thrustInput.Disable();
        rollInput.Disable();
        pitchInput.Disable();
        yawInput.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.useGravity = false;

            // adjust these values for feel
            rb.linearDamping = 0.0f;
            rb.angularDamping = 0.5f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Vector2 thrustValue = thrustInput.ReadValue<Vector2>();
        
        thrust = thrustInput.ReadValue<float>();
        roll = rollInput.ReadValue<float>();
        pitch = pitchInput.ReadValue<float>();
        yaw = yawInput.ReadValue<float>();
    }

    // Use for physics operations
    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Acceleration);

        Vector3 rotation = new Vector3(pitch, yaw, roll);
        rb.AddRelativeTorque(rotation, ForceMode.Acceleration);
    }
}
