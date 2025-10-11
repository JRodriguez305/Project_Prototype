using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Assign the TMP_Text manually in Inspector")]
    [SerializeField] private TMP_Text interactionText;

    private Coroutine activeCoroutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (!interactionText)
        {
            Debug.LogError("InteractionText not assigned! Drag a TMP_Text object into the inspector.");
        }
    }

    public void ShowMessage(string message, float duration)
    {
        if (!interactionText) return;

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(ShowRoutine(message, duration));
    }

    public void ClearMessage()
    {
        if (!interactionText) return;

        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        interactionText.text = "";
    }

    private IEnumerator ShowRoutine(string message, float duration)
    {
        interactionText.text = message;
        yield return new WaitForSeconds(duration);
        interactionText.text = "";
        activeCoroutine = null;
    }
}

