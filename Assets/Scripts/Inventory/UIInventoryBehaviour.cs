using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBehaviour : MonoBehaviour
{
    [SerializeField]
    InventoryBehaviour inventory;

    [SerializeField]
    GameObject inventoryItemButtonPrefab;

    [SerializeField]
    Transform inventoryItemButtonContainer;

    void Start()
    {
        RefreshInventoryUI();
    }

    private void OnEnable()
    {
        inventory.OnInventoryItemChange += RefreshInventoryUI;
    }

    private void OnDisable()
    {
        inventory.OnInventoryItemChange -= RefreshInventoryUI;
    }

    private void RefreshInventoryUI()
    {
        int _childCount = inventoryItemButtonContainer.childCount;

        for (int i = 0; i < _childCount; i++)
        {
            Destroy(inventoryItemButtonContainer.GetChild(i).gameObject);
        }

        List<InventoryItem> _inventoryItems = inventory.GetInventoryItems();

        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            GameObject _newButton = Instantiate(inventoryItemButtonPrefab, inventoryItemButtonContainer);
            _newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _inventoryItems[i].itemName;
        }
    }
}
