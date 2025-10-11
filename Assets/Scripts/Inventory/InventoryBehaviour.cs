using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehaviour : MonoBehaviour
{
    [SerializeField]
    List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public Action OnInventoryItemChange;

    public void AddInventoryItem(InventoryItem _inventoryItem)
    {
        inventoryItems.Add(_inventoryItem);
        OnInventoryItemChange?.Invoke();
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryItems;
    }
}
