using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public SkillType skillType;

    public float speed;

    public GameObject levelUpEffect;
}
public enum SkillType
{
    Tank,
    DPS,
    Heal,
}