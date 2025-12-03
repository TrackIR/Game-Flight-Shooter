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
    public int InputType;       // 0 = keyboard, 1 = match head rotation, 2 = angular momentum

    // Used to change the movement of the spaceship, keyboard controls
    private float thrust;
    private float pitch;
    private float yaw;
    private float roll;


    // Spaceship feel values
    public float MaxSpeed = 4.25f;
    public float LinearDamping = 0.5f;
    public float AngularDamping = 0.5f;

    public float ThrustScalar = 50.0f;
    public float PitchScalar = 1.0f;
    public float YawScalar = 1.3f;
    public float RollScalar = 1.1f;


    // TrackIR varibles
    public UInt16 AssignedApplicationId = 1000;     // Unique ID for application, used by TrackIR software
    public float TrackingLostTimeoutSeconds = 3.0f;     // Seconds till tracking is considered lost
    public float TrackingLostRecenterDurationSeconds = 1.0f;    // Seconds it takes to recenter after tracking is lost
    float m_staleDataDuration;      // How long since new tracking data



    NaturalPoint.TrackIR.Client m_trackirClient;        // Helper class for interacting with TrackIR API

    private void Awake()
    {
        spaceshipControls = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        if (InputType == 0)
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
    }

    private void OnDisable()
    {
        if (InputType == 0)
        {
            thrustInput.Disable();
            pitchInput.Disable();
            yawInput.Disable();
            rollInput.Disable();
        }
    }

    void OnApplicationQuit()
    {
        if(InputType != 0)
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

        InputType = MainMenuUI.controlType;

        if (InputType != 0)
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
        if (InputType != 0)
            UpdateTrackIR();
        else
        {
            thrust = thrustInput.ReadValue<float>() * ThrustScalar;
            pitch = pitchInput.ReadValue<float>();
            yaw = yawInput.ReadValue<float>();
            roll = -rollInput.ReadValue<float>();
        }
    }

    // Use for physics operations (is called at fixed time intervals not every frame)
    void FixedUpdate()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        // print(currentSpeed);

        // Thrust, if less than max speed
        if (currentSpeed < MaxSpeed)
            rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Acceleration);

        // Rotation
        // head match rotation
        if (InputType == 1)
        {
            transform.Rotate(pitch, yaw, roll);
        }
        //key board and angular momentum
        else
        {
            Vector3 rotation = new(pitch, yaw, roll);
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
                // New data was available, apply it directly here.

                // head match rotation
                if (InputType == 1)
                {
                    pitch = -pose.Orientation.X;
                    yaw = pose.Orientation.Y;
                    roll = -pose.Orientation.Z;
                }
                // angular momentum
                else if (InputType == 2)
                {
                    // Play with scalars to improve feel
                    pitch = -pose.Orientation.X * PitchScalar;
                    yaw = pose.Orientation.Y * YawScalar;
                    roll = -pose.Orientation.Z * RollScalar;
                }

                thrust = (pose.PositionMeters.Z * -1 * ThrustScalar) + 0.2f;

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
                    thrust = 0;
                    pitch = 0;
                    yaw = 0;
                    roll = 0;
                }
            }
        }
    }
}
