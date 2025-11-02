using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupAudio : MonoBehaviour
{
    [Header("Pickup Settings")]
    public AudioClip pickupSound;           // sound to play
    [Range(0f, 1f)] public float volume = 1f;
    public bool destroyOnPickup = true;     // should object disappear?

    [Header("Optional Settings")]
    public bool playAtPosition = true;      // plays sound at object position
    public bool disableRendererInstead = false; // for hidden object logic

    private AudioSource audioSource;
    private bool isPickedUp = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Prevent interference if already used for looping
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;   // make it 3D sound
        audioSource.volume = volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect player pickup
        if (isPickedUp) return;
        if (!other.CompareTag("Player")) return;

        isPickedUp = true;

        // Play the sound (at position or attached)
        if (pickupSound)
        {
            if (playAtPosition)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
            else
            {
                audioSource.clip = pickupSound;
                audioSource.Play();
            }
        }

        // Handle object removal
        if (destroyOnPickup)
        {
            Destroy(gameObject, pickupSound ? pickupSound.length : 0f);
        }
        else if (disableRendererInstead)
        {
            // Hide mesh but keep object in world
            Renderer r = GetComponent<Renderer>();
            if (r) r.enabled = false;
        }
    }
}
