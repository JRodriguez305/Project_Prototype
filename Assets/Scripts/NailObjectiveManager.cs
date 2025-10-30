using UnityEngine;

public class NailObjectiveManager : MonoBehaviour
{
    public static NailObjectiveManager Instance;

    [Header("Objective Settings")]
    public int totalNails = 4;
    private int collectedNails = 0;

    [Header("Reward Object")]
    public GameObject rewardObject; // assign in Inspector

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (rewardObject != null)
            rewardObject.SetActive(false); // hide at start
    }

    public void AddCollectedNail()
    {
        collectedNails++;
        Debug.Log($"🔩 Nail collected: {collectedNails}/{totalNails}");

        if (collectedNails >= totalNails)
        {
            AllNailsCollected();
        }
    }

    private void AllNailsCollected()
    {
        Debug.Log("✅ All nails collected! Revealing reward object.");

        if (rewardObject != null)
        {
            rewardObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ Reward object is not assigned in the NailObjectiveManager!");
        }
    }
}
