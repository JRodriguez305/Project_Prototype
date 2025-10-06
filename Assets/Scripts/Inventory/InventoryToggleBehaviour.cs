using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggleBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject inventoryPanel;

    private bool isInventoryVisible = false;

    void Update()
    {
        // Check for the key press
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventoryPanel.SetActive(isInventoryVisible);
    }
}
