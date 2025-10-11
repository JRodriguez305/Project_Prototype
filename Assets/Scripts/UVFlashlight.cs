using UnityEngine;
using System.Collections.Generic;

public class UVSpotlightEffect : MonoBehaviour
{
    public float range = 10f;
    public float spotAngle = 30f;
    public int rayCount = 25;
    public LayerMask uvLayer;
    public Color glowColor = Color.magenta;
    public float glowIntensity = 8f;

    private List<Renderer> hitRenderers = new List<Renderer>();

    void Update()
    {
        if (!enabled) return;

        hitRenderers.Clear();
        float halfAngle = spotAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 dir = transform.forward;
            dir = Quaternion.Euler(Random.Range(-halfAngle, halfAngle),
                                   Random.Range(-halfAngle, halfAngle),
                                   0) * dir;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range, uvLayer))
            {
                Renderer r = hit.collider.GetComponent<Renderer>();
                if (r != null)
                {
                    r.material.EnableKeyword("_EMISSION");
                    r.material.SetColor("_EmissionColor", glowColor * glowIntensity);
                    if (!hitRenderers.Contains(r))
                        hitRenderers.Add(r);
                }
            }
        }

        // Fade out non-hit renderers
        foreach (Renderer r in FindObjectsOfType<Renderer>())
        {
            if (!hitRenderers.Contains(r))
                r.material.SetColor("_EmissionColor", Color.black);
        }
    }
}
