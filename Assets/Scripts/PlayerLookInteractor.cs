using UnityEngine;

public class PlayerLookInteractor : MonoBehaviour
{
    public Camera cam;
    [Range(0.5f, 6f)] public float interactDistance = 3f;
    public LayerMask interactMask = ~0; // set in Inspector if you want

    Outline current;

    void Reset()
    {
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        if (!cam) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            var ol = hit.collider.GetComponentInParent<Outline>();
            if (ol != current)
            {
                if (current) current.SetEnabled(false);
                current = ol;
                if (current) current.SetEnabled(true);
            }
        }
        else
        {
            if (current) current.SetEnabled(false);
            current = null;
        }
    }
}
