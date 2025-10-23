using UnityEngine;

public class DoorUnlockBehaviour : MonoBehaviour
{
    public InventoryBehaviour inventory;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Keyhole"))
                {
                    TryUseKeyOnKeyhole(hit.collider.gameObject);
                }
            }
        }
    }

    void TryUseKeyOnKeyhole(GameObject keyhole)
    {
        var items = inventory.GetInventoryItems();
        InventoryItem keyItem = items.Find(items => items.itemName == "Key");

        if (keyItem != null)
        {
            keyhole.GetComponent<KeyholeBehaviour>().UnlockDoor();

            items.Remove(keyItem);
            inventory.OnInventoryItemChange?.Invoke();
        }
    }
}
