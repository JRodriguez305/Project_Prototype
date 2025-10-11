using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootstepsAndInteraction : MonoBehaviour
{
    [Header("Running Footsteps Settings")]
    public AudioClip footstepClip;
    public float stepInterval = 0.5f; // time between left/right steps
    public float panAmount = 1f;      // stereo pan left/right
    private AudioSource footstepSource;
    private bool leftFoot = true;

    [Header("Interaction Settings")]
    public TMP_Text uiText;
    public float audioDuration = 2f;   // footsteps play for 2 seconds
    public float messageDuration = 2f; // message shows for 2 seconds

    [Header("Player Movement")]
    public MonoBehaviour movementScript; // assign your movement script here

    private bool canInteract = false;
    private HiddenObjectBehaviour currentHiddenObject = null;
    private bool frozen = false;

    void Awake()
    {
        footstepSource = GetComponent<AudioSource>();

        if (uiText != null)
            uiText.gameObject.SetActive(false);

        footstepSource.loop = false;
        footstepSource.playOnAwake = false;
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E) && currentHiddenObject != null && !frozen)
        {
            // Immediately interact with object
            currentHiddenObject.Interact();

            // Freeze player movement
            if (movementScript != null)
                movementScript.enabled = false;

            // Start footsteps + message sequence
            StartCoroutine(InteractSequence());
        }
    }

    private IEnumerator InteractSequence()
    {
        frozen = true;

        // ---- Step 1: Play running footsteps behind player for exact duration ----
        if (footstepClip != null)
        {
            int totalSteps = Mathf.CeilToInt(audioDuration / stepInterval);
            for (int i = 0; i < totalSteps; i++)
            {
                footstepSource.panStereo = leftFoot ? -panAmount : panAmount;
                footstepSource.PlayOneShot(footstepClip);
                leftFoot = !leftFoot;

                yield return new WaitForSeconds(stepInterval);
            }

            footstepSource.Stop();
        }

        // ---- Step 2: Show message ----
        if (uiText != null)
        {
            uiText.text = "What was that!";
            uiText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(messageDuration);

        if (uiText != null)
            uiText.gameObject.SetActive(false);

        // ---- Unfreeze player ----
        frozen = false;
        if (movementScript != null)
            movementScript.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hidden"))
        {
            canInteract = true;
            currentHiddenObject = other.GetComponent<HiddenObjectBehaviour>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hidden"))
        {
            canInteract = false;
            currentHiddenObject = null;
        }
    }
}
