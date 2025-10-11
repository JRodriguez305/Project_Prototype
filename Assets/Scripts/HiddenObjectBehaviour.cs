using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectBehaviour : MonoBehaviour
{
    private Renderer rend;
    private Collider col;

    private bool isRevealed = false;

    public string requiredTag = "Hidden";

    [SerializeField] private InventoryItem itemToAdd;
    [SerializeField] private InventoryBehaviour inventory;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        if (rend != null)
        {
            Hidden();
        }
    }

    void Hidden()
    {
        rend.enabled = false;
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

    private void OnMouseDown()
    {
        if (isRevealed && CompareTag(requiredTag))
        {
            Debug.Log("Picked up item");
            inventory.AddInventoryItem(itemToAdd);
            Destroy(gameObject);
        }
    }

    // ✅ New method added for player interaction
    public void Interact()
    {
        Debug.Log("Player interacted with hidden object.");

        // Optionally add to inventory
        if (inventory != null && itemToAdd != null)
        {
            inventory.AddInventoryItem(itemToAdd);
        }

        // Immediately destroy the object
        Destroy(gameObject);
    }
}
