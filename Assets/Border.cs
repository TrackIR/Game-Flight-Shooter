using UnityEngine;
using System;
using System.Collections.Generic;

public class Border : MonoBehaviour
{
    private Renderer objectRenderer;
    private float borderSize = 125.0f; // could calculate this automatically
    private float threshold = 100.0f;

    private enum BorderType
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    [SerializeField] private BorderType borderType;
    [SerializeField] private GameObject ship;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    // This probably shouldn't be called once per frame, but im lazy right now cause finals week
    void Update()
    {
        // Check if player position has passed a certain threshold range
        // If yes, then start calculating player distance based on which threshold they crossed
        switch (borderType)
        {
            case BorderType.Top:
                if (ship.transform.position.y >= threshold)
                    SetOpacity(((Math.Abs(ship.transform.position.y) - threshold) / (borderSize - threshold)) / 4.0f);
                else
                    SetOpacity(0.0f);
                break;
            
            case BorderType.Bottom:
                if (ship.transform.position.y <= -threshold)
                    SetOpacity(((Math.Abs(ship.transform.position.y) - threshold) / (borderSize - threshold)) / 4.0f);
                else
                    SetOpacity(0.0f);
                break;
            
            case BorderType.Left:
                if (ship.transform.position.x <= -threshold)
                    SetOpacity(((Math.Abs(ship.transform.position.x) - threshold) / (borderSize - threshold)) / 4.0f);
                else
                    SetOpacity(0.0f);
                break;
            
            case BorderType.Right:
                if (ship.transform.position.x >= threshold)
                    SetOpacity(((Math.Abs(ship.transform.position.x) - threshold) / (borderSize - threshold)) / 4.0f);
                else
                    SetOpacity(0.0f);
                break;
            
            case BorderType.Front:
                if (ship.transform.position.z >= threshold)
                    SetOpacity(((Math.Abs(ship.transform.position.z) - threshold) / (borderSize - threshold)) / 4.0f);
                else
                    SetOpacity(0.0f);
                break;
            
            case BorderType.Back:
                if (ship.transform.position.z <= -threshold)
                    SetOpacity(((Math.Abs(ship.transform.position.z) - threshold) / (borderSize - threshold)) / 4.0f);
                else
                    SetOpacity(0.0f);
                break;
        }
    }

    public void SetOpacity(float alphaValue)
    {
        Color color = objectRenderer.material.color;
        color.a = alphaValue;
        objectRenderer.material.color = color;
    }
}
