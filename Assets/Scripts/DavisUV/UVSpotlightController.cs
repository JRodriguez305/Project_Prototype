using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVSpotlightController : MonoBehaviour
{
    [Header("Flashlight Reference")]
    public Light uvFlashlight;

    [Header("Glow Settings")]
    public Color glowColor = new Color(0.6f, 0.3f, 1f);
    [Range(0f, 20f)] public float baseIntensity = 10f;
    [Range(0.5f, 30f)] public float range = 10f;

    [Header("Wall Spread Settings")]
    public float minDistance = 0.5f;
    public float maxDistance = 5f;
    public float maxSpreadMultiplier = 2f;
    public float closeIntensityMultiplier = 0.6f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_GlowColor", glowColor);
        mat.SetFloat("_MaxRange", range);
        mat.SetFloat("_GlowIntensity", baseIntensity);
        mat.SetFloat("_LightOn", 0f);
    }

    void Update()
    {
        if (uvFlashlight == null)
            return;

        if (!uvFlashlight.enabled)
        {
            mat.SetFloat("_LightOn", 0f);
            return;
        }

        mat.SetFloat("_LightOn", 1f);
        mat.SetVector("_LightPos", uvFlashlight.transform.position);
        mat.SetVector("_LightDir", uvFlashlight.transform.forward);

        float spreadMultiplier = 1f;
        float intensityMultiplier = 1f;

        if (Physics.Raycast(uvFlashlight.transform.position, uvFlashlight.transform.forward, out RaycastHit hit, maxDistance))
        {
            float dist = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            float t = 1f - Mathf.InverseLerp(minDistance, maxDistance, dist);
            spreadMultiplier = Mathf.Lerp(1f, maxSpreadMultiplier, t);
            intensityMultiplier = Mathf.Lerp(1f, closeIntensityMultiplier, t);
        }

        float coneAngle = uvFlashlight.spotAngle * 0.5f * Mathf.Deg2Rad * spreadMultiplier;
        mat.SetFloat("_ConeAngle", coneAngle);
        mat.SetFloat("_GlowIntensity", baseIntensity * intensityMultiplier);
    }
}
