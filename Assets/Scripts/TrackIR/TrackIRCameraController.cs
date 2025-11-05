using UnityEngine;

public class TrackIRCameraController : MonoBehaviour
{
    public TrackIRBridge trackIR;
    public float yawMultiplier = 1f;
    public float pitchMultiplier = 1f;
    public float maxPitch = 30f;

    void Update()
    {
        if (trackIR == null) return;


        Vector3 rot = trackIR.rotationDeg;

        float yaw = rot.y * yawMultiplier;
        float pitch = rot.x * pitchMultiplier;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(-pitch, yaw, 0f);
    }
}
