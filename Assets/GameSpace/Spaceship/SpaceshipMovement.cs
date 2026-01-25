using UnityEngine;
using UnityEngine.InputSystem;

using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    // Defines the rigidbody for the object to use
    private Rigidbody rb;

    // Sets up the keyboard controls for the spaceship
    private PlayerInputActions spaceshipControls;
    private InputAction thrustInput;
    private InputAction pitchInput;
    private InputAction yawInput;
    private InputAction rollInput;

    // Input type
    public static int InputType = 1;       // 1 = no angular momentum, 2 = yes angular momentum

    // Used to change the movement of the spaceship
    private float thrust;
    private float pitch;
    private float yaw;
    private float roll;

    // Spaceship feel values
    public float MaxSpeed = 4.25f;
    public float LinearDamping = 0.5f;
    public float AngularDamping = 0.5f;

    public static float ThrustScaler = 50.0f;
    public static float PitchScaler = 1.0f;
    public static float YawScaler = 1.5f;
    public static float RollScaler = 1.1f;

    // Direct rotation feel (deg/sec) instead of per tick
    public float DirectRotationSpeed = 120.0f;

    // TrackIR varibles
    public UInt16 AssignedApplicationId = 1000;     // Unique ID for application, used by TrackIR software
    public float TrackingLostTimeoutSeconds = 3.0f;     // Seconds till tracking is considered lost
    public float TrackingLostRecenterDurationSeconds = 1.0f;    // Seconds it takes to recenter after tracking is lost
    float m_staleDataDuration;      // How long since new tracking data

    // Cache TrackIR rotation so it doesn't "drop to zero" between pose updates
    private float trackirPitch;
    private float trackirYaw;
    private float trackirRoll;
    private float trackirThrust;

    NaturalPoint.TrackIR.Client m_trackirClient;        // Helper class for interacting with TrackIR API


    private void OnEnable()
    {
        thrustInput = InputSystem.actions.FindAction("Thrust");
        thrustInput.Enable();

        pitchInput = InputSystem.actions.FindAction("Pitch");
        pitchInput.Enable();

        yawInput = InputSystem.actions.FindAction("Yaw");
        yawInput.Enable();

        rollInput = InputSystem.actions.FindAction("Roll");
        rollInput.Enable();
    }

    public void OnDisable()
    {
        thrustInput.Disable();
        pitchInput.Disable();
        yawInput.Disable();
        rollInput.Disable();
    }

    void OnApplicationQuit()
    {
        ShutDownTrackIR();
    }

    private void ShutDownTrackIR()
    {
        if (m_trackirClient != null)
            m_trackirClient.Disconnect();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;

            // Adjust these values for feel
            rb.linearDamping = LinearDamping;
            rb.angularDamping = AngularDamping;

            // Helps smooth rendering between FixedUpdate steps
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        InitializeTrackIR();
    }

    /// <summary>
    /// Attempts to instantiate the TrackIR client object using the specified application ID as well as the handle for
    /// Unity's foreground window.
    /// </summary>
    /// <remarks>
    /// If the user does not have the TrackIR software installed, the client constructor will throw, m_trackirClient
    /// will be null, and subsequent update/shutdown calls will early out accordingly.
    /// </remarks>
    private void InitializeTrackIR()
    {
        try
        {
            m_trackirClient = new NaturalPoint.TrackIR.Client(AssignedApplicationId, TrackIRNativeMethods.GetUnityHwnd());
        }
        catch (NaturalPoint.TrackIR.TrackIRException ex)
        {
            Debug.LogWarning("TrackIR Enhanced API not available.");
            Debug.LogException(ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Read keyboard every frame
        float kbThrust = thrustInput.ReadValue<float>() * ThrustScaler;

        // For direct mode we treat pitch/yaw/roll as "input" (-1..1) and convert to deg/sec later
        float kbPitch = pitchInput.ReadValue<float>();
        float kbYaw = yawInput.ReadValue<float>();
        float kbRoll = -rollInput.ReadValue<float>();

        // Update TrackIR
        UpdateTrackIR();

        // Combine inputs (TrackIR stays stable between updates; keyboard is continuous)
        thrust = kbThrust + trackirThrust;

        // For direct mode
        pitch = kbPitch + trackirPitch;
        yaw = kbYaw + trackirYaw;
        roll = kbRoll + trackirRoll;
    }

    // Use for physics operations (is called at fixed time intervals not every frame)
    void FixedUpdate()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        // Thrust, if less than max speed
        if (currentSpeed < MaxSpeed)
            rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Acceleration);

        // Rotation
        // head match rotation
        if (InputType == 1)
        {
            // Convert input to degrees per second, then apply per fixed step
            Vector3 eulerDelta =
                new Vector3(pitch, yaw, roll) * DirectRotationSpeed * Time.fixedDeltaTime;

            // Use Rigidbody rotation
            rb.MoveRotation(rb.rotation * Quaternion.Euler(eulerDelta));
        }
        //key board and angular momentum
        else
        {
            Vector3 rotation = new(pitch * PitchScaler, yaw * YawScaler, roll * RollScaler);
            rb.AddRelativeTorque(rotation, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Checks for the availability of new head tracking data. If new data is available, it's applied to this
    /// GameObject's position and orientation.
    /// </summary>
    /// <remarks>
    /// If no new data is available for longer than the configured timeout, tracking is considered lost, and we
    /// gradually recenter the object's position and orientation (interpolating both to identity over the duration
    /// specified by <see cref="TrackingLostRecenterDurationSeconds"/>).
    /// </remarks>
    private void UpdateTrackIR()
    {
        if (m_trackirClient != null)
        {
            bool bNewPoseAvailable = false;

            // UpdatePose() could throw if it attempts and fails to reconnect.
            // This should be rare. We'll treat it as non-recoverable.
            try
            {
                bNewPoseAvailable = m_trackirClient.UpdatePose();
            }
            catch (NaturalPoint.TrackIR.TrackIRException ex)
            {
                Debug.LogError("TrackIR.Client.UpdatePose threw an exception.");
                Debug.LogException(ex);

                m_trackirClient.Disconnect();
                m_trackirClient = null;
                return;
            }

            NaturalPoint.TrackIR.Pose pose = m_trackirClient.LatestPose;

            if (bNewPoseAvailable)
            {
                // New data was available, cache it so it doesn't "drop to zero" between updates.

                // head match rotation
                if (InputType == 1)
                {
                    // Cache as input-like values
                    trackirPitch = -pose.Orientation.X;
                    trackirYaw = pose.Orientation.Y;
                    trackirRoll = -pose.Orientation.Z;
                }
                // angular momentum
                else if (InputType == 2)
                {
                    // Cache values for torque mode
                    trackirPitch = -pose.Orientation.X;
                    trackirYaw = pose.Orientation.Y;
                    trackirRoll = -pose.Orientation.Z;
                }

                trackirThrust = (pose.PositionMeters.Z * -1f * ThrustScaler) + 0.2f;

                // moved applying the transformations to FixedUpdate so that movement isn't based on framerate
                m_staleDataDuration = 0.0f;
            }
            else
            {
                // Data was stale. If it's been stale for too long, smoothly recenter the camera.
                m_staleDataDuration += Time.deltaTime;

                if (m_staleDataDuration > TrackingLostTimeoutSeconds)
                {
                    // change to set pitch/yaw/roll/thrust to 0s
                    trackirThrust = 0;
                    trackirPitch = 0;
                    trackirYaw = 0;
                    trackirRoll = 0;
                }
            }
        }
    }
}
