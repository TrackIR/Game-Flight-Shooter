using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform thirdPersonPivot;
    public Transform firstPersonPivot;

    public static bool isFirstPerson;

    void Start()
    {
        isFirstPerson = PlayerPrefs.GetInt("povFrst") == 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
        }

        if (isFirstPerson)
        {
            this.transform.position = firstPersonPivot.position;
            this.transform.rotation = firstPersonPivot.rotation;
            Debug.Log("is first person!");
        }
        else
        {
            this.transform.position = thirdPersonPivot.position;
            this.transform.rotation = thirdPersonPivot.rotation;
            Debug.Log("not first person...");
        }
    }
}
