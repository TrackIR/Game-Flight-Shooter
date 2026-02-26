using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplySavedColors : MonoBehaviour
{
    // Allows ColorMenu to tell all ApplySavedColors instances to refresh.
    public static event Action ColorsChanged;
    public static void NotifyColorsChanged() => ColorsChanged?.Invoke();

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");      // URP Lit
    private static readonly int ColorId     = Shader.PropertyToID("_Color");         // Built-in fallback
    private static readonly int EmissionId  = Shader.PropertyToID("_EmissionColor"); // URP/Built-in

    [Header("Target (optional)")]
    [SerializeField] private GameObject target;

    [Header("Renderers (optional)")]
    [Tooltip("If empty, script will auto-find renderers under Target (or this GameObject).")]
    [SerializeField] private List<Renderer> renderers = new List<Renderer>();

    [Header("Material Filtering")]
    [Tooltip("Only apply to materials whose name contains this text. Leave empty to apply to all materials on these renderers.")]
    [SerializeField] private string materialNameContains = "";

    [Tooltip("If true, edits sharedMaterial (all users of that material). If false, edits material instances per renderer.")]
    [SerializeField] private bool useSharedMaterial = true;

    [Header("Which color to apply on this object?")]
    [SerializeField] private bool applySpaceshipColor = false;
    [SerializeField] private bool applyAsteroidColor = false;

    [Header("Emission Sync")]
    [SerializeField] private bool syncEmissionToBaseColor = true;

    [SerializeField, Min(0f)] private float spaceshipEmissionIntensity = 2f;
    [SerializeField, Min(0f)] private float asteroidEmissionIntensity = 2f;

    // Keys we support (new + old)
    private const string ShipPrefix = "ShipColor";
    private static readonly string[] AsteroidPrefixes = { "AstColor", "AsteroidColor" };

    private void Awake() => AutoCollectRenderersIfNeeded();

    private void OnEnable()  => ColorsChanged += ApplyNow;
    private void OnDisable() => ColorsChanged -= ApplyNow;

    private void Start() => ApplyNow();

    private void OnValidate()
    {
        if (Application.isPlaying)
            ApplyNow();
    }

    public void ApplyNow()
    {
        AutoCollectRenderersIfNeeded();
        if (renderers == null || renderers.Count == 0) return;

        if (applySpaceshipColor)
        {
            var shipColor = LoadColorAny(new[] { ShipPrefix }, Color.white);
            ApplyColorToRenderers(shipColor, spaceshipEmissionIntensity);
        }

        if (applyAsteroidColor)
        {
            var astColor = LoadColorAny(AsteroidPrefixes, Color.white);
            ApplyColorToRenderers(astColor, asteroidEmissionIntensity);
        }
    }

    private void AutoCollectRenderersIfNeeded()
    {
        if (renderers != null && renderers.Count > 0) return;
        var root = target != null ? target.transform : transform;
        renderers = new List<Renderer>(root.GetComponentsInChildren<Renderer>(true));
    }

    private void ApplyColorToRenderers(Color color, float intensity)
    {
        foreach (var r in renderers)
        {
            if (r == null) continue;

            var mats = useSharedMaterial ? r.sharedMaterials : r.materials;
            if (mats == null) continue;

            for (int i = 0; i < mats.Length; i++)
            {
                var m = mats[i];
                if (m == null) continue;

                if (!string.IsNullOrEmpty(materialNameContains) && !m.name.Contains(materialNameContains))
                    continue;

                // --- Base color (tint) ---
                bool setBase = false;

                if (m.HasProperty(BaseColorId))
                {
                    var curA = m.GetColor(BaseColorId).a;
                    m.SetColor(BaseColorId, new Color(color.r, color.g, color.b, curA));
                    setBase = true;
                }
                else if (m.HasProperty(ColorId))
                {
                    var curA = m.GetColor(ColorId).a;
                    m.SetColor(ColorId, new Color(color.r, color.g, color.b, curA));
                    setBase = true;
                }

                // --- Emission matches base color ---
                if (setBase && syncEmissionToBaseColor && m.HasProperty(EmissionId))
                {
                    // Multiply by intensity by 4
                    var emissive = new Color(color.r, color.g, color.b, 1f) * intensity;
                    m.SetColor(EmissionId, emissive);

                    // Ensure emission is actually enabled on the shader
                    m.EnableKeyword("_EMISSION");
                }
            }
        }
    }

    private Color LoadColorAny(string[] prefixes, Color fallback)
    {
        foreach (var p in prefixes)
            if (TryLoadColor(p, out var c)) return c;
        return fallback;
    }

    private bool TryLoadColor(string prefix, out Color color)
    {
        string rKey = prefix + "_R";
        string gKey = prefix + "_G";
        string bKey = prefix + "_B";

        if (!PlayerPrefs.HasKey(rKey) || !PlayerPrefs.HasKey(gKey) || !PlayerPrefs.HasKey(bKey))
        {
            color = default;
            return false;
        }

        float r = ReadChannel01(rKey);
        float g = ReadChannel01(gKey);
        float b = ReadChannel01(bKey);

        color = new Color(r, g, b, 1f);
        return true;
    }

    private float ReadChannel01(string key)
    {
        int i = PlayerPrefs.GetInt(key, int.MinValue);
        float f = PlayerPrefs.GetFloat(key, float.NaN);

        if (i != int.MinValue && i >= 0 && i <= 255)
            return Mathf.Clamp01(i / 255f);

        if (!float.IsNaN(f))
        {
            if (f >= 0f && f <= 1.5f) return Mathf.Clamp01(f);
            if (f > 1.5f) return Mathf.Clamp01(f / 255f);
        }

        return 1f;
    }
}