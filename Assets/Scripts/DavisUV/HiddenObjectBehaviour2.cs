using UnityEngine;

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

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        if (rend != null)
            Hidden();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform; // fallback

        if (uvFlashlight == null)
            uvFlashlight = FindObjectOfType<Light>(); // fallback
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
        if (!isRevealed) return;

        float distToPlayer = Vector3.Distance(player.position, transform.position);

        if (distToPlayer <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"[HiddenObject] {name} picked up with E.");

            if (CompareTag(requiredTag))
            {
                if (inventory != null && itemToAdd != null)
                    inventory.AddInventoryItem(itemToAdd);
            }

            Destroy(gameObject);
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
        }
    }

    // 🟣 Debug visualization in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, revealDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}

