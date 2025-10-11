using UnityEngine;

public class UVFlashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light uvLight;
    public float revealRange = 10f;
    public float revealAngle = 45f;
    public LayerMask revealLayer;

    [Header("Toggle Key")]
    public KeyCode toggleKey = KeyCode.F;

    private bool isOn = true;

    void Update()
    {
        // Toggle flashlight
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            if (uvLight != null)
                uvLight.enabled = isOn;
        }

        if (!isOn) return;

        // Use modern API instead of deprecated FindObjectsOfType
        var hiddenObjects = Object.FindObjectsByType<UVHiddenObject>(FindObjectsSortMode.None);

        foreach (var hidden in hiddenObjects)
        {
            Vector3 dirToTarget = (hidden.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, hidden.transform.position);
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            if (distance <= revealRange && angle < revealAngle)
            {
                hidden.Reveal();
            }
            else
            {
                hidden.Hide();
            }
        }
    }
}
