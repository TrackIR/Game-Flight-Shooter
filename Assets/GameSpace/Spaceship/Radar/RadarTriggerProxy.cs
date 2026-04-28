using UnityEngine;

public class TriggerProxy : MonoBehaviour
{
    [SerializeField] private SpaceshipRadar parentScript;

    private void OnTriggerEnter(Collider other) 
    {
        parentScript.HandleTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other) 
    {
        parentScript.HandleTriggerExit(other);
    }
}
