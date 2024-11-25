using UnityEngine;

public class TestUI : MonoBehaviour
{
    public CritterCollectingManager collectingManager;

    private void Awake()
    {
        collectingManager = FindObjectOfType<CritterCollectingManager>();
        if (collectingManager == null)
        {
            Debug.LogError("CritterCollectingManager instance not found!");
        }
    }

    public void SimulateEmperor()
    {
        Debug.Log("SimulateEmperor button clicked.");
        Critter critter = collectingManager.GetCritterByName("Emperor Butterfly");
        if (critter != null)
        {
            Debug.Log("Critter found: " + critter.critterName);
            collectingManager.IdentifyCritter(critter);
        }
        else
        {
            Debug.LogWarning("Critter not found: Emperor Butterfly");
        }
    }

    public void SimulateMonarch()
    {
        Debug.Log("SimulateMonarch button clicked.");
        Critter critter = collectingManager.GetCritterByName("Monarch Butterfly");
        if (critter != null)
        {
            Debug.Log("Critter found: " + critter.critterName);
            collectingManager.IdentifyCritter(critter);
        }
        else
        {
            Debug.LogWarning("Critter not found: Monarch Butterfly");
        }
    }

    public void SimulatePeacock()
    {
        Debug.Log("SimulatePeacock button clicked.");
        Critter critter = collectingManager.GetCritterByName("Peacock Butterfly");
        if (critter != null)
        {
            Debug.Log("Critter found: " + critter.critterName);
            collectingManager.IdentifyCritter(critter);
        }
        else
        {
            Debug.LogWarning("Critter not found: Peacock Butterfly");
        }
    }
}
