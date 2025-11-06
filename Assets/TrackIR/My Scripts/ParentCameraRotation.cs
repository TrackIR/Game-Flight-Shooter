using UnityEngine;

public class ParentCameraRotation : MonoBehaviour
{


    public Transform childCamera;

    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        if (childCamera == null)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, childCamera.rotation, rotationSpeed * Time.deltaTime);
    }

}