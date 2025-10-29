using UnityEngine;

public class DoorOpenBehaviour : MonoBehaviour
{
    private bool isUnlocked = false;
    private bool isOpening = false;

    public float openSpeed = 2f;
    public float openAngle = 90f;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;
    }

    void Update()
    {
        if (isOpening)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }
    public void Unlock()
    {
        isUnlocked = true;
        isOpening = true;
    }
}
