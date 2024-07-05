using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public SkillType skillType;
    public int level;
    public int health;
    public float speed;
    // public int exp = 0;
    
    // public int expBaseCost;
    // private const int scalingMul = 15;
    // private const int expIncrement = 135;
    //
    //
    // public int expRealCost
    // {
    //     get
    //     {
    //         return expBaseCost + (level - 1) * expIncrement + scalingMul * (level - 1) * (level - 1);
    //     }
    // }
}
public enum SkillType
{
    Tank,
    DPS,
    Heal,
}