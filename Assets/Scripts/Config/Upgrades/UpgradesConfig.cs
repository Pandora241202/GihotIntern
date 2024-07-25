using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesConfig", menuName = "Config/UpgradesConfig")]
public class UpgradesConfig :ScriptableObject
{
    public List<Upgrade> lsUpgradesConfig;
}
[System.Serializable]
public class Upgrade
{
    public string name;
    public string des;
    public int basePrice;
    public int priceScale;
}

