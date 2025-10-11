using UnityEngine;

public class PlayerFlashlightController : MonoBehaviour
{
    public Light uvFlashlight;         // The UV spotlight
    public KeyCode toggleKey = KeyCode.F; // Key to toggle on/off

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && uvFlashlight != null)
        {
            uvFlashlight.enabled = !uvFlashlight.enabled;
        }
    }
}
