using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterCollectingManager : MonoBehaviour
{
    [SerializeField]
    private List<Critter> allCritters; // List of all possible critters

    [SerializeField]
    private GameObject blathersButton;

    [SerializeField]
    private GameObject uiElements; // UI elements to display after identification

    [SerializeField]
    private float uiDelay = 2.0f; // Delay before showing the UI

    [SerializeField]
    private GameObject emperorModel;

    [SerializeField]
    private GameObject monarchModel;

    [SerializeField]
    private GameObject peacockModel;

    private Dictionary<string, GameObject> critterModelMap = new Dictionary<string, GameObject>();
    private Dictionary<string, string> referenceImageToCritterName = new Dictionary<string, string>
    {
        { "Insect_Card_Emperor", "Emperor Butterfly" },
        { "Insect_Card_Monarch", "Monarch Butterfly" },
        { "Insect_Card_Peacock", "Peacock Butterfly" }
    };
    private HashSet<Critter> collectedCritters = new HashSet<Critter>();
    private int collectedCritterCount = 0;

    [SerializeField]
    private CritterCollector critterCollector;
    private CritterpediaManager critterpediaManager;

    private void Start()
    {
        Debug.Log($"Total critters in allCritters before initialization: {allCritters.Count}");
        foreach (var critter in allCritters)
        {
            Debug.Log(
                $"Critter in allCritters: {critter.critterName}, HashCode: {critter.GetHashCode()}"
            );
        }

        ClearCritterData();
        InitializeCritterModelMap();
        blathersButton.SetActive(false);
        uiElements.SetActive(false);

        critterpediaManager = FindObjectOfType<CritterpediaManager>();
        if (critterpediaManager == null)
        {
            Debug.LogError("CritterpediaManager instance not found!");
        }
    }

    private void InitializeCritterModelMap()
    {
        foreach (Critter critter in allCritters)
        {
            if (critter.critterName == "Emperor Butterfly" && emperorModel != null)
                critterModelMap[critter.critterName] = emperorModel;
            else if (critter.critterName == "Monarch Butterfly" && monarchModel != null)
                critterModelMap[critter.critterName] = monarchModel;
            else if (critter.critterName == "Peacock Butterfly" && peacockModel != null)
                critterModelMap[critter.critterName] = peacockModel;
        }

        Debug.Log($"CritterModelMap initialized. Total critters: {critterModelMap.Count}");
    }

    public void ClearCritterData()
    {
        collectedCritters.Clear();
        collectedCritterCount = 0;
        Debug.Log($"Critter data cleared. CollectedCritters count: {collectedCritters.Count}");
    }

    public Critter GetCritterByName(string referenceImageName)
    {
        if (referenceImageToCritterName.TryGetValue(referenceImageName, out string critterName))
        {
            foreach (Critter critter in allCritters)
            {
                if (critter.critterName == critterName)
                {
                    Debug.Log($"GetCritterByName found critter: {critter.critterName}");
                    Debug.Log($"Mapping '{referenceImageName}' to critter '{critterName}'");

                    return critter;
                }
            }
        }

        Debug.LogWarning($"Critter data not found for image name: {referenceImageName}");
        return null;
    }

    public void IdentifyCritter(Critter critter)
    {
        if (critter == null)
        {
            Debug.LogWarning("Critter is null in IdentifyCritter.");
            return;
        }

        Debug.Log($"Identifying critter: {critter.critterName}");

        if (collectedCritters.Contains(critter))
        {
            Debug.Log($"Critter already collected: {critter.critterName}");
            ShowModelOnly(critter.critterName);
        }
        else
        {
            Debug.Log($"New critter identified: {critter.critterName}");
            ShowModelOnly(critter.critterName);
            critterCollector.ShowCritter(critter);
            StartCoroutine(ShowUIDelayed(critter));
        }
    }

    public bool AddToPedia(Critter critter)
    {
        if (critter == null)
        {
            Debug.LogWarning("Cannot add null critter to Pedia.");
            return false;
        }

        if (collectedCritters.Contains(critter))
        {
            Debug.LogWarning($"Critter already collected: {critter.critterName}");
            return false;
        }

        collectedCritters.Add(critter);
        collectedCritterCount++;
        Debug.Log($"Added {critter.critterName} to collectedCritters.");

        if (critterpediaManager != null)
        {
            critterpediaManager.AddCritterToPedia(critter);
            Debug.Log($"Critter {critter.critterName} added to Critterpedia.");
        }
        else
        {
            Debug.LogWarning("CritterpediaManager instance not found!");
        }

        if (collectedCritterCount == 3)
        {
            Debug.Log("Collected 3 critters. Unlocking Blathers' button.");
            ActivateBlathersButton();
        }

        return true;
    }

    private IEnumerator ShowUIDelayed(Critter critter)
    {
        yield return new WaitForSeconds(uiDelay);
        uiElements.SetActive(true);
        Debug.Log("UI shown for critter: " + critter.critterName);
    }

    private void ShowModelOnly(string critterName)
    {
        DeactivateAllModels();

        if (critterModelMap.TryGetValue(critterName, out GameObject model))
        {
            model.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No model assigned for critter: " + critterName);
        }
    }

    public void DeactivateAllModels()
    {
        foreach (var model in critterModelMap.Values)
        {
            model.SetActive(false);
        }
        Debug.Log("All models deactivated.");
    }

    private void ActivateBlathersButton()
    {
        blathersButton.SetActive(true);
        Debug.Log("Blathers' button activated.");

        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.CompleteCritterCollection();
        }
        else
        {
            Debug.LogWarning("DialogueManager instance not found!");
        }
    }
}
