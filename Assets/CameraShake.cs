using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        Quaternion originalRot = transform.localRotation;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float xPos = Random.Range(-1f, 1f) * magnitude;
            float yPos = Random.Range(-1f, 1f) * magnitude;
            float yRot = Random.Range(-20f, 20f) * magnitude;
            float zRot = Random.Range(-20f, 20f) * magnitude;

            transform.localPosition = new Vector3(xPos, yPos, originalPos.z);
            transform.Rotate(originalRot.x, yRot, zRot);

            elapsed += Time.deltaTime;

            yield return null;
        }
    
        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }
}
