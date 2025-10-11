using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVSpotlightController : MonoBehaviour
{
    [Header("Flashlight Reference")]
    public Light uvFlashlight;

    [Header("Glow Settings")]
    public Color glowColor = new Color(0.5f, 0f, 1f);
    [Range(0f, 20f)] public float baseIntensity = 10f;
    [Range(0.5f, 30f)] public float range = 10f;

    [Header("Wall Spread Settings")]
    public float minDistance = 0.5f;  // Distance at which beam is widest
    public float maxDistance = 5f;    // Normal beam range
    public float maxSpreadMultiplier = 2f; // How wide cone gets when close
    public float closeIntensityMultiplier = 0.6f; // Slight dim when near wall

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_GlowColor", glowColor);
        mat.SetFloat("_Range", range);
        mat.SetFloat("_Intensity", baseIntensity);
    }

    void Update()
    {
        if (uvFlashlight == null) return;

        // Default multipliers
        float spreadMultiplier = 1f;
        float intensityMultiplier = 1f;

        // ✅ Raycast forward from the flashlight to detect walls
        if (Physics.Raycast(uvFlashlight.transform.position, uvFlashlight.transform.forward, out RaycastHit hit, maxDistance))
        {
            float dist = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            float t = 1f - Mathf.InverseLerp(minDistance, maxDistance, dist);

            spreadMultiplier = Mathf.Lerp(1f, maxSpreadMultiplier, t);
            intensityMultiplier = Mathf.Lerp(1f, closeIntensityMultiplier, t);
        }

        // Update shader parameters
        mat.SetVector("_LightPos", uvFlashlight.transform.position);
        mat.SetVector("_LightDir", uvFlashlight.transform.forward);

        float coneAngle = uvFlashlight.spotAngle * 0.5f * Mathf.Deg2Rad * spreadMultiplier;
        mat.SetFloat("_ConeAngle", coneAngle);
        mat.SetFloat("_Intensity", baseIntensity * intensityMultiplier);
    }
}
