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

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false; // start invisible
    }

    public void Reveal()
    {
        if (isRevealed || isCollected || rend == null) return;
        rend.enabled = true;
        isRevealed = true;
        gameObject.layer = LayerMask.NameToLayer("HiddenTest");
    }

    public void Hide()
    {
        // safeguard for destroyed renderer
        if (rend == null || !isRevealed || isCollected) return;
        rend.enabled = false;
        isRevealed = false;
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

    private void Collect()
    {
        isCollected = true;

        Debug.Log($"Picked up: {itemToAdd?.itemName ?? "Unknown"}");

        if (inventory != null && itemToAdd != null)
            inventory.AddInventoryItem(itemToAdd);

        // Delay destruction slightly so UVLightBehaviour can clean up
        Destroy(gameObject, 0.1f);
    }
}
