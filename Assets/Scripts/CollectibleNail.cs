using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class CollectibleNail : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 2.5f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Pickup FX")]
    public AudioClip pickupSound;
    public ParticleSystem pickupEffect;
    public float destroyDelay = 0.5f;

    private bool collected = false;
    private Material mat;
    private AudioSource audioSource;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        mat = GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();

        // Collider sanity setup
        Collider col = GetComponent<Collider>();
        col.isTrigger = false;

        // Audio setup
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
        if (collected || cam == null || mat == null) return;

        // Only interactable under UV light
        float lightOn = mat.HasProperty("_LightOn") ? mat.GetFloat("_LightOn") : 0f;
        if (lightOn < 0.5f) return;

        // Raycast from camera center
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.cyan);

            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(interactKey))
                {
                    Debug.Log("✅ Picked up glowing nail!");
                    Collect();
                }
            }
        }
    }

    void Collect()
    {
        if (collected) return;
        collected = true;

        // Notify manager
        if (NailObjectiveManager.Instance != null)
            NailObjectiveManager.Instance.AddCollectedNail();
        else
            Debug.LogWarning("⚠️ NailObjectiveManager not found in scene!");

        StartCoroutine(DestroyAfterEffect());
    }

    IEnumerator DestroyAfterEffect()
    {
        // Play pickup sound
        if (pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        // Spawn particles
        if (pickupEffect != null)
        {
            ParticleSystem fx = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, fx.main.duration + 0.2f);
        }

        // Smooth glow fade-out
        if (mat != null && mat.HasProperty("_GlowIntensity"))
        {
            float startGlow = mat.GetFloat("_GlowIntensity");
            float t = 0f;
            while (t < destroyDelay)
            {
                t += Time.deltaTime;
                float newGlow = Mathf.Lerp(startGlow, 0f, t / destroyDelay);
                mat.SetFloat("_GlowIntensity", newGlow);
                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
