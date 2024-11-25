using UnityEngine;
using UnityEngine.UI;

public class CritterpediaSlot : MonoBehaviour
{
    public Critter assignedCritter; // Pre-assigned critter for this slot
    public Image critterImage;      // Image to display the critter logo
    public bool isCollected = false; // Tracks whether the critter has been collected
    private CritterpediaManager critterpediaManager; // Reference to the Critterpedia manager

    private void Awake()
    {
        if (critterImage == null)
        {
            Debug.LogError("Critter Image is not assigned on CritterpediaSlot.", this);
        }

        // Set the slot to inactive initially
        gameObject.SetActive(false);
    }

    public void SetCritterpediaManager(CritterpediaManager manager)
    {
        critterpediaManager = manager;
    }

    public void MarkAsCollected()
    {
        if (isCollected)
        {
            Debug.LogWarning($"Critter {assignedCritter.critterName} is already collected.");
            return;
        }

        isCollected = true;

        if (assignedCritter != null && critterImage != null)
        {
            critterImage.sprite = assignedCritter.logo; // Set the critter's logo
            gameObject.SetActive(true); // Make the slot visible
            Debug.Log($"Slot unlocked for critter: {assignedCritter.critterName}");
        }
    }

    public void OnSlotTapped()
    {
        if (!isCollected)
        {
            Debug.Log("This critter has not been collected yet!");
            return;
        }

        if (critterpediaManager != null && assignedCritter != null)
        {
            critterpediaManager.ShowCritterDetailsImage(assignedCritter);
            Debug.Log($"Tapped on slot with critter: {assignedCritter.critterName}");
        }
    }
}
