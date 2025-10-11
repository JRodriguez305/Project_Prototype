using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVRevealSticker : MonoBehaviour
{
    public Light uvFlashlight;
    public Color glowColor = Color.magenta;
    public float glowIntensity = 10f;
    public float raycastDistance = 20f;

    private Material mat;
    private Color baseColor;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");

        // Save the base color and make it fully transparent
        baseColor = mat.color;
        baseColor.a = 0f;
        mat.color = baseColor;

        // Start with emission off (no glow)
        mat.SetColor("_EmissionColor", Color.black);
    }

    void Update()
    {
        if (uvFlashlight == null) return;

        Ray ray = new Ray(uvFlashlight.transform.position, uvFlashlight.transform.forward);
        RaycastHit hit;

        bool isHit = uvFlashlight.enabled &&
                     Physics.Raycast(ray, out hit, raycastDistance) &&
                     hit.transform == transform;

        if (isHit)
        {
            // Fully visible + glow when hit
            baseColor.a = 1f;
            mat.color = baseColor;
            mat.SetColor("_EmissionColor", glowColor * glowIntensity);
        }
        else
        {
            // Fully transparent when not hit
            baseColor.a = 0f;
            mat.color = baseColor;
            mat.SetColor("_EmissionColor", Color.black);
        }
    }
}
