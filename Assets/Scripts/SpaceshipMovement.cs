using UnityEngine;
using UnityEngine.InputSystem;

using System;
using System.Runtime.InteropServices;
using System.Text;

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
    private InputAction rollInput;
    private InputAction pitchInput;
    private InputAction yawInput;

    // Used to change the movement of the spaceship
    private float thrust;
    private float roll;
    private float pitch;
    private float yaw;


    // TrackIR varibles
    public UInt16 AssignedApplicationId = 1000;     // Unique ID for application, used by TrackIR software
    public float TrackingLostTimeoutSeconds = 3.0f;     // Seconds till tracking is considered lost
    public float TrackingLostRecenterDurationSeconds = 1.0f;    // Seconds it takes to recenter after tracking is lost
    float m_staleDataDuration;      // How long since new tracking data

    // TrackIR look directions
    private float thrustIR;
    private float rollIR;
    private float pitchIR;
    private float yawIR;
    NaturalPoint.TrackIR.Client m_trackirClient;        // Helper class for interacting with TrackIR API


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

            // adjust these values for feel
            rb.linearDamping = 0.25f;
            rb.angularDamping = 0.5f;
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
                
        thrust = thrustInput.ReadValue<float>();
        roll = rollInput.ReadValue<float>();
        pitch = pitchInput.ReadValue<float>();
        yaw = yawInput.ReadValue<float>();
    }

    // Use for physics operations
    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * thrust, ForceMode.Acceleration);

        // this order of angles is right
        Vector3 rotation = new(pitch, yaw, -roll);
        rb.AddRelativeTorque(rotation, ForceMode.Acceleration);

        rb.AddRelativeForce(Vector3.forward * thrustIR, ForceMode.Acceleration);

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
                pitchIR = -pose.Orientation.X;
                yawIR = pose.Orientation.Y;
                rollIR = -pose.Orientation.Z;

                thrustIR = (Cubed(pose.PositionMeters.Z) * -50) + 0.2f;     //play with scalars to improve feel
                // print(-pose.PositionMeters.Z);                           //use as reference of raw values

                m_staleDataDuration = 0.0f;
            }
            else
            {
                // Data was stale. If it's been stale for too long, smoothly recenter the camera.
                m_staleDataDuration += Time.deltaTime;

                if (m_staleDataDuration > TrackingLostTimeoutSeconds)
                {
                    // change to set pitch/yaw/roll to 0s?

                    pitchIR = 0;
                    yawIR = 0;
                    rollIR = 0;

                    thrustIR = 0;
                }
            }
        }
    }

    private float Cubed(float a)
    {
        return a * a * a;
    }

}


