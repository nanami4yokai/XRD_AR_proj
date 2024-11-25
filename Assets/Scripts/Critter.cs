using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Critter")]
public class Critter : ScriptableObject
{
    public string critterName;         // Name of the critter
    public Sprite logo;                // Logo for the Critterpedia slot
    public Sprite infoCard;            // Detailed info card image
    public Sprite animationSprite;     // Sprite used for animations in AR identification
    public RuntimeAnimatorController animatorController; // Animator for critter animations
    public string description;         // Additional details if needed

    public override bool Equals(object obj)
    {
        if (obj is Critter otherCritter)
        {
            bool result = this.critterName.Equals(otherCritter.critterName);
            Debug.Log($"Comparing Critters: {this.critterName} == {otherCritter.critterName}: {result}");
            return result;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = critterName != null ? critterName.GetHashCode() : 0;
        Debug.Log($"HashCode for Critter {critterName}: {hash}");
        return hash;
    }

}
