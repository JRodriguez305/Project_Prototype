using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(AudioSource))]
public class UVFootstepAudio : MonoBehaviour
{
    [SerializeField] private Transform uvFlashlight;       // assign your UV spotlight here
    [SerializeField] private float detectionAngle = 0.5f;  // must match shader cone angle
    [SerializeField] private float detectionDistance = 5f; // max distance to detect light
    [SerializeField] private float playDuration = 2f;      // how long the audio plays

    private AudioSource audioSource;
    private Renderer rend;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (hasPlayed) return;

        Vector3 toFootprint = transform.position - uvFlashlight.position;
        float dot = Vector3.Dot(toFootprint.normalized, uvFlashlight.forward);

        if (dot > Mathf.Cos(detectionAngle) && toFootprint.magnitude <= detectionDistance)
        {
            audioSource.Play();
            hasPlayed = true;
            // Stop audio after playDuration
            Invoke(nameof(StopAudio), playDuration);
        }
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
