using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpConfig", menuName = "Config/AllLevelUpConfig")]
public class AllLevelUpConfig : ScriptableObject
{
    public List<LevelUpConfig> allLevelUpConfigList = new List<LevelUpConfig>();
}
