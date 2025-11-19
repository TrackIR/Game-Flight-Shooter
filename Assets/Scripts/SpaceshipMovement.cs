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


    // Used to change the movement of the spaceship, keyboard controls
    private float thrustK;
    private float pitchK;
    private float yawK;
    private float rollK;


    // Spaceship feel values
    public float MaxSpeed = 4.25f;
    public float LinearDamping = 0.5f;
    public float AngularDamping = 0.5f;

    public float ThrustIRScalar = 50.0f;
    public float PitchIRScalar = 1.0f;
    public float YawIRScalar = 1.3f;
    public float RollIRScalar = 1.1f;


    // TrackIR varibles
    public UInt16 AssignedApplicationId = 1000;     // Unique ID for application, used by TrackIR software
    public float TrackingLostTimeoutSeconds = 3.0f;     // Seconds till tracking is considered lost
    public float TrackingLostRecenterDurationSeconds = 1.0f;    // Seconds it takes to recenter after tracking is lost
    float m_staleDataDuration;      // How long since new tracking data

    // TrackIR look directions
    private float thrustIR;
    private float pitchIR;
    private float yawIR;
    private float rollIR;

    NaturalPoint.TrackIR.Client m_trackirClient;        // Helper class for interacting with TrackIR API

    private void Awake()
    {
        spaceshipControls = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        thrustInput = spaceshipControls.Player.Thrust;
        thrustInput.Enable();

        pitchInput = spaceshipControls.Player.Pitch;
        pitchInput.Enable();

        yawInput = spaceshipControls.Player.Yaw;
        yawInput.Enable();

        rollInput = spaceshipControls.Player.Roll;
        rollInput.Enable();
    }

    private void OnDisable()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;

            // Adjust these values for feel
            rb.linearDamping = LinearDamping;
            rb.angularDamping = AngularDamping;
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
        UpdateTrackIR();
                
        thrustK = thrustInput.ReadValue<float>();
        pitchK = pitchInput.ReadValue<float>();
        yawK = yawInput.ReadValue<float>();
        rollK = rollInput.ReadValue<float>();

    }

    // Use for physics operations (is called at fixed time intervals not every frame)
    void FixedUpdate()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        print(currentSpeed);

        // Thrust (keyboard then TrackIR)
        if (currentSpeed < MaxSpeed)
        {
            rb.AddRelativeForce(Vector3.forward * thrustK, ForceMode.Acceleration);
            rb.AddRelativeForce(Vector3.forward * thrustIR, ForceMode.Acceleration);
        }

        // Keyboard controls
        Vector3 rotation = new(pitchK, yawK, -rollK);
        rb.AddRelativeTorque(rotation, ForceMode.Acceleration);

        // TrackIR controls
        Vector3 rotationIR = new(pitchIR, yawIR, rollIR);
        rb.AddRelativeTorque(rotationIR, ForceMode.Acceleration);
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
                // New data was available, apply it directly here.
                // Play with scalars to improve feel
                pitchIR = -pose.Orientation.X * PitchIRScalar;
                yawIR = pose.Orientation.Y * YawIRScalar;
                rollIR = -pose.Orientation.Z * RollIRScalar;

                thrustIR = (pose.PositionMeters.Z * -1 * ThrustIRScalar) + 0.2f;

                m_staleDataDuration = 0.0f;
            }
            else
            {
                // Data was stale. If it's been stale for too long, smoothly recenter the camera.
                m_staleDataDuration += Time.deltaTime;

                if (m_staleDataDuration > TrackingLostTimeoutSeconds)
                {
                    // change to set pitch/yaw/roll/thrust to 0s
                    thrustIR = 0;
                    pitchIR = 0;
                    yawIR = 0;
                    rollIR = 0;
                }
            }
        }
    }
}
