using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/LevelUpConfig")]

// NOTE: all Level Up buff are PERMANENT (unlike pick-up buff, which is NOT permanent (has a duration))
public class LevelUpConfig : ScriptableObject
{
    // public int level;
    public float critRateIncrements;
    public float critDamageIncrements;
    public float warpAmount;
    public GameObject aoeMeteorEffect;
    public List<string> finalOptions = new List<string>();

    public Dictionary<string,int> levelUpDict = new Dictionary<string,int>()
        {{"Health", 0},{"Speed", 0},{"Damage", 0},{"CRIT", 0},{"Life Steal", 0}, {"Fire Rate", 0}, {"AOE Meteor", 0}, {"Time Warp", 0}};
    // public List<string> defaultOptions = new List<string> { "Health", "Speed", "Damage", "CRIT", "Life Steal","Fire Rate", "AOE Meteor" , "Time Warp", "Poison Aura"};
    public List<string> defaultOptions = new List<string> { "Health", "Speed", "Fire Rate"};
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
        for (int i = 0; i < defaultOptions.Count; i++)
        {
            string temp = defaultOptions[i];
            int randomIndex = UnityEngine.Random.Range(0, defaultOptions.Count);
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
    }

    public virtual void ApplyBaseStat(string buff)
    {
        var player = AllManager.Instance().playerManager.dictPlayers[Player_ID.MyPlayerID];
        Debug.Log("Buff chosen: " + buff);
        switch (buff) //Note: Reference to how UIItem/ItemLevelUp.cs (txtName.text) is set up
        {
            case "Health":
            Debug.Log("Level Up Health called");
                levelUpDict[buff]++; // Note: increase level first, else first level = 0 -> 1 * 0 = 0
                var healthBoost = Mathf.Floor(levelUpDict[buff] * 0.5f);
                player.ChangeHealth(1  * healthBoost);
                //to do: + max health

                break;
            case "Speed":
                Debug.Log("Level Up Speed called");
                levelUpDict[buff]++;
                var speedBoost = levelUpDict[buff] * 0.05f;
                player.SetSpeedBoostByLevelUp(speedBoost);
                break;

            case "Damage":
                Debug.Log("Level Up Damage called");
                levelUpDict[buff]++;
                var damageBoost = Mathf.Floor(levelUpDict[buff] * 0.5f);
                player.SetDamageBoost(damageBoost);
                break;

            case "CRIT":
                Debug.Log("Level Up Crit called");
                levelUpDict[buff]++;
                var critRateIncrease = levelUpDict[buff] * 0.02f;
                var critDamageIncrease = levelUpDict[buff] * 0.04f;
                critRateIncrements += critRateIncrease;
                critDamageIncrements += critDamageIncrease;
                break;

            case "Life Steal":
                Debug.Log("Life Steal called");
                levelUpDict[buff]++;
                var lifeStealBuff = levelUpDict[buff]  * 0.01f;
                player.lifeSteal += lifeStealBuff;
                break;

            case "Fire Rate":
                Debug.Log("Fire Rate called");
                levelUpDict[buff]++;
                break;

            case "AOE Meteor":
                Debug.Log("AOE Meteor Strike called");
                levelUpDict[buff]++;
                GameObject aoeMeteorObj = GameObject.Instantiate(aoeMeteorEffect, player.playerTrans.position, Quaternion.identity);
                aoeMeteorObj.GetComponent<ParticleSystem>().Pause();
                player.aoeMeteorObjList.Add(aoeMeteorObj);
                AllManager.Instance().effectManager.AddEffect(aoeMeteorObj);
                break;

            case "SkillCD":
                Debug.Log("SkillCD called");
                break;

            case "Health Regen":
                Debug.Log("Health Regen called");
                break;

            case "Time Warp": 
                Debug.Log("Time Warp called");
                levelUpDict[buff]++;
                var warpAmount = levelUpDict[buff] * 0.01f;
                TimeWarpLevelUp(-warpAmount);
                break;

            case "Poison Aura":
                Debug.Log("Poison Aura called");
                break;

            default:
                Debug.Log("No base buff applied");
                break;
        }
    }
    
    public virtual void OpenMenu()
    {
        RandomLevelUpChoice();
    }

    public float getLevelUpCritRate()
    {
        return critRateIncrements;
    }
    public float getLevelUpCritDamage()
    {
        return critDamageIncrements;
    }
    public float getLevelUpFireRate()
    {
        // return (int)levelUpDict["Fire Rate"] * 0.75f;
        return levelUpDict["Fire Rate"] * 0.75f;
    }
    // Time Warp Level Up: PERMANENTLY decrease the speed of all creeps by a small amount
    public virtual void TimeWarpLevelUp(float warpAmount = 0) // Note: debuff -> decrease speed -> warpAmount is a negative number
    {
        foreach (var creep in AllManager.Instance().creepManager.GetCreepActiveDict())
        {
            creep.Value.speed *= 1 + warpAmount;
        }
    }

    // AOEMeteorStrikeLevelUp: deal one instance of damage to every creep inside player's radius.
    public virtual void AOEMeteorStrikeLevelUp(Vector3 center, float radius = 0)
    {
        int damage = levelUpDict["AOE Meteor"] * 90;
        Dictionary<int, Creep> activeCreeps = AllManager.Instance().creepManager.GetCreepActiveDict();
        foreach (var creep in activeCreeps)
        {
            var distance = Vector3.Distance(center, creep.Value.creepTrans.position);
            if (distance <= radius)
            {
                creep.Value.ProcessDmg(damage, Player_ID.MyPlayerID);
            }
        }
    }
}
