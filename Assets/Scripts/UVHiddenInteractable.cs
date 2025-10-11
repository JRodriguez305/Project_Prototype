using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class UVHiddenObject : MonoBehaviour
{
    [Header("Reveal Settings")]
    public string requiredTag = "Player";        // Player tag for interaction
    public Color glowColor = Color.blue;         // UV glow color
    public float maxGlowIntensity = 4f;          // How bright it glows
    public float fadeSpeed = 2f;                 // How fast it fades in/out

    [Header("Inventory Settings")]
    public InventoryItem itemToAdd;              // Inventory item
    public InventoryBehaviour inventory;         // Reference to inventory

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;

    private Renderer rend;
    private Collider col;
    private Material mat;
    private bool isInteractable = false;
    private bool isRevealed = false;
    private float visibility = 0f;               // 0 = invisible, 1 = fully visible
    private Color currentEmission;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        col.isTrigger = true;

        // Use a copy of the material to avoid affecting other objects
        mat = rend.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black);

        rend.enabled = false;
        SetAlpha(0f);
    }

    void Update()
    {
        // Smoothly update emission and alpha
        float intensity = Mathf.Lerp(0f, maxGlowIntensity, visibility);
        currentEmission = Color.Lerp(currentEmission, glowColor * intensity, Time.deltaTime * fadeSpeed);
        SetAlpha(visibility);

        // Interaction
        if (isRevealed && isInteractable && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    /// <summary>
    /// Call this from your UV flashlight to reveal the object.
    /// </summary>
    public void Reveal()
    {
        if (!isRevealed)
        {
            isRevealed = true;
            rend.enabled = true;
            StopAllCoroutines();
            StartCoroutine(FadeIn());
            gameObject.layer = LayerMask.NameToLayer("HiddenUV");
        }
    }

    /// <summary>
    /// Call this to hide the object again.
    /// </summary>
    public void Hide()
    {
        isRevealed = false;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        while (visibility < 1f)
        {
            visibility += Time.deltaTime * fadeSpeed;
            if (visibility > 1f) visibility = 1f;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        while (visibility > 0f)
        {
            visibility -= Time.deltaTime * fadeSpeed;
            if (visibility < 0f) visibility = 0f;
            if (visibility == 0f)
                rend.enabled = false; // fully invisible
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
            isInteractable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
            isInteractable = false;
    }

    public void Interact()
    {
        if (itemToAdd != null)
        {
            Debug.Log("[UVHiddenObject] Picked up item: " + itemToAdd.ToString());
            if (inventory != null)
                inventory.AddInventoryItem(itemToAdd);
        }
        else
        {
            Debug.Log("[UVHiddenObject] Picked up unknown item!");
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Sets the transparency (alpha) and updates emission for all shader types.
    /// </summary>
    private void SetAlpha(float a)
    {
        // Base color transparency
        if (mat.HasProperty("_Color"))
        {
            Color c = mat.color;
            c.a = a;
            mat.color = c;
        }
        else if (mat.HasProperty("_BaseColor"))
        {
            Color c = mat.GetColor("_BaseColor");
            c.a = a;
            mat.SetColor("_BaseColor", c);
        }

        // Update emission color for glow
        if (mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", currentEmission);
        }
    }
}
