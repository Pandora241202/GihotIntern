using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/LevelUpConfig")]

// NOTE: all Level Up buff are PERMANENT (unlike pick-up buff, which is NOT permanent (has a duration))
public class LevelUpConfig : ScriptableObject
{
    //every player start at level ONE (1)
    public int level;
    // all of the increase value below are general for ALL level. 
    // each level may have other specific increase value.
    // public int healthIncrease;
    // public float speedIncrease;
    // public float damageIncrease;
    // public float critRateIncrease;
    // public float critDamageIncrease;
    // public float lifeStealIncrease;
    // a list of some random weighted level up increments

    // a list of variable for non base stat upgrade
    public float warpAmount;

    public List<string> additionalOptions = new List<string>();
    public List<string> finalOptions = new List<string>();

    public Dictionary<string,int> levelUpDict = new Dictionary<string,int>()
        {{"health",0},{"speed",0},{"damage",0},{"crit",0},{"lifeSteal",0}};
    public List<string> defaultOptions = new List<string> { "health", "speed", "damage", "crit", "lifeSteal" };
    private bool isLoadedDict = false;
    public void OnSetUpLevelUpDict()
    {
        foreach (var choice in defaultOptions)
        {
            levelUpDict.Add(choice,0);
        }
    }
    public virtual void RandomLevelUpChoice()
    {
        finalOptions.Clear(); // Clear for each level
        Debug.Log("Additional options: " + string.Join(", ", additionalOptions.ToArray()));
        
        //List<string> defaultOptions = new List<string> { "health" };
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
        //Debug.Log("Final options: " + string.Join(", ", finalOptions.ToArray()));
        additionalOptions.Clear();
    }

    public virtual void ApplyBaseStat(string buff = "")
    {
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        // int level;
       // Debug.Log(buff);
        switch (buff)
        {
            case "health":
                levelUpDict[buff]++; // Note: increase level first, else first level = 0 -> 1 * 0 = 0
                var healthBoost = levelUpDict[buff];
                player.ChangeHealth(1  * healthBoost);
                //to do: + max health

                break;
            case "speed":
                levelUpDict[buff]++;
                var speedBoost = levelUpDict[buff];
                player.SetSpeedBoostByLevelUp(speedBoost);
                break;

            case "damage":
                levelUpDict[buff]++;
                var damageBoost = levelUpDict[buff];
                //Debug.Log("Level up: Damage increased by " + damageBoost);
                player.SetDamageBoost(damageBoost);
                break;
            case "crit":
            //TODO: Rebase then add this later @Hung
                levelUpDict[buff]++;
                // level = levelUpDict[buff];
                // critRateIncrease += critRateIncrements;
                // critDamageIncrease += critDamageIncrements;
                break;
            case "lifeSteal":
                levelUpDict[buff]++;
                var lifeStealBuff = levelUpDict[buff];
                player.lifeSteal += 0.01f * lifeStealBuff;
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
            //TODO: apply non base stat later
            case "AOE Meteor":
                Debug.Log("AOE Meteor Strike called");
                // AOEMeteorStrikeLevelUp();
                break;
            case "SkillCD":
                Debug.Log("SkillCD called");
                break;
            case "Health Regen":
                Debug.Log("Health Regen called");
                break;
            case "Time Warp": 
                Debug.Log("Time Warp called");
                // TimeWarpLevelUp();
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
    
    public virtual void OpenMenu()
    {
        RandomLevelUpChoice();
    }

    // Time Warp Level Up: PERMANENTLY decrease the speed of all creeps by a small amount
    public virtual void TimeWarpLevelUp(float warpAmount = 0) // Note: debuff -> decrease speed -> warpAmount is a negative number
    {
        Debug.Log("Time Warp Amount: "  + warpAmount);
        foreach (var creep in AllManager.Instance().creepManager.GetCreepActiveDict())
        {
            creep.Value.speed *= 1 + warpAmount;
        }
    }

    // AOEMeteorStrikeLevelUp: deal damage to every creep inside player's radius.
    public virtual void AOEMeteorStrikeLevelUp(int damage = 0, float radius = 0)
    {
        Debug.Log("AOE Meteor Damage Amount: "  + damage);
        Debug.Log("radius of meteor: " + radius);
        Dictionary<int, Creep> activeCreeps = AllManager.Instance().creepManager.GetCreepActiveDict();
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        var playerPosition = player.playerTrans;
        foreach (var creep in activeCreeps)
        {
            var distance = Vector3.Distance(playerPosition.position, creep.Value.creepTrans.position);
            if (distance <= radius)
            {
                creep.Value.ProcessDmg(damage, Player_ID.MyPlayerID);
                Debug.Log("Creep hit by meteor");
            }
        }
    }
}
