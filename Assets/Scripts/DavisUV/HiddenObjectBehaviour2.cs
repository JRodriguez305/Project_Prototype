using UnityEngine;
using TMPro;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class HiddenObjectBehaviour2 : MonoBehaviour
{
    private Renderer rend;
    private Collider col;
    private bool isRevealed = false;

    [Header("Settings")]
    public string requiredTag = "Hidden";
    public float revealDistance = 8f;
    public float interactDistance = 3f;

    [Header("Inventory")]
    [SerializeField] private InventoryItem itemToAdd;
    [SerializeField] private InventoryBehaviour inventory;

    [Header("References")]
    public Light uvFlashlight;
    public Transform player;
    public TextMeshProUGUI messageText; // Assign your UI text here

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        Hidden();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (uvFlashlight == null)
            uvFlashlight = FindObjectOfType<Light>();

        ClearMessage();
    }

    void Update()
    {
        if (uvFlashlight == null || player == null)
            return;

        HandleRevealLogic();
        HandleInteraction();
    }

    void HandleRevealLogic()
    {
        if (uvFlashlight.enabled)
        {
            Vector3 toObject = transform.position - uvFlashlight.transform.position;
            float distance = toObject.magnitude;
            float angle = Vector3.Angle(uvFlashlight.transform.forward, toObject);

            bool inCone = distance < revealDistance && angle < uvFlashlight.spotAngle * 0.5f;

            if (inCone)
                Reveal();
            else
                Hide();
        }
        else
        {
            Hide();
        }
    }

    void HandleInteraction()
    {
        if (!isRevealed)
        {
            ClearMessage();
            return;
        }

        float distToPlayer = Vector3.Distance(player.position, transform.position);

        if (distToPlayer <= interactDistance)
        {
            ShowMessage("Press E to pick up " + itemToAdd.itemName);

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

    void Hidden()
    {
        rend.enabled = false;
        col.enabled = false;
    }

    public void Reveal()
    {
        if (!isRevealed)
        {
            isRevealed = true;
            rend.enabled = true;
            col.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("HiddenTest");
        }
    }

    public void Hide()
    {
        if (isRevealed)
        {
            isRevealed = false;
            rend.enabled = false;
            col.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            ClearMessage();
        }
    }

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, revealDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
