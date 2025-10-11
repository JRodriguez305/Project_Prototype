using UnityEngine;

public class FlashlightFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 localOffset = new Vector3(0f, -0.1f, 0.3f);

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Match camera rotation perfectly
        transform.rotation = cameraTransform.rotation;

        // Keep position offset (so light appears in front of camera)
        transform.position = cameraTransform.position +
                             cameraTransform.rotation * localOffset;
    }
}
