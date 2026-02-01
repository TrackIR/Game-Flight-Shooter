using UnityEngine;

public class CameraModeController : MonoBehaviour
{
    public Transform thirdPersonPivot;
    public Transform firstPersonPivot;

    public GameObject exteriorShipModel;
    public GameObject cockpitModel;

    public GameObject cameraHolder;

    private bool inFirstPerson = false;

    public static bool toggleCameraFlag = false;

    void Update()
    {
        // Press C to toggle camera mode
        if (Input.GetKeyDown(KeyCode.C) || toggleCameraFlag)
        {
            toggleCameraFlag = false;
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
            cameraHolder.transform.position = firstPersonPivot.position;
            cameraHolder.transform.rotation = firstPersonPivot.rotation;
        }
        else
        {
            cameraHolder.transform.position = thirdPersonPivot.position;
            cameraHolder.transform.rotation = thirdPersonPivot.rotation;
        }
    }
}
