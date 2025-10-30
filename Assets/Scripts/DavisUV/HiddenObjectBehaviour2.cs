using System.Collections;                // ✅ Required for IEnumerator
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class HiddenObjectBehaviour2 : MonoBehaviour
{
    private Renderer rend;
    private Collider col;
    private bool isRevealed = false;
    private Coroutine fadeRoutine, hideDelayRoutine;

    [Header("Settings")]
    public string requiredTag = "Hidden";
    public float revealDistance = 8f;
    public float interactDistance = 3f;

    [Header("Inventory")]
    [SerializeField] private InventoryItem itemToAdd;
    [SerializeField] private InventoryBehaviour inventory;

    [Header("References")]
    public Light uvFlashlight;            // Tagged "UVLight" in scene
    public Transform player;              // Tagged "Player"
    public TextMeshProUGUI messageText;   // Assign in inspector

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        HideInstant();

        // Auto-assign player and UV light if not set
        player ??= GameObject.FindGameObjectWithTag("Player")?.transform;
        uvFlashlight ??= GameObject.FindGameObjectWithTag("UVLight")?.GetComponent<Light>();
        inventory ??= FindObjectOfType<InventoryBehaviour>();

        ClearMessage();
    }

    void Update()
    {
        if (!uvFlashlight || !player)
            return;

        HandleRevealLogic();
        HandleInteraction();
    }

    // ----------------- Reveal Logic -----------------
    void HandleRevealLogic()
    {
        bool lightOn = uvFlashlight.enabled && uvFlashlight.gameObject.activeInHierarchy;

        if (!lightOn)
        {
            Hide();
            return;
        }

        Vector3 toObj = transform.position - uvFlashlight.transform.position;
        float dist = toObj.magnitude;
        float angle = Vector3.Angle(uvFlashlight.transform.forward, toObj);

        bool inCone = dist < revealDistance && angle < uvFlashlight.spotAngle * 0.5f;

        if (inCone)
        {
            if (hideDelayRoutine != null)
                StopCoroutine(hideDelayRoutine);
            Reveal();
        }
        else if (hideDelayRoutine == null)
        {
            hideDelayRoutine = StartCoroutine(HideAfterDelay(0.25f)); // buffer to prevent flicker
        }
    }

    // ----------------- Interaction Logic -----------------
    void HandleInteraction()
    {
        if (!isRevealed)
        {
            ClearMessage();
            return;
        }

        float d = Vector3.Distance(player.position, transform.position);

        if (d <= interactDistance)
        {
            ShowMessage($"<b><color=#A020F0>Press [E]</color></b> to pick up {itemToAdd.itemName}");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log($"[HiddenObject] {name} picked up with E.");

                if (CompareTag(requiredTag))
                {
                    if (inventory != null && itemToAdd != null)
                        inventory.AddInventoryItem(itemToAdd);
                }

                Destroy(gameObject);
                ClearMessage();
            }
        }
        else
        {
            ClearMessage();
        }
    }

    // ----------------- Visibility Control -----------------
    void HideInstant()
    {
        rend.enabled = false;
        col.enabled = false;
    }

    IEnumerator FadeVisibility(bool visible)
    {
        float duration = 0.4f;
        float start = rend.material.color.a;
        float end = visible ? 1f : 0f;
        Color c = rend.material.color;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            c.a = Mathf.Lerp(start, end, t / duration);
            rend.material.color = c;
            yield return null;
        }

        c.a = end;
        rend.material.color = c;
        rend.enabled = visible;
    }

    public void Reveal()
    {
        if (isRevealed) return;
        isRevealed = true;
        col.enabled = true;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeVisibility(true));

        gameObject.layer = LayerMask.NameToLayer("HiddenTest");
    }

    public void Hide()
    {
        if (!isRevealed) return;
        isRevealed = false;
        col.enabled = false;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeVisibility(false));

        gameObject.layer = LayerMask.NameToLayer("Default");
        ClearMessage();
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Hide();
        hideDelayRoutine = null;
    }

    // ----------------- UI Message -----------------
    void ShowMessage(string text)
    {
        if (messageText != null)
            messageText.text = text;
    }

    void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    // ----------------- Gizmos -----------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, revealDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
