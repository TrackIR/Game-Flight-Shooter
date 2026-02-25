using UnityEngine;

public class FullScreenController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Screen.fullScreen = !Screen.fullScreen;
    }
}
