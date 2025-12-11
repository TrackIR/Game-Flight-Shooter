using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform thirdPersonPivot;
    public Transform firstPersonPivot;

    private bool isFirstPerson = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
        }

        if (isFirstPerson)
        {
            transform.position = firstPersonPivot.position;
            transform.rotation = firstPersonPivot.rotation;
        }
        else
        {
            transform.position = thirdPersonPivot.position;
            transform.rotation = thirdPersonPivot.rotation;
        }
    }
}
