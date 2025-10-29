using UnityEngine;

public class DoorSpawnerBehaviour : MonoBehaviour
{
    [Header("Door Setup")]
    public GameObject doorPrefab;
    public Transform spawnPoint;
    public GameObject doorManager;

    public void SpawnDoor()
    {
        if (doorPrefab == null || spawnPoint == null || doorManager == null)
        {
            return;
        }

        GameObject doorInstance = Instantiate(doorPrefab, spawnPoint.position, spawnPoint.rotation);

        doorInstance.transform.SetParent(doorManager.transform);
        doorInstance.transform.localPosition = new Vector3(-0.519699931f, 1.92449999f, -0.0932502747f); ;
        doorInstance.transform.localRotation = Quaternion.identity;

        KeyholeBehaviour keyhole = doorInstance.GetComponentInChildren<KeyholeBehaviour>();
        DoorOpenBehaviour doorOpen = doorManager.GetComponent<DoorOpenBehaviour>();
        InventoryBehaviour playerInventory = FindObjectOfType<InventoryBehaviour>();
        DoorUnlockBehaviour unlock = doorInstance.GetComponentInChildren<DoorUnlockBehaviour>();

        if (keyhole != null && doorOpen != null)
        {
            keyhole.connectedDoor = doorOpen;
        }

        if (unlock != null && playerInventory != null)
        {
            unlock.inventory = playerInventory;
        }
    }
}
