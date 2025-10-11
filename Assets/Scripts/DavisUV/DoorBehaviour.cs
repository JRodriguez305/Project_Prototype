using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class UVDoorBehaviour : MonoBehaviour
{
    [Header("Settings")]
    public string requiredKeyName;
    public string sceneToLoad;
    public float interactDistance = 3f;
    public float lookAngle = 60f;         // Max angle player can look at door to show prompt

    [Header("References")]
    public Transform player;
    public InventoryBehaviour playerInventory;
    public Light uvFlashlight;
    public TextMeshProUGUI messageText;

    private Renderer rend;
    private Collider col;
    private bool isRevealed = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        rend.enabled = false;
        col.enabled = false;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerInventory == null)
            playerInventory = FindObjectOfType<InventoryBehaviour>();

        if (uvFlashlight == null)
            uvFlashlight = FindObjectOfType<Light>();

        ClearMessage();
    }

    void Update()
    {
        if (uvFlashlight == null || player == null) return;

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

            bool inCone = distance < 8f && angle < uvFlashlight.spotAngle * 0.5f;

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

        if (distToPlayer <= interactDistance && IsPlayerLookingAtDoor())
        {
            if (HasKey())
            {
                ShowMessage("Press E to open door");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
            else
            {
                ShowMessage("You need a key to open this door");
            }
        }
        else
        {
            ClearMessage();
        }
    }

    bool IsPlayerLookingAtDoor()
    {
        Vector3 toDoor = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, toDoor);
        return angle < lookAngle * 0.5f;
    }

    bool HasKey()
    {
        foreach (var item in playerInventory.GetInventoryItems())
        {
            if (item != null && item.itemName == requiredKeyName)
                return true;
        }
        return false;
    }

    void Reveal()
    {
        if (!isRevealed)
        {
            isRevealed = true;
            rend.enabled = true;
            col.enabled = true;
        }
    }

    void Hide()
    {
        if (isRevealed)
        {
            isRevealed = false;
            rend.enabled = false;
            col.enabled = false;
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
