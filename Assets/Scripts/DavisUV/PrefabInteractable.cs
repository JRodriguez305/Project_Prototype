using UnityEngine;
using System.Collections;

public class PrefabInteractable : MonoBehaviour
{
    [Header("Messages")]
    [SerializeField] private string promptMessage = "Press E to interact";
    [SerializeField] private string interactionMessage = "You interacted with the object!";
    [SerializeField] private float promptDuration = 3f;
    [SerializeField] private float interactionDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactionClip;
    [SerializeField] private float audioPlayDuration = 2f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 3f;

    [Header("Player Freeze Settings")]
    [SerializeField] private MonoBehaviour playerCameraController; // drag your camera controller script here
    [SerializeField] private float freezeDuration = 2f; // How long to freeze the camera

    private Transform player;
    private bool hasInteracted = false;
    private bool isPlayerNearby = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (!player)
            Debug.LogError("Player object with tag 'Player' not found in the scene!");

        if (!UIManager.instance)
            Debug.LogError("UIManager not found in the scene!");
    }

    private void Update()
    {
        if (!player || hasInteracted) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactDistance)
        {
            if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                UIManager.instance.ShowMessage(promptMessage, promptDuration);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
        else
        {
            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                UIManager.instance.ClearMessage(); // Clear prompt immediately when leaving range
            }
        }
    }

    private void Interact()
    {
        hasInteracted = true;

        // Show post-interaction message
        UIManager.instance.ShowMessage(interactionMessage, interactionDuration);

        // Play audio if assigned
        if (audioSource && interactionClip)
        {
            audioSource.PlayOneShot(interactionClip);
            StartCoroutine(StopAudioAfter(audioPlayDuration));
        }

        // Freeze player camera if assigned
        if (playerCameraController != null)
        {
            StartCoroutine(FreezeCamera());
        }
    }

    private IEnumerator StopAudioAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (audioSource) audioSource.Stop();
    }

    private IEnumerator FreezeCamera()
    {
        playerCameraController.enabled = false;
        yield return new WaitForSeconds(freezeDuration);
        playerCameraController.enabled = true;
    }
}
