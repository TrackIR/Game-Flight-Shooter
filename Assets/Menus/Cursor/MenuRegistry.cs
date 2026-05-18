using UnityEngine;
using UnityEngine.UIElements;

public class MenuRegistry : MonoBehaviour
{
    [SerializeField] private UIDocument[] menus;

    private void Start()
    {
        var cursorInput = FindObjectsByType<CursorInput>(FindObjectsSortMode.None)[0];
        cursorInput.SetMenus(menus);
    }
}
