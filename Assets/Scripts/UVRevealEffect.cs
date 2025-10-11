using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class UVRevealInteractable : MonoBehaviour
{
    [Header("Reveal Settings")]
    public Color glowColor = Color.magenta;
    public float maxGlowIntensity = 6f;
    public float fadeSpeed = 5f;
    public float revealDuration = 0.5f;

    [Header("Interaction Settings")]
    public string interactTag = "Player";
    public KeyCode interactKey = KeyCode.E;
    public string itemName = "Hidden Object";

    private Material mat;
    private float visibility = 0f;
    private float lastRevealTime;
    private Color currentEmission;
    private Color targetEmission;
    private bool initialized = false;
    private bool isInteractable = false;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);
        SetAlpha(0f);
        initialized = true;

        // Make sure the collider can be detected
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
            col.isTrigger = true;
    }

    void Update()
    {
        if (!initialized) return;

        float timeSinceReveal = Time.time - lastRevealTime;

        // Fade out when not in light
        if (timeSinceReveal > revealDuration)
        {
            visibility = Mathf.Lerp(visibility, 0f, Time.deltaTime * fadeSpeed);
        }

        // Update visuals
        float intensity = Mathf.Lerp(0f, maxGlowIntensity, visibility);
        targetEmission = glowColor * intensity;
        currentEmission = Color.Lerp(currentEmission, targetEmission, Time.deltaTime * fadeSpeed);
        mat.SetColor("_EmissionColor", currentEmission);
        SetAlpha(visibility);

        // Interaction logic
        if (isInteractable && visibility > 0.8f && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    public void RevealFromUV(Vector3 lightPos, Vector3 lightDir)
    {
        lastRevealTime = Time.time;
        visibility = Mathf.Lerp(visibility, 1f, Time.deltaTime * fadeSpeed);
    }

    private void SetAlpha(float a)
    {
        if (!mat.HasProperty("_Color")) return;
        Color c = mat.color;
        c.a = a;
        mat.color = c;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(interactTag))
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(interactTag))
        {
            isInteractable = false;
        }
    }

    private void Interact()
    {
        Debug.Log($"[UVRevealInteractable] {itemName} interacted with!");
        // Example: you can play a sound, add to inventory, or destroy it
        // Destroy(gameObject);
    }
}
