using UnityEngine;

[RequireComponent(typeof(Renderer))]
[DisallowMultipleComponent]
public class InteractableOutline : MonoBehaviour
{
    [Header("UV Gating")]
    public bool requireUVHit = true;   // only allow outline if UV is actually hitting (_LightOn=1)
    public bool requireRevealed = true; // also require HiddenObjectBehaviour.IsRevealed

    private Outline outline;
    private Renderer rend;
    private HiddenObjectBehaviour hidden;
    private Material mat;

    void Awake()
    {
        outline = GetComponent<Outline>();
        rend = GetComponent<Renderer>();
        hidden = GetComponent<HiddenObjectBehaviour>();
        if (rend) mat = rend.material;
    }

    public bool CanOutlineNow()
    {
        if (requireRevealed && hidden != null && !hidden.IsRevealed) return false;

        if (requireUVHit && mat != null && mat.HasProperty("_LightOn"))
        {
            float on = mat.GetFloat("_LightOn");
            if (on < 0.5f) return false; // not under the UV cone right now
        }
        return true;
    }

    public void HoverStart()
    {
        if (outline == null) outline = GetComponent<Outline>();
        if (outline == null) return;

        if (CanOutlineNow()) outline.EnableOutline();
    }

    public void HoverEnd()
    {
        if (outline == null) outline = GetComponent<Outline>();
        if (outline == null) return;

        outline.DisableOutline();
    }
}
