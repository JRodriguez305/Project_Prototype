using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class HiddenObjectBehaviour : MonoBehaviour
{
    private Renderer rend;
    private bool isRevealed = false;
    private bool isCollected = false;

    [Header("Pickup Settings")]
    public string requiredTag = "Hidden";
    [SerializeField] private InventoryItem itemToAdd;
    [SerializeField] private InventoryBehaviour inventory;

    // 👁️ External read-only property
    public bool IsRevealed => isRevealed && !isCollected;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false; // starts invisible
    }

    /// <summary>
    /// Makes the object visible when UV light hits it.
    /// </summary>
    public void Reveal()
    {
        if (isRevealed || isCollected || rend == null) return;
        rend.enabled = true;
        isRevealed = true;

        // optional: switch to a UV-visible layer if needed
        gameObject.layer = LayerMask.NameToLayer("HiddenTest");
    }

    /// <summary>
    /// Hides the object when UV light is no longer hitting it.
    /// </summary>
    public void Hide()
    {
        if (rend == null || !isRevealed || isCollected) return;
        rend.enabled = false;
        isRevealed = false;

        // optional: maintain same hidden layer
        gameObject.layer = LayerMask.NameToLayer("HiddenTest");
    }

    private void OnMouseDown()
    {
        if (isRevealed && CompareTag(requiredTag) && !isCollected)
        {
            Collect();
        }
    }

    public void Interact()
    {
        if (!isRevealed || isCollected) return;
        Collect();
    }

    /// <summary>
    /// Handles collection logic, inventory addition, and cleanup.
    /// </summary>
    private void Collect()
    {
        isCollected = true;
        Debug.Log($"🧩 Picked up: {itemToAdd?.itemName ?? "Unknown"}");

        if (inventory != null && itemToAdd != null)
            inventory.AddInventoryItem(itemToAdd);

        // Short delay before destroy to allow UV system to update safely
        Destroy(gameObject, 0.1f);
    }
}
