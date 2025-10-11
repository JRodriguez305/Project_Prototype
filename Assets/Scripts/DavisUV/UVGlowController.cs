using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVGlowController : MonoBehaviour
{
    public Light uvFlashlight;              // The UV spotlight
    public KeyCode toggleKey = KeyCode.F;   // Key to toggle flashlight
    public Color glowColor = new Color(0.5f, 0f, 1f); // UV neon
    public float glowIntensity = 5f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_GlowColor", glowColor);
        mat.SetFloat("_GlowIntensity", glowIntensity);
    }

    void Update()
    {
        // Toggle flashlight
        if (Input.GetKeyDown(toggleKey))
        {
            if (uvFlashlight != null)
                uvFlashlight.enabled = !uvFlashlight.enabled;
        }

        // Pass light info to shader only if flashlight is on
        if (uvFlashlight != null && uvFlashlight.enabled)
        {
            mat.SetVector("_LightPos", uvFlashlight.transform.position);
            mat.SetVector("_LightDir", uvFlashlight.transform.forward);
            mat.SetFloat("_ConeAngle", uvFlashlight.spotAngle * 0.5f * Mathf.Deg2Rad);
        }
        else
        {
            mat.SetFloat("_GlowIntensity", 0f); // no glow when flashlight off
        }
    }
}
