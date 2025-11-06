using UnityEngine;

public class ThirdPersonCameraMovement : MonoBehaviour
{

    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float minSpeed = 1f;
    public float sideSpeed = 5f;
    public float smoothing = 5f;

    public float neutralZone = 5f;
    public float pitchSensitivityDown = 2.0f;
    public float pitchSensitivityUp = 0.5f;
    public float yawSensitivity = 1.0f;

    private float currentSpeed = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    void Update()
    {
        float pitch = transform.eulerAngles.x;
        float yaw = transform.eulerAngles.y;

        if (pitch > 180f) pitch -= 360f;
        if (yaw > 180f) yaw -= 360f;

        float targetSpeed;

        if (Mathf.Abs(pitch) <= neutralZone)
        {
            targetSpeed = minSpeed;
        }
        else
        {
            float effectivePitch = Mathf.Abs(pitch) - neutralZone;
            float normalizedPitch = Mathf.Clamp(effectivePitch / (90f - neutralZone), 0f, 1f);

            if (pitch < 0)
            {
                targetSpeed = Mathf.Lerp(minSpeed, maxSpeed * 0.5f, normalizedPitch * pitchSensitivityUp);
            }
            else
            {
                targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, normalizedPitch * pitchSensitivityDown);
            }
        }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        float sidewaysSpeed = 0f;
        if (Mathf.Abs(yaw) > neutralZone)
        {
            float effectiveYaw = (Mathf.Abs(yaw) - neutralZone) * Mathf.Sign(yaw);
            float normalizedYaw = Mathf.Clamp(effectiveYaw / (90f - neutralZone), -1f, 1f);
            sidewaysSpeed = normalizedYaw * sideSpeed * yawSensitivity;
        }

        Vector3 moveDir = (transform.forward * currentSpeed) + (transform.right * sidewaysSpeed);
        currentVelocity = Vector3.Lerp(currentVelocity, moveDir, smoothing * Time.deltaTime);

        transform.position += currentVelocity * Time.deltaTime;
    }
}
