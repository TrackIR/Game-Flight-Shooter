using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class CursorInput : MonoBehaviour
{
    public RectTransform cursorTransform;
    public UIDocument uiDocument;

    VisualElement root;
    int pointerId = 0;

    void Awake()
    {
        root = uiDocument.rootVisualElement;
    }

    void Update()
    {
        Vector2 screenPos = cursorTransform.position;

        SendPointerMove(screenPos);

        if (Input.GetKeyDown(KeyCode.Space))
            SendPointerDown(screenPos);

        if (Input.GetKeyUp(KeyCode.Space))
            SendPointerUp(screenPos);
    }

    void SendPointerMove(Vector2 pos)
    {
        // var evt = new PointerMoveEvent();
        // evt.Init(
        //     pointerId,
        //     pos,
        //     Vector2.zero,
        //     0,
        //     PointerType.mouse,
        //     0
        // );

        using (var moveEvt = PointerMoveEvent.GetPooled(
            pointerId,
            screenPos,
            Vector2.zero
        ))
        {
            root.SendEvent(moveEvt);
        }
    }

    void SendPointerDown(Vector2 pos)
    {
        // var evt = new PointerDownEvent();
        // evt.Init(
        //     pointerId,
        //     pos,
        //     Vector2.zero,
        //     0,
        //     PointerType.mouse,
        //     0
        // );

        using (var downEvt = PointerMoveEvent.GetPooled(
            pointerId,
            screenPos,
            0
        ))
        {
            root.SendEvent(downEvt);
        }

    }

    void SendPointerUp(Vector2 pos)
    {
        // var evt = PointerUpEvent();
        // evt.Init(
        //     pointerId,
        //     pos,
        //     Vector2.zero,
        //     0,
        //     PointerType.mouse,
        //     0
        // );
        using (var upEvt = PointerUpEvent.GetPooled(
            pointerId,
            screenPos,
            0
        ))
        {
            root.SendEvent(evt);
        }
    }
}
