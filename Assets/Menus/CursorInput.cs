using UnityEngine;
using UnityEngine.UIElements;

public class CursorInput : MonoBehaviour
{
    public RectTransform cursorTransform;
    public UIDocument[] allMenus;
    private UIDocument activeUIDocument;
    private VisualElement lastHovered;
    private VisualElement pickedElement;
    private float nullElementTimer = 0.25f;

    void Update()
    {
        // select the correct UI Document
        foreach (var menu in allMenus)
        {
            if (menu.isActiveAndEnabled)
            {
                activeUIDocument = menu;
                break;
            }
        }
        if (activeUIDocument == null)
            return;

        // UI toolkit uses top left as origin instead of bottom left
        Vector2 screenPos = cursorTransform.position;
        screenPos.y = Screen.height - screenPos.y;

        IPanel panel = activeUIDocument.rootVisualElement.panel;
        Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(panel, screenPos);

        //find the element under the cursor + radius
        VisualElement newPickedElement = PickRadius(panel, panelPos, 30f);


        // 0.25 seconds of nullElements before deselecting a button
        if (newPickedElement != null)
        {
            pickedElement = newPickedElement;
            nullElementTimer = 0.25f;
        }
        else
        {
            if(nullElementTimer < 0f)
                pickedElement = newPickedElement;

            nullElementTimer -= Time.deltaTime;
        }

        // hover effects
        HandleHover(pickedElement);

        // click button under cursor when hit spacebar
        if (Input.GetKeyDown(KeyCode.Space) && pickedElement != null)
        {
            using (var clickEvt = NavigationSubmitEvent.GetPooled())
            {
                clickEvt.target = pickedElement;
                pickedElement.SendEvent(clickEvt);
            }
        }
    }

    private VisualElement PickRadius(IPanel panel, Vector2 center, float radius)
    {
        Vector2[] pointsToQuery = new Vector2[]
        {
            center,
            center + new Vector2(0, radius),
            center + new Vector2(0, -radius),
            center + new Vector2(radius, 0),
            center + new Vector2(-radius, 0),
            center + new Vector2(radius/2, radius/2),
            center + new Vector2(radius/2, -radius/2),
            center + new Vector2(-radius/2, radius/2),
            center + new Vector2(-radius/2, -radius/2)
        };

        foreach (var point in pointsToQuery)
        {
            var element = panel.Pick(point);

            if (element != null && element is Button)
                return element;
        }
        return null;
    }

    private void HandleHover(VisualElement currElement)
    {
        if (currElement == lastHovered)
            return;

        if (lastHovered != null)
        {
            using (var leaveEvt = PointerLeaveEvent.GetPooled())
            {
                leaveEvt.target = lastHovered;
                lastHovered.SendEvent(leaveEvt);
            }
        }

        if (currElement != null)
        {
            using (var enterEvt = PointerEnterEvent.GetPooled())
            {
                enterEvt.target = currElement;
                currElement.SendEvent(enterEvt);
            }
        }

        lastHovered = currElement;
    }
}
