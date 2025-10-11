using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class UVFootstepAudio : MonoBehaviour
{
    [SerializeField] private Transform uvFlashlight;
    [SerializeField] private float detectionAngle = 0.5f;
    [SerializeField] private float detectionDistance = 5f;
    [SerializeField] private float playDuration = 2f;

    private AudioSource audioSource;
    private Renderer rend;
    private bool hasPlayed = false;
    private bool initialized = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();

        // ✅ Disable spatial effect — prevents transform resync
        audioSource.spatialBlend = 0f; // make it 2D sound
        audioSource.playOnAwake = false;

        // ✅ Pre-warm audio to prevent Unity's first-frame sync issue
        audioSource.Play();
        audioSource.Stop();

        initialized = true;
    }

    void LateUpdate() // ✅ run after camera moves
    {
        if (!initialized || hasPlayed || uvFlashlight == null) return;

        Vector3 toFootprint = transform.position - uvFlashlight.position;
        float dot = Vector3.Dot(toFootprint.normalized, uvFlashlight.forward);

        if (dot > Mathf.Cos(detectionAngle) && toFootprint.magnitude <= detectionDistance)
        {
            audioSource.Play();
            hasPlayed = true;
            Invoke(nameof(StopAudio), playDuration);
        }
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
