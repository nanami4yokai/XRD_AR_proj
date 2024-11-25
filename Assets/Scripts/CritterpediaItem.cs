using UnityEngine;
using UnityEngine.UI;

public class CritterpediaItem : MonoBehaviour
{
    public Critter critter; // Critter associated with this item
    public Image critterImage;

    public void InitializeCritter(Critter newCritter)
    {
        critter = newCritter;
        critterImage.sprite = newCritter.infoCard; // Set the critter image in the UI
    }
}
