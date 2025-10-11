using UnityEngine;
using UnityEngine.UI;       // For UI Text
using System.Collections;   // Needed for IEnumerator

public class InteractableObject : MonoBehaviour
{
    [Header("Message Settings")]
    public string message = "What was that!";
    public Text uiText;           // Assign the UI Text element in Inspector
    public float displayTime = 2f;

    private bool playerNearby = false;

    void Update()
    {
        // Check for interaction input
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ShowMessage());
        }
    }

    private IEnumerator ShowMessage()
    {
        uiText.text = message;          // Set the text
        uiText.gameObject.SetActive(true);  // Show it

        yield return new WaitForSeconds(displayTime); // Wait for displayTime

        uiText.gameObject.SetActive(false); // Hide text
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect if player enters trigger
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detect if player leaves trigger
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
