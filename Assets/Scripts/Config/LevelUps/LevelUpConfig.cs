using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: all Level Up buff are PERMANENT (unlike pick-up buff, which is NOT permanent (has a duration))
public class LevelUpConfig : ScriptableObject
{
    //every player start at level ONE (1)
    public int level;
    // all of the increase value below are general for ALL level. 
    // each level may have other specific increase value.
    public int healthIncrease;
    public float speedIncrease;
    public float damageIncrease;
    public float critRateIncrease;
    public float critDamageIncrease;
    public float lifeStealIncrease;
    // a list of some random weighted level up increments
    public virtual int[] getHealthIncrements() { return new int[] { }; }
    public virtual float[] getSpeedIncrements() { return new float[] { }; }
    public virtual float[] getDamageIncrements() { return new float[] { }; }
    public virtual float[] getCritRateIncrements() { return new float[] { }; }
    public virtual float[] getCritDamageIncrements() { return new float[] { }; }
    public virtual float[] getLifeStealIncrements() { return new float[] { }; }

    // a list of variable for non base stat upgrade
    public float warpAmount;

    public List<string> additionalOptions = new List<string>();
    List<string> finalOptions = new List<string>();
    public virtual void RandomLevelUpChoice()
    {
        finalOptions.Clear(); // Clear for each level
        Debug.Log("Additional options: " + string.Join(", ", additionalOptions.ToArray()));
        // List<string> defaultOptions = new List<string> { "health", "speed", "damage", "crit", "lifeSteal" };
        List<string> defaultOptions = new List<string> { "health" };
        defaultOptions.AddRange(additionalOptions);

        for (int i = 0; i < defaultOptions.Count; i++)
        {
            string temp = defaultOptions[i];
            int randomIndex = Random.Range(0, defaultOptions.Count);
            defaultOptions[i] = defaultOptions[randomIndex];
            defaultOptions[randomIndex] = temp;
        }
        for (int i = 0; i < defaultOptions.Count && finalOptions.Count < 3; i++)
        {
            if (!finalOptions.Contains(defaultOptions[i]))
            {
                finalOptions.Add(defaultOptions[i]);
            }
        }
        additionalOptions.Clear();
    }

    public virtual void ApplyBaseStat(string buff = "")
    {
        switch (buff)
        {
            case "health":
                var healthIncrements = getHealthIncrements()[Random.Range(0, getHealthIncrements().Length)];
                Debug.Log("Level up: Health increased by " + healthIncrements);
                healthIncrease += healthIncrements;
                break;
            case "speed":
                var speedIncrements = getSpeedIncrements()[Random.Range(0, getSpeedIncrements().Length)];
                Debug.Log("Level up: Speed increased by " + speedIncrements);
                speedIncrease += speedIncrements;
                break;
            case "damage":
                var damageIncrements = getDamageIncrements()[Random.Range(0, getDamageIncrements().Length)];
            Debug.Log("Level up: Damage increased by " + damageIncrements);
                damageIncrease += damageIncrements;
                break;
            case "crit":
                var critRateIncrements = getCritRateIncrements()[Random.Range(0, getCritRateIncrements().Length)];
                var critDamageIncrements = getCritDamageIncrements()[Random.Range(0, getCritDamageIncrements().Length)];
            Debug.Log("Level up: Crit rate increased by " + critRateIncrements + " and Crit damage increased by " + critDamageIncrements);
                critRateIncrease += critRateIncrements;
                critDamageIncrease += critDamageIncrements;
                break;
            case "lifeSteal":
                var lifeStealIncrements = getLifeStealIncrements()[Random.Range(0, getLifeStealIncrements().Length)];
                Debug.Log("Level up: Life steal increased by " + lifeStealIncrements);
                lifeStealIncrease += lifeStealIncrements;
                break;
            default:
                Debug.Log("No base buff applied");
                break;
        }
    }
    public virtual void ApplyNonBaseStat(string buff = "")
    {
        Debug.Log("Applying additional options");
        switch (buff)
        {
            case "AOE Meteor Strike":
                Debug.Log("AOE Meteor Strike called");
                break;
            case "SkillCD":
                Debug.Log("SkillCD called");
                break;
            case "Health Regen":
                Debug.Log("Health Regen called");
                break;
            case "Time Warp":
                Debug.Log("Time Warp called");
                TimeWarpLevelUp();
                break;
            case "Poison Aura":
                Debug.Log("Poison Aura called");
                break;
            // TODO: etc
            default:
                Debug.Log("No non base buff applied");
                break;
        }
    }
    //TODO: implement this for each level up UI (player choose upgrade) @Quoc
    public virtual string FinalChoice()
    {
        Debug.Log("final options list are: " + string.Join(", ", finalOptions.ToArray()));
        string chosenOption = finalOptions[Random.Range(0, finalOptions.Count)];
        Debug.Log("Chosen option: " + chosenOption);
        return chosenOption;
    }
    public virtual void ApplyChoice()
    {
        RandomLevelUpChoice();
        var chosen = FinalChoice();
        ApplyBaseStat(chosen);
        ApplyNonBaseStat(chosen);
    }

}
