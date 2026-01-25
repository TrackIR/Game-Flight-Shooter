using UnityEngine;
using UnityEngine.UI;

public class DamageFlashEffect : MonoBehaviour
{
    public Image flashImage;
    public float flashSpeed = 2f;
    public float maxAlpha = 0.8f;

    private Color flashColor;
    private bool flashActive = false;

    void Start()
    {
        flashColor = flashImage.color;
        flashColor.a = 0;
        flashImage.color = flashColor;
    }

    void Update()
    {
        if (!flashActive) return;

        flashColor.a = Mathf.Lerp(flashColor.a, 0f, flashSpeed * Time.deltaTime);
        flashImage.color = flashColor;

        if (flashColor.a <= 0.01f)
        {
            flashColor.a = 0f;
            flashImage.color = flashColor;
            flashActive = false;
        }
    }

    public void Flash()
    {
        flashColor.a = maxAlpha;
        flashImage.color = flashColor;
        flashActive = true;
    }
}
