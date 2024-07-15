using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Config/LevelUpConfig/Level1")]
public class Level1 : LevelUpConfig
{
    public override int[] getHealthIncrements() { return new int[] { 1, 2 }; }
    public override float[] getSpeedIncrements() { return new float[] { 0.05f, 0.1f }; }
    public override float[] getDamageIncrements() { return new float[] { 1f }; }
    public override float[] getCritRateIncrements() { return new float[] { 0.02f, 0.04f}; }
    public override float[] getCritDamageIncrements() { return new float[] { 0.04f, 0.8f}; }
    public override float[] getLifeStealIncrements() { return new float[] { 0.01f }; }

    
    public Level1()
    {
        additionalOptions.Add("AOE Meteor");
    }
    public override void ApplyNonBaseStat(string buff = "")
    {
        additionalOptions.Add("AOE Meteor");
        Debug.Log("Apply level 1 ");
        switch (buff)
        {
            case "Time Warp":
                Debug.Log("Level 1: Time Warp");
                TimeWarpLevelUp(-1f);
                break;
            case "AOE Meteor":
                Debug.Log("Level 1: AOE Meteor");
                AOEMeteorStrikeLevelUp(9999, 9999);
                break;
            default:
                Debug.Log("No buff applied");
                break;
        }
    }
}

