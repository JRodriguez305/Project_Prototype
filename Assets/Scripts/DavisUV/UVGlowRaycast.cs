using UnityEngine;

public class UVGlowRaycast : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light uvFlashlight;           // The UV spotlight
    public KeyCode toggleKey = KeyCode.F; // Key to toggle flashlight
    public float flashlightIntensity = 8f;

    [Header("Glow Settings")]
    public Color glowColor = Color.magenta;
    public float glowIntensity = 8f;

    [Header("Raycast Settings")]
    public float raycastDistance = 20f; // Max distance for detecting objects

    private bool flashlightOn = true;
    private Material mat;

    void Start()
    {
        // Get the object's material
        Renderer objectRenderer = GetComponent<Renderer>();
        mat = objectRenderer.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);
    }

    void Update()
    {
        // Toggle flashlight
        if (Input.GetKeyDown(toggleKey))
        {
            flashlightOn = !flashlightOn;
            uvFlashlight.enabled = flashlightOn;
        }

        // Only process glow if flashlight is on
        if (flashlightOn)
        {
            Ray ray = new Ray(uvFlashlight.transform.position, uvFlashlight.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.transform == transform)
                {
                    // Object is being hit by the light → glow
                    mat.SetColor("_EmissionColor", glowColor * glowIntensity);
                }
                else
                {
                    mat.SetColor("_EmissionColor", Color.black);
                }
            }
            else
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
        }
    }
}

