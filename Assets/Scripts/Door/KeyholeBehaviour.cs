using UnityEngine;

public class KeyholeBehaviour : MonoBehaviour
{
    public DoorBehaviour connectedDoor;

    public void UnlockDoor()
    {
        connectedDoor.Unlock();
    }
}
