using UnityEngine;

public class ApplySavedColors : MonoBehaviour
{
    public enum TargetType { Spaceship, Asteroids }
    public TargetType target = TargetType.Spaceship;

    [Tooltip("If empty, will auto-find renderers in children.")]
    public Renderer[] renderers;

    private const string ShipKey = "ShipColor";
    private const string AstKey  = "AstColor";

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>(true);

        Color c = (target == TargetType.Spaceship)
            ? LoadColor(ShipKey, Color.white)
            : LoadColor(AstKey,  Color.white);

        ApplyToAllRenderers(c);
    }

    private void ApplyToAllRenderers(Color c)
    {
        foreach (var r in renderers)
        {
            if (r == null) continue;

            // material creates an instance (OK for per-object color)
            var mat = r.material;

            if (mat.HasProperty("_BaseColor"))
                mat.SetColor("_BaseColor", c);   // URP/HDRP
            else if (mat.HasProperty("_Color"))
                mat.SetColor("_Color", c);       // Built-in
        }
    }

    private static Color LoadColor(string keyPrefix, Color fallback)
    {
        if (!PlayerPrefs.HasKey(keyPrefix + "_R")) return fallback;

        float r = PlayerPrefs.GetFloat(keyPrefix + "_R", fallback.r);
        float g = PlayerPrefs.GetFloat(keyPrefix + "_G", fallback.g);
        float b = PlayerPrefs.GetFloat(keyPrefix + "_B", fallback.b);
        return new Color(r, g, b, 1f);
    }
}
