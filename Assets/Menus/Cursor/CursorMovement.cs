using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    public RectTransform cursorTransform;

    private Vector2 cursorPos;
    private float vertical;
    private float horizontal;    

    // TrackIR varibles
    public ushort AssignedApplicationId = 1000;     // Unique ID for application, used by TrackIR software
    public float TrackingLostTimeoutSeconds = 3.0f;     // Seconds till tracking is considered lost
    public float TrackingLostRecenterDurationSeconds = 1.0f;    // Seconds it takes to recenter after tracking is lost
    float m_staleDataDuration;      // How long since new tracking data

    NaturalPoint.TrackIR.Client m_trackirClient;        // Helper class for interacting with TrackIR API


    void Start()
    {
    #if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    #endif
    
        cursorPos = cursorTransform.anchoredPosition;

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
            m_trackirClient.Disconnect();
    }

    void Update()
    {
        UpdateTrackIR();

        // deadzone
        if (Mathf.Abs(horizontal) < 0.075f)
            horizontal = 0f;
        if (Mathf.Abs(vertical) < 0.075f)
            vertical = 0f;

        cursorPos += new Vector2(horizontal, vertical) * Time.deltaTime * 1000;
        
        RectTransform canvas = cursorTransform.root as RectTransform;

        Vector2 min = canvas.rect.min;
        Vector2 max = canvas.rect.max;

        cursorPos.x = Mathf.Clamp(cursorPos.x, min.x, max.x);
        cursorPos.y = Mathf.Clamp(cursorPos.y, min.y, max.y);

        cursorTransform.anchoredPosition = cursorPos;
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

                vertical = pose.Orientation.X;
                horizontal = pose.Orientation.Y;


                m_staleDataDuration = 0.0f;
            }
            else
            {
                // Data was stale. If it's been stale for too long, smoothly recenter the camera.
                m_staleDataDuration += Time.deltaTime;

                if (m_staleDataDuration > TrackingLostTimeoutSeconds)
                {
                   vertical = 0f;
                   horizontal = 0f;
                }
            }
        }
    }
}
