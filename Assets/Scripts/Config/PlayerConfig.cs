using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] SkillType skillType;

    [SerializeField] float baseMaxHealth;

    [SerializeField] float lifeSteal;

    [SerializeField] float baseSpeed;

    [SerializeField] GameObject levelUpEffect;

    [SerializeField] LayerMask creepLayerMask;

    public SkillType SkillType => skillType;

    public float BaseMaxHealth => baseMaxHealth;

    public float LifeSteal => lifeSteal;

    public float BaseSpeed => baseSpeed;

    public GameObject LevelUpEffect => levelUpEffect;

    public LayerMask CreepLayerMask => creepLayerMask;
}

public enum SkillType
{
    Tank,
    DPS,
    Heal,
}