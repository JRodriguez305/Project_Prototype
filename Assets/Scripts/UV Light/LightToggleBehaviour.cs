using UnityEngine;

public class LightToggleBehaviour : MonoBehaviour
{
    [Header("Flashlight References")]
    [SerializeField] private GameObject uvLight;
    [SerializeField] private GameObject flashLight;

    [Header("Controls")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private UVLightBehaviour uvBehaviour;
    private Light uvLightSource;
    private Light normalLightSource;
    private bool isUVMode = false;

    void Start()
    {
        if (uvLight == null || flashLight == null)
        {
            Debug.LogWarning("LightToggleBehaviour: Missing light references!");
            return;
        }

        uvBehaviour = uvLight.GetComponent<UVLightBehaviour>();
        uvLightSource = uvLight.GetComponent<Light>();
        normalLightSource = flashLight.GetComponent<Light>();

        SetMode(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isUVMode = !isUVMode;
            SetMode(isUVMode);
        }
    }

    private void SetMode(bool uvMode)
    {
        uvLight.SetActive(uvMode);
        flashLight.SetActive(!uvMode);

        if (uvBehaviour != null)
            uvBehaviour.enabled = uvMode;

        if (uvLightSource != null)
            uvLightSource.enabled = uvMode;
        if (normalLightSource != null)
            normalLightSource.enabled = !uvMode;

        Debug.Log($"Light mode: {(uvMode ? "UV" : "Normal")}");
    }
}
