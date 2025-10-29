using UnityEngine;

public class KeyholeBehaviour : MonoBehaviour
{
    public DoorOpenBehaviour connectedDoor;

    public void UnlockDoor()
    {
        connectedDoor.Unlock();
    }
}
