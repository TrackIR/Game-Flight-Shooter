using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public float minPitch = -70f;
    public float maxPitch = 70f;

    float yaw;
    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse.current can be null if no mouse is present
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        // note: delta is in pixels per frame, so multiply by sensitivity * Time.deltaTime
        yaw += delta.x * mouseSensitivity * Time.deltaTime;
        pitch -= delta.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);

        // we can unlock with Esc
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
