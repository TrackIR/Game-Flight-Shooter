using UnityEngine;

public class TrackIRManager : MonoBehaviour
{
    public static TrackIRManager Instance {get; private set;}

    // TrackIR varibles
    public ushort AssignedApplicationId = 1001;     // Unique ID for application, used by TrackIR software
    public float TrackingLostTimeoutSeconds = 3.0f;     // Seconds till tracking is considered lost
    public float TrackingLostRecenterDurationSeconds = 1.0f;    // Seconds it takes to recenter after tracking is lost
    
    float m_staleDataDuration;      // How long since new tracking data
    private NaturalPoint.TrackIR.Client m_trackirClient;        // Helper class for interacting with TrackIR API

    public float HeadPitch { get; private set; }
    public float HeadYaw { get; private set; }
    public float HeadRoll { get; private set; }

    public float HeadXPos { get; private set; }
    public float HeadYPos { get; private set; }
    public float HeadZPos { get; private set; }     // Thrust for spaceship

    public bool IsTracking { get; private set; }


    private void Awake()
    {
        // singleton class
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

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
    
    
    void OnApplicationQuit()
    {
        ShutDownTrackIR();
    }

    private void ShutDownTrackIR()
    {
        if (m_trackirClient != null)
        {
            m_trackirClient.Disconnect();
            m_trackirClient = null;
        }
    }

    void Update()
    {
        UpdateTrackIR();
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

                HeadPitch = -pose.Orientation.X;
                HeadYaw = pose.Orientation.Y;
                HeadRoll = -pose.Orientation.Z;

                HeadXPos = pose.PositionMeters.X;
                HeadYPos = pose.PositionMeters.Y;
                HeadZPos = pose.PositionMeters.Z;

                m_staleDataDuration = 0.0f;
                IsTracking = true;
            }
            else
            {
                // Data was stale. If it's been stale for too long, smoothly recenter the camera.
                m_staleDataDuration += Time.deltaTime;

                if (m_staleDataDuration > TrackingLostTimeoutSeconds)
                {
                    HeadPitch = 0f;
                    HeadYaw = 0f;
                    HeadRoll = 0f;
                    HeadXPos = 0f;
                    HeadYPos = 0f;
                    HeadZPos = 0f;

                    IsTracking = false;
                }
            }
        }
    }
}
