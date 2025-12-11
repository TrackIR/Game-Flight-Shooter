using UnityEngine;

public class CameraModeController : MonoBehaviour
{
    public Transform thirdPersonPivot;
    public Transform firstPersonPivot;

    public GameObject exteriorShipModel;
    public GameObject cockpitModel;

    public Camera mainCamera;

    private bool inFirstPerson = false;

    void Update()
    {
        // Press C to toggle camera mode
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCameraMode();
        }

        UpdateCameraPosition();
    }

    void ToggleCameraMode()
    {
        inFirstPerson = !inFirstPerson;

        // Show/hide models
        exteriorShipModel.SetActive(!inFirstPerson);
        cockpitModel.SetActive(inFirstPerson);
    }

    void UpdateCameraPosition()
    {
        if (inFirstPerson)
        {
            mainCamera.transform.position = firstPersonPivot.position;
            mainCamera.transform.rotation = firstPersonPivot.rotation;
        }
        else
        {
            mainCamera.transform.position = thirdPersonPivot.position;
            mainCamera.transform.rotation = thirdPersonPivot.rotation;
        }
    }
}
