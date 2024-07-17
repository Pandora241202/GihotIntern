using UnityEngine;

[CreateAssetMenu(fileName = "AllCreepConfig", menuName = "Config/CreepConfig/AllCreep")]
public class AllCreepConfig : ScriptableObject
{
    [SerializeField] CreepConfig[] creepConfigs;

    public CreepConfig[] CreepConfigs => creepConfigs;
}
