using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CritterpediaManager : MonoBehaviour
{
    [SerializeField] private GameObject critterpediaButton;
    [SerializeField] private CanvasGroup critterpediaCanvasGroup;
    [SerializeField] private Transform critterSlotParent;
    [SerializeField] private List<Critter> allCritters; // Predefined list of critters
    [SerializeField] public GameObject critterDetailsImage;
    public GameObject darkOverlay; // Reference to the Dark Overlay


    private List<CritterpediaSlot> critterSlots = new List<CritterpediaSlot>();

    private void Start()
    {
        critterpediaButton.SetActive(false); // Ensure the button is initially inactive
        critterpediaCanvasGroup.alpha = 0f; // Hide the Critterpedia initially
        critterpediaCanvasGroup.interactable = false;
        critterpediaCanvasGroup.blocksRaycasts = false;

        critterDetailsImage.SetActive(false); // Ensure the critter details image is hidden initially
        darkOverlay.SetActive(false);

        InitializeSlots(); // Pre-assign critters to slots
    }

    private void InitializeSlots()
    {
        foreach (Transform child in critterSlotParent)
        {
            CritterpediaSlot slot = child.GetComponent<CritterpediaSlot>();
            if (slot != null)
            {
                int slotIndex = critterSlots.Count;
                // Check if there's a corresponding critter
                if (slotIndex < allCritters.Count)
                {
                    slot.assignedCritter = allCritters[slotIndex]; // Pre-assign critter
                    slot.SetCritterpediaManager(this);
                }
                else
                {
                    // No critter for this slot, leave it hidden
                    slot.gameObject.SetActive(false);
                }

                critterSlots.Add(slot);
            }
        }

        Debug.Log("Critterpedia slots initialized.");
    }


    public void AddCritterToPedia(Critter critter)
    {
        if (critter == null)
        {
            Debug.LogWarning("Cannot add a null critter to the Critterpedia.");
            return;
        }

        foreach (CritterpediaSlot slot in critterSlots)
        {
            if (slot.assignedCritter == critter && !slot.isCollected)
            {
                slot.MarkAsCollected();
                return;
            }
        }

        Debug.LogWarning($"No slot found for critter: {critter.critterName}");
    }

    public void ShowCritterDetailsImage(Critter critter)
    {
        if (critterDetailsImage == null || critter == null)
        {
            Debug.LogWarning("CritterDetailsImage or critter is null.");
            return;
        }

        darkOverlay.SetActive(true);
        critterDetailsImage.SetActive(true);
        Image imageComponent = critterDetailsImage.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = critter.infoCard; // Set the critter's info card image
            Debug.Log($"Displayed details for critter: {critter.critterName}");
        }
        else
        {
            Debug.LogWarning("Image component not found on CritterDetailsImage.");
        }
    }

    public void OpenCritterpedia()
    {
        critterpediaCanvasGroup.alpha = 1f;
        critterpediaCanvasGroup.interactable = true;
        critterpediaCanvasGroup.blocksRaycasts = true;
        Debug.Log("Critterpedia opened.");
    }

    public void CloseCritterpedia()
    {
        critterpediaCanvasGroup.alpha = 0f;
        critterpediaCanvasGroup.interactable = false;
        critterpediaCanvasGroup.blocksRaycasts = false;
        Debug.Log("Critterpedia closed.");
    }

    public void ActivateCritterpediaButton()
    {
        critterpediaButton.SetActive(true);
        Debug.Log("Critterpedia button activated.");
    }

    public void CloseInfoCard()
    {
        critterDetailsImage.SetActive(false);
        darkOverlay.SetActive(false);
    }

    public void ShowInfoCard(Critter critter)
    {
        critterDetailsImage.SetActive(true);
        darkOverlay.SetActive(true);
    }

}
