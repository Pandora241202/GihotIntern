using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpConfig : ScriptableObject
{
    //every player start at level ONE ()
    public int level;
    // all of the increase value below are general for ALL level. 
    // each level may have other specific increase value.
    public int healthIncrease;
    public float speedIncrease;
    public float damageIncrease;
    public float critRateIncrease;
    public float critDamageIncrease;
    public List<string> additionalOptions = new List<string>();

    // a list of some random weighted level up increments
    public virtual int[] getHealthIncrements() { return new int[] { }; }
    public virtual float[] getSpeedIncrements() { return new float[] { }; }
    public virtual float[] getDamageIncrements() { return new float[] { }; }
    public virtual float[] getCritRateIncrements() { return new float[] { }; }
    public virtual float[] getCritDamageIncrements() { return new float[] { }; }
    List<string> finalOptions = new List<string>();
    public virtual void RandomLevelUpChoice()
    {
        finalOptions.Clear(); // Clear for each level
        List<string> defaultOptions = new List<string> { "health", "speed", "damage", "crit" };
        defaultOptions.AddRange(additionalOptions);

        while (finalOptions.Count < 3)
        {
            string option = defaultOptions[Random.Range(0, defaultOptions.Count)];
            if (!finalOptions.Contains(option))
            {
                finalOptions.Add(option);
            }
        }
        Debug.Log("final options are: " + string.Join(", ", finalOptions.ToArray()));
    }

    public virtual void ApplyDefault()
    {
        foreach (string option in finalOptions)
        {
            switch (option)
            {
                case "health":
                Debug.Log("Level up: Health increased by " + getHealthIncrements()[Random.Range(0, getHealthIncrements().Length)]);
                    healthIncrease += getHealthIncrements()[Random.Range(0, getHealthIncrements().Length)];
                    break;
                case "speed":
                Debug.Log("Level up: Speed increased by " + getSpeedIncrements()[Random.Range(0, getSpeedIncrements().Length)]);
                    speedIncrease += getSpeedIncrements()[Random.Range(0, getSpeedIncrements().Length)];
                    break;
                case "damage":
                Debug.Log("Level up: Damage increased by " + getDamageIncrements()[Random.Range(0, getDamageIncrements().Length)]);
                    damageIncrease += getDamageIncrements()[Random.Range(0, getDamageIncrements().Length)];
                    break;
                case "crit":
                Debug.Log("Level up: Crit rate increased by " + getCritRateIncrements()[Random.Range(0, getCritRateIncrements().Length)] + " and Crit damage increased by " + getCritDamageIncrements()[Random.Range(0, getCritDamageIncrements().Length)]);
                    critRateIncrease += getCritRateIncrements()[Random.Range(0, getCritRateIncrements().Length)];
                    critDamageIncrease += getCritDamageIncrements()[Random.Range(0, getCritDamageIncrements().Length)];
                    break;
                default:
                    break;
            }
        }
    }
    public virtual void ApplyAdditional(string buff = "")
    {
        Debug.Log("Applying additional options");
    }
    public virtual void ApplyChoice()
    {
        RandomLevelUpChoice();
        ApplyDefault();
        ApplyAdditional();
    }
}
