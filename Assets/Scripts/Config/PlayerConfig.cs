using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public SkillType skillType;

    public float speed;
}
public enum SkillType
{
    Tank,
    DPS,
    Heal,
}