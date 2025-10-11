using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class UVHiddenInteractable : MonoBehaviour
{
    [Header("Reveal Settings")]
    public string requiredTag = "Hidden";
    private Renderer rend;
    private Collider col;
    private bool isRevealed = false;

    [Header("Inventory Settings")]
    [SerializeField] private InventoryItem itemToAdd;
    [SerializeField] private InventoryBehaviour inventory;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;
    private bool isInteractable = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        col.isTrigger = true; // ensures trigger works for interaction
        Hide();
    }

    void Update()
    {
        // Interaction check
        if (isRevealed && isInteractable && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    public void Reveal()
    {
        if (!isRevealed)
        {
            isRevealed = true;
            rend.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("HiddenTest");
        }
    }

    public void Hide()
    {
        if (isRevealed)
        {
            isRevealed = false;
            rend.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("HiddenTest");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isInteractable = false;
        }
    }

    // ✅ Player interaction method
    public void Interact()
    {
        // Fixed: don't assume InventoryItem has a .name
        Debug.Log("[UVHiddenInteractable] Item picked up!");

        if (inventory != null && itemToAdd != null)
        {
            inventory.AddInventoryItem(itemToAdd);
        }

        Destroy(gameObject);
    }

    // ✅ Optional helper methods for external scripts
    public bool IsInteractable() => isInteractable;
    public bool IsRevealed() => isRevealed;
}
