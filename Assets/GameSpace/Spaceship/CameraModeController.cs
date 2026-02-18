using System.Linq.Expressions;
using UnityEngine;

public class CameraModeController : MonoBehaviour
{
    public Transform thirdPersonPivot;
    public Transform firstPersonPivot;

    public GameObject exteriorShipModel;
    public GameObject cockpitModel;

    public GameObject cameraHolder;

    private bool inFirstPerson;

    void Start()
    {
        if (PlayerPrefs.GetInt("povFrst") == 1)
        {
            inFirstPerson = true;
            exteriorShipModel.SetActive(false);
            cockpitModel.SetActive(true);
        }
        else
        {
            inFirstPerson = false;
            exteriorShipModel.SetActive(true);
            cockpitModel.SetActive(false);
        }
    }

    void Update()
    {
        // Press C to toggle camera mode
        if (Input.GetKeyDown(KeyCode.C))
            ToggleCameraMode();

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
