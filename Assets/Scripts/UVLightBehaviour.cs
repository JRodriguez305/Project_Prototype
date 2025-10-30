using UnityEngine;
using System.Collections.Generic;

public class UVLightBehaviour : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 10f;
    [Range(0.1f, 2f)] public float detectionRadius = 0.5f;
    public LayerMask detectionMask;

    private readonly List<HiddenObjectBehaviour> revealedObjects = new List<HiddenObjectBehaviour>();

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.magenta);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, detectionRadius, detectionRange, detectionMask);
        HashSet<HiddenObjectBehaviour> hitThisFrame = new HashSet<HiddenObjectBehaviour>();

        foreach (RaycastHit hit in hits)
        {
            HiddenObjectBehaviour hidden = hit.collider.GetComponent<HiddenObjectBehaviour>();
            if (hidden != null)
            {
                hidden.Reveal();
                hitThisFrame.Add(hidden);

                if (!revealedObjects.Contains(hidden))
                    revealedObjects.Add(hidden);
            }
        }

        // Safely hide objects that are no longer hit
        for (int i = revealedObjects.Count - 1; i >= 0; i--)
        {
            HiddenObjectBehaviour obj = revealedObjects[i];

            // skip destroyed or null references
            if (obj == null)
            {
                revealedObjects.RemoveAt(i);
                continue;
            }

            if (!hitThisFrame.Contains(obj))
            {
                obj.Hide();
                revealedObjects.RemoveAt(i);
            }
        }
    }
}
