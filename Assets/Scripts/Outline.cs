using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    [Header("Appearance")]
    public Color outlineColor = new Color(0.1f, 0.8f, 1f, 1f);
    [Range(0.001f, 0.1f)] public float outlineWidth = 0.02f;
    [Range(0f, 1f)] public float outlineAlpha = 1f;

    [Header("Behaviour")]
    public bool startEnabled = false;

    static readonly string ShaderName = "Custom/OutlineInvertedHull";
    Material outlineMatInstance;
    Renderer rend;
    Material[] originalMats;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalMats = rend.sharedMaterials;

        var shader = Shader.Find(ShaderName);
        if (!shader)
        {
            Debug.LogError($"[Outline] Shader '{ShaderName}' not found. Did you create it?");
            enabled = false;
            return;
        }

        outlineMatInstance = new Material(shader);
        ApplySettingsToMaterial();

        if (startEnabled)
            EnableOutline();
    }

    void OnDestroy()
    {
        if (outlineMatInstance)
        {
            if (Application.isPlaying) Destroy(outlineMatInstance);
            else DestroyImmediate(outlineMatInstance);
        }
    }

    public void SetEnabled(bool on)
    {
        if (on) EnableOutline();
        else DisableOutline();
    }

    public void EnableOutline()
    {
        if (!outlineMatInstance) return;

        var mats = rend.sharedMaterials;
        // Prevent duplicates
        for (int i = 0; i < mats.Length; i++)
            if (mats[i] == outlineMatInstance) return;

        var newMats = new Material[mats.Length + 1];
        for (int i = 0; i < mats.Length; i++) newMats[i] = mats[i];
        newMats[mats.Length] = outlineMatInstance;
        rend.sharedMaterials = newMats;
    }

    public void DisableOutline()
    {
        var mats = rend.sharedMaterials;
        int idx = System.Array.IndexOf(mats, outlineMatInstance);
        if (idx < 0) return;

        var newMats = new Material[mats.Length - 1];
        int n = 0;
        for (int i = 0; i < mats.Length; i++)
            if (i != idx) newMats[n++] = mats[i];

        rend.sharedMaterials = newMats;
    }

    public void ApplySettingsToMaterial()
    {
        if (!outlineMatInstance) return;
        outlineMatInstance.SetColor("_OutlineColor", outlineColor);
        outlineMatInstance.SetFloat("_OutlineWidth", outlineWidth);
        outlineMatInstance.SetFloat("_Alpha", outlineAlpha);
        outlineMatInstance.enableInstancing = true;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (outlineMatInstance) ApplySettingsToMaterial();
    }
#endif
}
