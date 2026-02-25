using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    public RectTransform cursorTransform;

    private Vector2 cursorPos;
    private float vertical;
    private float horizontal;    

    void Start()
    {
    #if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    #endif
    
        cursorPos = cursorTransform.anchoredPosition;
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (TrackIRManager.Instance != null && TrackIRManager.Instance.IsTracking)
        {
            horizontal = TrackIRManager.Instance.HeadYaw;
            vertical = -TrackIRManager.Instance.HeadPitch;
        }

        // deadzone
        if (Mathf.Abs(horizontal) < 0.075f)
            horizontal = 0f;
        if (Mathf.Abs(vertical) < 0.075f)
            vertical = 0f;

        cursorPos += new Vector2(horizontal, vertical) * Time.deltaTime * 1000;
        
        RectTransform canvas = cursorTransform.root as RectTransform;

        cursorPos.x = Mathf.Clamp(cursorPos.x, canvas.rect.min.x, canvas.rect.max.x);
        cursorPos.y = Mathf.Clamp(cursorPos.y, canvas.rect.min.y, canvas.rect.max.y);

        cursorTransform.anchoredPosition = cursorPos;
    }
}
